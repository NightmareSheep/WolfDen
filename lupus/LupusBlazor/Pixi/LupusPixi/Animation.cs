using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class Animation
    {
        public Animation(AnimatedSprite sprite, AnimationConfiguration animationConfiguration, AnimationFactory animationFactory)
        {
            this.Sprite = sprite;
            AnimationConfiguration = animationConfiguration;
            AnimationFactory = animationFactory;
            this.Sprite.OnCompleteEvent += this.RaiseOnCompleteEvent;
            this.Sprite.OnFrameChangeEvent += this.CheckQueueFrame;
            if (sprite.Loop)
                this.QueueFrame = -1;
        }


        public AnimatedSprite Sprite { get; set; }
        public AnimationConfiguration AnimationConfiguration { get; }
        public AnimationFactory AnimationFactory { get; }

        public event Func<Task> OnCompleteEvent;
        public event Func<Task> OnQueueCompleteEvent;
        public int QueueFrame = 0;
        public bool QueueCompleteEventFired = true;

        protected async Task RaiseOnCompleteEvent()
        {
            await RaiseOnQueueCompleteEvent();

            if (OnCompleteEvent != null)
            {
                var invocationList = OnCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }



        protected async Task CheckQueueFrame(int currentFrame)
        {
            if (QueueFrame == -1)
                return;

            if (currentFrame >= QueueFrame && !QueueCompleteEventFired)
            {
                await RaiseOnQueueCompleteEvent();
            }
        }

        protected async Task RaiseOnQueueCompleteEvent()
        {
            if (!QueueCompleteEventFired)
            {
                QueueCompleteEventFired = true;

                if (OnQueueCompleteEvent != null)
                {
                    var invocationList = OnQueueCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                    foreach (var subscriber in invocationList)
                        await subscriber();
                }
            }
        }

        


        public virtual async Task Play()
        {

            QueueCompleteEventFired = false;
            if (!this.Sprite.Loop)
            {
                await this.Sprite.GotoAndPlay(0);
                await this.Sprite.SetVisibility(true);
                await Sprite.RaiseOnFrameChangeEvent(0);
            }
            else
            {
                await this.Sprite.SetVisibility(true);
                await Sprite.Play();
            }
            
        }

        public virtual async Task End()
        {
            if (!Sprite.Loop)
                await this.Sprite.Stop();
            await this.Sprite.SetVisibility(false);
        }

        public virtual async Task Dispose()
        {
            //this.Sprite.OnCompleteEvent -= this.RaiseOnCompleteEvent;
            //this.Sprite.OnFrameChangeEvent -= this.CheckQueueFrame;
            //await this.Sprite.Dispose();
            AnimationFactory.Recycle(AnimationConfiguration, this);
        }
    }
}
