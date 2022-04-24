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

        public event EventHandler OnCompleteEvent;
        public event EventHandler OnQueueCompleteEvent;
        public int QueueFrame = 0;
        public bool QueueCompleteEventFired = true;

        protected void RaiseOnCompleteEvent(object sender, EventArgs e)
        {
            RaiseOnQueueCompleteEvent();
            OnCompleteEvent?.Invoke(sender, e);
        }



        protected void CheckQueueFrame(object sender, int currentFrame)
        {
            if (QueueFrame == -1)
                return;

            if (currentFrame >= QueueFrame && !QueueCompleteEventFired)
            {
                 RaiseOnQueueCompleteEvent();
            }
        }

        protected void RaiseOnQueueCompleteEvent()
        {
            if (!QueueCompleteEventFired)
            {
                QueueCompleteEventFired = true;
                OnQueueCompleteEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        


        public virtual void Play()
        {

            QueueCompleteEventFired = false;
            if (!this.Sprite.Loop)
            {
                 this.Sprite.GotoAndPlay(0);
                 this.Sprite.SetVisibility(true);
                 Sprite.RaiseOnFrameChangeEvent(0);
            }
            else
            {
                 this.Sprite.SetVisibility(true);
                 Sprite.Play();
            }
            
        }

        public virtual void End()
        {
            if (!Sprite.Loop)
                 this.Sprite.Stop();
             this.Sprite.SetVisibility(false);
        }

        public virtual void Dispose()
        {
            //this.Sprite.OnCompleteEvent -= this.RaiseOnCompleteEvent;
            //this.Sprite.OnFrameChangeEvent -= this.CheckQueueFrame;
            // this.Sprite.Dispose();
            AnimationFactory.Recycle(AnimationConfiguration, this);
        }
    }
}
