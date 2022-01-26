using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Other;
using LupusBlazor.Animation;
using LupusBlazor.Audio;
using Microsoft.JSInterop;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class AnimationFactory
    {
        public IJSRuntime JSRuntime { get; set; }
        public Application Application { get; }
        public AudioPlayer Audioplayer { get; }
        private JavascriptHelperModule JavascriptHelper { get; set; }

        public AnimationFactory(IJSRuntime jSRuntime, Application application, AudioPlayer audioplayer)
        {
            JSRuntime = jSRuntime;
            Application = application;
            Audioplayer = audioplayer;
        }

        public async Task<AnimationFactory> Initialize()
        {
            this.JavascriptHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            return this;
        }

        public async Task<Animation> GetAnimation(Actors actor, Animations animation, Direction direction)
        {
            Animation result;
            var audioPlayer = this.Audioplayer;

            switch (animation)
            {
                case Animations.Idle:
                    switch (actor)
                    {
                        case Actors.Chest:
                            result = await GetDirectionAnimation(actor, Direction.None, "idle", new() { 100 }, new() { 0 });
                            break;
                        default:
                            result = await GetDirectionAnimation(actor, Direction.None, "idle", new() { 640, 80, 640, 80 }, new() { 0, 1, 2, 1 });
                            break;
                    }

                    var hitArea = await JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Rectangle" }, new() { -8, -8, 16, 16 });
                    await JavascriptHelper.SetJavascriptProperty(new string[] { "hitArea" }, hitArea, result.Sprite.JSInstance);
                    await hitArea.DisposeAsync();
                    result.QueueFrame = -1;
                    await result.Sprite.SetLoop(true);
                    return result;
                case Animations.Attack:
                case Animations.MissedAttack:
                case Animations.Pull:

                    // Sprites do not have an animation for west so we take east and mirror it.
                    var animationDirection = direction == Direction.West ? Direction.East : direction;

                    result = await GetDirectionAnimation(actor, animationDirection, "attack", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 });
                    if (result == null) { return null; }

                    Effects soundEffect = Effects.none;
                    if (animation == Animations.Attack)
                        soundEffect = Effects.SwordStrikesArmor;
                    if (animation == Animations.MissedAttack)
                        soundEffect = Effects.DaggerWoosh;
                    if (animation == Animations.Pull)
                        soundEffect = Effects.TakeASipOfWater;

                    result.Sprite.OnFrameChangeEvent += async (int frame) => { if (frame == 1) { await audioPlayer.PlaySound(soundEffect); } };
                    result.QueueFrame = 2;
                    if (direction == Direction.West)
                        result.Sprite.ScaleX = -1f;
                    return result;
                case Animations.ShortDamaged:
                    switch (direction)
                    {
                        case Direction.North:
                            return await GetDirectionAnimation(actor, Direction.North, "damaged", new() { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                        case Direction.East:
                            return await GetDirectionAnimation(actor, Direction.East, "damaged", new() { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                        case Direction.South:
                            return await GetDirectionAnimation(actor, Direction.South, "damaged", new() { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                        case Direction.West:
                            result = await GetDirectionAnimation(actor, Direction.East, "damaged", new() { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                            if (result == null) { return null; }
                            result.Sprite.ScaleX = -1f;
                            return result;
                    }
                    break;
                case Animations.Open:
                    result = await GetDirectionAnimation(actor, Direction.None, "opening", new() { 4, 80, 80, 640 }, new() { 0, 1, 2, 3 });
                    return result;
            }

            return null;
        }

        public async Task<MovingAnimation> GetMovingAnimation(Actors actor, Animations animation, Direction direction)
        {
            MovingAnimation result;
            var audioPlayer = this.Audioplayer;

            var xDirection = 0;
            var yDirection = 0;

            switch (direction)
            {
                case Direction.North:
                    yDirection = -16;
                    break;
                case Direction.East:
                    xDirection = 16;
                    break;
                case Direction.South:
                    yDirection = 16;
                    break;
                case Direction.West:
                    xDirection = -16;
                    break;
            }


            switch (animation)
            {
                
                case Animations.Damaged:

                    AnimatedSprite sprite = null;

                    switch (actor)
                    {
                        case Actors.Chest:
                            sprite = await GetDirectionSprite(actor, Direction.None, "idle", new() { 100 }, new() { 0 });

                            break;
                        default:
                            sprite = await GetDirectionSprite(actor, direction == Direction.West ? Direction.East : direction, "damaged", new() { 120, 80, 80, 80, 80, 80 }, new() { 0, 1, 2, 1, 2, 0 });
                            if (direction == Direction.West)
                                sprite.ScaleX = -1;
                            break;
                    }

                    result = await GetMovingAnimation(sprite, -xDirection, -yDirection, 520, 0);
                    return result;
                    

                case Animations.Move:
                    result = await GetMovingAnimation(actor, direction == Direction.West ? Direction.East : direction, "move", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 }, xDirection, yDirection, 250, 250);
                    if (result == null)
                        return null;
                    if (direction == Direction.West)
                    {
                        result.Sprite.ScaleX = -1;
                    }
                    return result;                  
            }

            return null;
        }

        public async Task<AnimatedSprite> GetDirectionSprite(Actors actor, Direction direction, string animationName, List<int> times, List<int> frames)
        {
            var name = actor.ToString().ToLower();
            var directionString = direction != Direction.None ? "_" + this.DirectionToString(direction) : "";

            this.JavascriptHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            var spritesheet = await this.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new string[] { "PIXI", "Loader", "shared", "resources", "sprites", "spritesheet" });

            var textureNames = new List<string>();

            foreach (var frame in frames)
                textureNames.Add(name + "_" + animationName + directionString + "_" + frame + ".png");

            var textures = new List<IJSObjectReference>();

            foreach (var textureName in textureNames)
            {
                var texture = await this.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new String[] { "textures", textureName }, spritesheet);
                if (texture == null)
                    return null;
                textures.Add(texture);
            }

            if (textures.Contains(null))
                return null;

            var sprite = new AnimatedSprite(this.JSRuntime, textures, times);
            await sprite.Initialize();
            await sprite.SetVisibility(false);
            await sprite.SetLoop(false);
            await sprite.SetAnchor(0.5f);

            foreach (var texture in textures)
                await texture.DisposeAsync();

            return sprite;
        }

        public async Task<Animation> GetDirectionAnimation(Actors actor, Direction direction, string animationName, List<int> times, List<int> frames)
        {
            var sprite = await GetDirectionSprite(actor, direction, animationName, times, frames);
            if (sprite == null)
                return null;
            var animation = new Animation(sprite);
            animation.QueueFrame = 1;
            return animation;
        }


        public async Task<MovingAnimation> GetMovingAnimation(AnimatedSprite sprite, int xDistance, int yDistance, int duration, int queueDuration = -1)
        {
            if (sprite == null)
                return null;
            await sprite.SetLoop(true);
            var movingAnimation = new MovingAnimation(this.Application, sprite, xDistance, yDistance, duration, queueDuration);
            return movingAnimation;
        }
        public async Task<MovingAnimation> GetMovingAnimation(Actors actor, Direction direction, string animationName, List<int> times, List<int> frames, int xDistance, int yDistance, int duration, int queueDuration = - 1)
        {
            var sprite = await GetDirectionSprite(actor, direction, animationName, times, frames);
            if (sprite == null)
                return null;
            return await GetMovingAnimation(sprite, xDistance, yDistance, duration, queueDuration);
        }

        private string DirectionToString(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return "up";
                case Direction.East:
                    return "right";
                case Direction.South:
                    return "down";
                case Direction.West:
                    return "left";
                default:
                    return "up";
            }
        }

        


    }
}
