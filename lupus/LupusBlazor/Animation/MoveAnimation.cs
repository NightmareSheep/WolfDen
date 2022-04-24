using LupusBlazor.Audio;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Animation
{
    public class MoveAnimation : Animation
    {
        public int EndX { get; }
        public int EndY { get; }

        public MoveAnimation(BlazorGame game, IJSRuntime jSRuntime, string spriteId, int duration, int queueDuration, int x, int y, int endX, int endY, bool resetAnimation = false, Effects soundEffect = Effects.none, Action durationCallback = null, Action queueDurationCallback = null) : base(game, jSRuntime, spriteId, duration, queueDuration, x, y, resetAnimation , soundEffect, durationCallback, queueDurationCallback)
        {
            this.EndX = endX;
            this.EndY = endY;
        }

        public override void Play(Action animationPlayerCallback)
        {
            this.AnimationPlayerCallback = animationPlayerCallback;
            PixiHelper.MoveSprite(JSRuntime, objRef, SpriteId, QueueDuration, Duration, X, Y, EndX, EndY, ResetAnimation);
            this.Game.AudioPlayer.PlaySound(this.SoundEffect);
        }
    }
}
