using LupusBlazor.Units;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LupusBlazor.Extensions;

namespace LupusBlazor.Animation
{
    public class AnimationPlayer
    {
        private BlazorGame Game { get; set; }
        private Queue<IAnimation> AnimationQueue = new Queue<IAnimation>();
        public bool AnimationsEnabled;
        private bool Playing;

        public AnimationPlayer(IJSRuntime jSRuntime, BlazorGame game)
        {
            JSRuntime = jSRuntime;
            Game = game;
        }

        public IJSRuntime JSRuntime { get; }

        public async Task QueueAnimation(IAnimation animation)
        {
            if (!AnimationsEnabled)
                return;

            AnimationQueue.Enqueue(animation);
            if (!Playing)
            {
                Playing = true;
                await PlayAnimation();
            }
        }

        public async Task PlayAnimation()
        {
            if (AnimationQueue.Count == 0)
            {
                Playing = false;
                return;
            }

            var animation = AnimationQueue.Dequeue();
            await animation.Play(PlayAnimation);
        }

        public async Task PlayUnitAnimation(BlazorUnit unit, string animationId, int duration, Audio.Effects sound = Audio.Effects.none, Func<Task> callback = null)
        {
            await PixiHelper.SetSpriteVisible(JSRuntime, unit.Id + " Idle", false);
            var animation = new Animation(this.Game, this.JSRuntime, unit.Id + " " + animationId, duration, 0, unit.Tile.XCoord(), unit.Tile.YCoord(), true, sound, 
                async () => {
                    if (callback != null)
                        await callback();

                    await PixiHelper.SetSpriteVisible(JSRuntime, unit.Id + " Idle", true);
                }
                );

            await this.QueueAnimation(animation);
        }
    }
}
