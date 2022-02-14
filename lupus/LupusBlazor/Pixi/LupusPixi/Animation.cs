using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class Animation
    {
        public AnimatedSprite Sprite { get; set; }
        public event Func<Task> OnCompleteEvent;
        public event Func<Task> OnQueueCompleteEvent;
        public int QueueFrame = 0;

        protected async Task RaiseOnCompleteEvent()
        {
            if (OnCompleteEvent != null)
            {
                var invocationList = OnCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        protected async Task RaiseOnQueueCompleteEvent(int currentFrame)
        {
            if (currentFrame != QueueFrame)
                return;

            if (OnQueueCompleteEvent != null)
            {
                var invocationList = OnQueueCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public Animation(AnimatedSprite sprite)
        {
            this.Sprite = sprite;
            this.Sprite.OnCompleteEvent += this.RaiseOnCompleteEvent;
            this.Sprite.OnFrameChangeEvent += this.RaiseOnQueueCompleteEvent;
            if (sprite.Loop)
                this.QueueFrame = -1;
        }


        public virtual async Task Play()
        {                
            
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
            this.Sprite.OnCompleteEvent -= this.RaiseOnCompleteEvent;
            this.Sprite.OnFrameChangeEvent -= this.RaiseOnQueueCompleteEvent;
            await this.Sprite.Dispose();
        }
    }
}
