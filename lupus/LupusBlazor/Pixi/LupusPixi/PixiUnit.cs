using LupusBlazor.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class PixiUnit
    {
        public IJSRuntime JSRuntime { get; }
        public Container Container { get; private set; }
        private Dictionary<Animations, Animation> UnitAnimations { get; set; } = new Dictionary<Animations, Animation>();

        public Animations BaseAnimation { get; set; } = Animations.Idle;

        private Animation CurrentAnimation { get; set; }

        public async Task AddAnimation(Animations name, Animation animation)
        {
            UnitAnimations.Add(name, animation);
            await this.Container.AddChild(animation.Sprite);

            if (name != this.BaseAnimation)
                animation.OnCompleteEvent += PlayBaseAnimation;
        }
        public PixiUnit(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public async Task Initialize()
        {
            this.Container = new Container(this.JSRuntime);
            await this.Container.Initialize();
        }
        public async Task PlayAnimation(Animations animation)
        {
            if (this.CurrentAnimation != null)
                await this.CurrentAnimation.End();

            this.UnitAnimations.TryGetValue(animation, out var instance);
            if (instance == null)
                return;

            this.CurrentAnimation = instance;
            await instance.Play();
        }

        public async Task PlayBaseAnimation() => await this.PlayAnimation(this.BaseAnimation);
    }
}
