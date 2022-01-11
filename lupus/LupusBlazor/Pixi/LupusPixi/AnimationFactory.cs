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
        public AudioPlayer Audioplayer { get; }
        public Application Application { get; }

        public AnimationFactory(Application application, IJSRuntime jSRuntime, AudioPlayer audioplayer)
        {
            this.Application = application;
            JSRuntime = jSRuntime;
            Audioplayer = audioplayer;
        }

        public async Task<Animation> GetAnimation(Actors actor, Animations animation)
        {
            Animation result;
            var audioPlayer = this.Audioplayer;

            switch (animation)
            {
                case Animations.Idle:
                    return await GetIdleAnimation(actor);
                case Animations.AttackUp:
                    result = await GetDirectionAnimation(actor, Direction.North, "attack", new()      { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 });
                    result.Sprite.OnFrameChangeEvent += async (int frame) => { if (frame == 1) { await audioPlayer.PlaySound(Audio.Effects.SwordStrikesArmor); } };
                    result.QueueFrame = 2;
                    return result;
                case Animations.AttackRight:
                    result = await GetDirectionAnimation(actor, Direction.East, "attack", new()       { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 });
                    result.Sprite.OnFrameChangeEvent += async (int frame) => { if (frame == 1) { await audioPlayer.PlaySound(Audio.Effects.SwordStrikesArmor); } };
                    result.QueueFrame = 2;
                    return result;
                case Animations.AttackDown:
                    result = await GetDirectionAnimation(actor, Direction.South, "attack", new()      { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 });
                    result.Sprite.OnFrameChangeEvent += async (int frame) => { if (frame == 1) { await audioPlayer.PlaySound(Audio.Effects.SwordStrikesArmor); } };
                    result.QueueFrame = 2;
                    return result;
                case Animations.AttackLeft:
                    result = await GetDirectionAnimation(actor, Direction.East, "attack", new()       { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 });
                    result.Sprite.OnFrameChangeEvent += async (int frame) => { if (frame == 1) { await audioPlayer.PlaySound(Audio.Effects.SwordStrikesArmor); } };
                    result.Sprite.ScaleX = -1f;
                    result.QueueFrame = 2;
                    return result;
                case Animations.ShortDamagedFromUp:
                    return await GetDirectionAnimation(actor, Direction.North, "damaged", new()     { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                case Animations.ShortDamagedFromRight:
                    return await GetDirectionAnimation(actor, Direction.East, "damaged", new()      { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                case Animations.ShortDamagedFromDown:
                    return await GetDirectionAnimation(actor, Direction.South, "damaged", new()     { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                case Animations.ShortDamagedFromLeft:
                    result = await GetDirectionAnimation(actor, Direction.East, "damaged", new()      { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 });
                    result.Sprite.ScaleX = -1f;
                    return result;
            }

            return null;
        }

        public async Task<MovingAnimation> GetMovingAnimation(Actors actor, Animations animation)
        {
            MovingAnimation result;
            var audioPlayer = this.Audioplayer;

            switch (animation)
            {
                
                case Animations.DamagedFromUp:
                    return await GetMovingAnimation(actor, Direction.North, "damaged", new() { 120, 80, 80, 80, 80, 80 }, new() { 0, 1, 2, 1, 2, 0 }, 0, 16, 520, 0);
                case Animations.DamagedFromRight:
                    return await GetMovingAnimation(actor, Direction.East, "damaged", new() { 120, 80, 80, 80, 80, 80 }, new() { 0, 1, 2, 1, 2, 0 }, -16, 0, 520, 0);
                case Animations.DamagedFromDown:
                    return await GetMovingAnimation(actor, Direction.South, "damaged", new() { 120, 80, 80, 80, 80, 80 }, new() { 0, 1, 2, 1, 2, 0 }, 0, -16, 520, 0);
                case Animations.DamagedFromLeft:
                    result = await GetMovingAnimation(actor, Direction.East, "damaged", new() { 120, 80, 80, 80, 80, 80 }, new() { 0, 1, 2, 1, 2, 0 }, 16, 0, 520, 0);
                    result.Sprite.ScaleX = -1f;
                    return result;
                case Animations.MoveUp:
                    return await GetMovingAnimation(actor, Direction.North,     "move", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 }, 0, -16, 520, 520);
                case Animations.MoveRight:
                    return await GetMovingAnimation(actor, Direction.East,      "move", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 }, 16, 0, 520, 520);
                case Animations.MoveDown:
                    return await GetMovingAnimation(actor, Direction.South,     "move", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 }, 0, 16, 520, 520);
                case Animations.MoveLeft:
                    result = await GetMovingAnimation(actor, Direction.East,      "move", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 }, -16, 0, 520, 520);
                    result.Sprite.ScaleX = -1;
                    return result;

            }

            return null;
        }

        private async Task<Animation> GetIdleAnimation(Actors actor)
        {
            var name = actor.ToString().ToLower();
            var spritesheet = this.Application?.SpriteSheets["sprites"];

            var textureNames = new List<string>() {
                name + "_idle_0.png",
                name + "_idle_1.png",
                name + "_idle_2.png",
                name + "_idle_1.png",
            };

            var times = new List<int>()
            {
                640,
                80,
                640,
                80
            };

            var textures = new List<IJSObjectReference>();

            foreach (var textureName in textureNames)
                textures.Add(await this.Application?.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new String[] { "textures", textureName }, spritesheet));

            var sprite = new AnimatedSprite(this.Application, this.JSRuntime, textures, times);
            await sprite.Initialize();
            await sprite.SetVisibility(false);
            await sprite.SetAnchor(0.5f);
            foreach (var texture in textures)
                await texture.DisposeAsync();

            return new Animation(sprite);
        }

        public async Task<Animation> GetDirectionAnimation(Actors actor, Direction direction, string animationName, List<int> times, List<int> frames)
        {
            var name = actor.ToString().ToLower();
            var directionString = this.DirectionToString(direction);


            var spritesheet = this.Application?.SpriteSheets["sprites"];
            var textureNames = new List<string>();

            foreach (var frame in frames)
                textureNames.Add(name + "_" + animationName + "_" + directionString + "_" + frame + ".png");

            var textures = new List<IJSObjectReference>();

            foreach (var textureName in textureNames)
                textures.Add(await this.Application?.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new String[] { "textures", textureName }, spritesheet));

            var sprite = new AnimatedSprite(this.Application, this.JSRuntime, textures, times);
            await sprite.Initialize();
            await sprite.SetVisibility(false);
            await sprite.SetLoop(false);
            await sprite.SetAnchor(0.5f);

            foreach (var texture in textures)
                await texture.DisposeAsync();

            return new Animation(sprite);
        }

        public async Task<MovingAnimation> GetMovingAnimation(Actors actor, Direction direction, string animationName, List<int> times, List<int> frames, int xDistance, int yDistance, int duration, int queueDuration = - 1)
        {
            var dummyAnimation = await GetDirectionAnimation(actor, direction, animationName, times, frames);
            var sprite = dummyAnimation.Sprite;
            dummyAnimation.Dispose();
            await sprite.SetLoop(true);
            var movingAnimation = new MovingAnimation(sprite, this.Application, xDistance, yDistance, duration, queueDuration);
            return movingAnimation;
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
