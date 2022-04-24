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
        public Action DurationCallback { get; }
        public Action QueueDurationCallback { get; }
        public Action AnimationPlayerCallback { get; set; }
        protected DotNetObjectReference<Animation> objRef;
        protected bool ReadyToDispose;

        public Animation(BlazorGame game, IJSRuntime jSRuntime, string spriteId, int duration, int queueDuration, int x, int y, bool resetAnimation = false, Effects soundEffect = Effects.none, Action durationCallback = null, Action queueDurationCallback = null)
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

        public virtual void Play(Action animationPlayerCallback)
        {
            this.AnimationPlayerCallback = animationPlayerCallback;
            PixiHelper.Animation(JSRuntime, objRef, SpriteId, QueueDuration, Duration, X, Y, ResetAnimation);
            Game.AudioPlayer.PlaySound(this.SoundEffect);
        }

        [JSInvokable]
        public void QueueDurationCallBack()
        {
            if (this.QueueDurationCallback != null)
                QueueDurationCallback();

            if (this.AnimationPlayerCallback != null)
                AnimationPlayerCallback();

            if (ReadyToDispose)
                objRef.Dispose();
            ReadyToDispose = true;
        }

        [JSInvokable]
        public void DurationCallBack()
        {
            if (this.DurationCallback != null)
                DurationCallback();

            if (ReadyToDispose)
                objRef.Dispose();
            ReadyToDispose = true;
        }
    }
}
