using LupusBlazor.Audio;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Animation
{
    public class Animation : IAnimation
    {
        public BlazorGame Game { get; }
        public IJSRuntime JSRuntime { get; }
        public string SpriteId { get; }
        public int Duration { get; }
        public int QueueDuration { get; set; }
        public int X { get; }
        public int Y { get; }
        public bool ResetAnimation { get; }
        public Effects SoundEffect { get; }
        public Func<Task> DurationCallback { get; }
        public Func<Task> QueueDurationCallback { get; }
        public Func<Task> AnimationPlayerCallback { get; set; }
        protected DotNetObjectReference<Animation> objRef;
        protected bool ReadyToDispose;

        public Animation(BlazorGame game, IJSRuntime jSRuntime, string spriteId, int duration, int queueDuration, int x, int y, bool resetAnimation = false, Effects soundEffect = Effects.none, Func<Task> durationCallback = null, Func<Task> queueDurationCallback = null)
        {
            Game = game;
            JSRuntime = jSRuntime;
            SpriteId = spriteId;
            Duration = duration;
            QueueDuration = queueDuration;
            this.X = x;
            this.Y = y;
            ResetAnimation = resetAnimation;
            SoundEffect = soundEffect;
            DurationCallback = durationCallback;
            QueueDurationCallback = queueDurationCallback;
            objRef = DotNetObjectReference.Create(this);
        }

        public virtual async Task Play(Func<Task> animationPlayerCallback)
        {
            this.AnimationPlayerCallback = animationPlayerCallback;
            await PixiHelper.Animation(JSRuntime, objRef, SpriteId, QueueDuration, Duration, X, Y, ResetAnimation);
            await this.Game.AudioPlayer.PlaySound(this.SoundEffect);
        }

        [JSInvokable]
        public async Task QueueDurationCallBack()
        {
            if (this.QueueDurationCallback != null)
                await QueueDurationCallback();

            if (this.AnimationPlayerCallback != null)
                await this.AnimationPlayerCallback();

            if (ReadyToDispose)
                objRef.Dispose();
            ReadyToDispose = true;
        }

        [JSInvokable]
        public async Task DurationCallBack()
        {
            if (this.DurationCallback != null)
                await DurationCallback();

            if (ReadyToDispose)
                objRef.Dispose();
            ReadyToDispose = true;
        }
    }
}
