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

        private async Task RaiseOnCompleteEvent()
        {
            if (OnCompleteEvent != null)
            {
                var invocationList = OnCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        private async Task RaiseOnQueueCompleteEvent()
        {
            if (OnCompleteEvent != null)
            {
                var invocationList = OnQueueCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public Animation(AnimatedSprite sprite)
        {
            this.Sprite = sprite;
            this.OnCompleteEvent += this.End;
            this.Sprite.OnCompleteEvent += this.RaiseOnCompleteEvent;
        }


        public async Task Play()
        {
            await this.Sprite.GotoAndPlay(0);
            await this.Sprite.SetVisibility(true);
        }

        public async Task End()
        {
            await this.Sprite.Stop();
            await this.Sprite.SetVisibility(false);
        }
    }
}
