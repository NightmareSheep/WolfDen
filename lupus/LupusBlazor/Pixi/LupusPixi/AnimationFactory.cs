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
        public static async Task<AnimationFactory> GetInstance(IJSRuntime jSRuntime, Application application, AudioPlayer audioplayer)
        {
            return instance ??= await new AnimationFactory(jSRuntime, application, audioplayer).Initialize();
        }
        private static AnimationFactory instance;

        public IJSRuntime JSRuntime { get; set; }
        public Application Application { get; }
        public AudioPlayer Audioplayer { get; }
        private JavascriptHelperModule JavascriptHelper { get; set; }
        private Random random = new Random();
        private Dictionary<AnimationConfiguration, Stack<Animation>> RecycledAnimations = new();

        private AnimationFactory(IJSRuntime jSRuntime, Application application, AudioPlayer audioplayer)
        {
            JSRuntime = jSRuntime;
            Application = application;
            Audioplayer = audioplayer;
        }

        private async Task<AnimationFactory> Initialize()
        {
            this.JavascriptHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            return this;
        }

        public async Task<Animation> GetAnimation(Actors actor, Animations animation, Direction direction)
        {
            var animationConfiguration = new AnimationConfiguration(actor, animation, direction);
            if (RecycledAnimations.TryGetValue(animationConfiguration, out var stack) && stack.Count > 0)
                return stack.Pop();

            Animation result;
            var audioPlayer = this.Audioplayer;
            List<string> sounds = new();

            switch (animation)
            {
                case Animations.Idle:
                    switch (actor)
                    {
                        case Actors.Chest:
                            result = await GetDirectionAnimation(animationConfiguration, "idle", new() { 100 }, new() { 0 });
                            break;
                        default:
                            result = await GetDirectionAnimation(animationConfiguration, "idle", new() { 640, 80, 640, 80 }, new() { 0, 1, 2, 1 });
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

                    result = await GetDirectionAnimation(animationConfiguration, "attack", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 });
                    if (result == null) { return null; }

                    Effects soundEffect = Effects.none;
                    //if (animation == Animations.Attack)
                    //    soundEffect = Effects.SwordStrikesArmor;
                    if (animation == Animations.MissedAttack)
                        soundEffect = Effects.DaggerWoosh;
                    //if (animation == Animations.Pull)
                    //    soundEffect = Effects.TakeASipOfWater;

                    sounds = GetSounds(GetWeaponString(actor), "Hit");                    

                    result.Sprite.OnFrameChangeEvent += async (int frame) => { 
                        if (frame == 1) {
                            if (animation == Animations.MissedAttack)
                                await audioPlayer.PlaySound(Effects.DaggerWoosh);
                            else
                            {
                                var random = this.random.Next(sounds.Count);
                                if (sounds.Count > 0)
                                    await audioPlayer.PlaySoundEffect(sounds[random]);
                            }
                        } 
                    };
                    result.QueueFrame = 2;
                    if (direction == Direction.West)
                        result.Sprite.ScaleX = -1f;
                    return result;
                case Animations.ShortDamaged:
                    sounds = GetSounds(actor, "Hit");
                    result = await GetDirectionAnimation(animationConfiguration, "damaged", new() { 120, 80, 80, 80 }, new() { 0, 1, 2, 0 }, direction == Direction.West ? Direction.East : direction);
                    if (result == null)
                        return null;
                    if (direction == Direction.West)
                        result.Sprite.ScaleX = -1;
                    result.Sprite.OnFrameChangeEvent += async (int frame) => {
                        if (frame == 0)
                        {
                            var random = this.random.Next(sounds.Count);
                            if (sounds.Count > 0)
                                await audioPlayer.PlaySoundEffect(sounds[random]);
                        }
                    };

                    return result;
                case Animations.Open:
                    result = await GetDirectionAnimation(animationConfiguration, "opening", new() { 4, 80, 80, 640 }, new() { 0, 1, 2, 3 });
                    return result;
                case Animations.Cheer:
                    result = await GetDirectionAnimation(animationConfiguration, "cheer", new() { 80, 80, 220, 80 }, new() { 0, 1, 2, 1 });
                    if (result == null) { return null; }
                    sounds = GetSounds(actor, "What");
                    result.Sprite.OnFrameChangeEvent += async (int frame) => { 
                        if (frame == 0) {
                            var random = this.random.Next(sounds.Count);
                            if (sounds.Count > 0)
                                await audioPlayer.PlaySoundEffect(sounds[random]);
                        }
                    };
                    hitArea = await JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Rectangle" }, new() { -8, -8, 16, 16 });
                    await JavascriptHelper.SetJavascriptProperty(new string[] { "hitArea" }, hitArea, result.Sprite.JSInstance);
                    await hitArea.DisposeAsync();
                    return result;
                case Animations.Death:
                    result = await GetDirectionAnimation(animationConfiguration, "death", new() { 100, 100, 220, 2000 }, new() { 0, 1, 2, 3 });
                    if (result == null) { return null; }
                    sounds = GetSounds(actor, "Death");
                    result.Sprite.OnFrameChangeEvent += async (int frame) => {
                        if (frame == 0)
                        {
                            var random = this.random.Next(sounds.Count);
                            if (sounds.Count > 0)
                                await audioPlayer.PlaySoundEffect(sounds[random]);
                        }
                    };
                    return result;

            }

            return null;
        }

        public async Task<MovingAnimation> GetMovingAnimation(Actors actor, Animations animation, Direction direction)
        {
            var animationConfiguration = new AnimationConfiguration(actor, animation, direction);
            if (RecycledAnimations.TryGetValue(animationConfiguration, out var stack) && stack.Count > 0)
                return stack.Pop() as MovingAnimation;

            MovingAnimation result;
            var audioPlayer = this.Audioplayer;

            var xDirection = 0;
            var yDirection = 0;
            List<string> sounds = new();


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
                            sprite = await GetDirectionSprite(animationConfiguration, "idle", new() { 100 }, new() { 0 });

                            break;
                        default:
                            sprite = await GetDirectionSprite(animationConfiguration, "damaged", new() { 120, 80, 80, 80, 80, 80 }, new() { 0, 1, 2, 1, 2, 0 }, direction == Direction.West ? Direction.East : direction);
                            if (direction == Direction.West)
                                sprite.ScaleX = -1;
                            break;
                    }

                    result = await GetMovingAnimation(animationConfiguration, sprite, -xDirection, -yDirection, 520, 0);
                    if (result == null)
                        return null;

                    await result.Sprite.SetLoop(false);
                    var baseId = actor.ToString() + "_Hit";
                    var i = 1;
                    if (audioPlayer.SoundEffects.Contains(baseId))
                        sounds.Add(baseId);
                    while (true)
                    {                      
                        var id = baseId + i;
                        if (audioPlayer.SoundEffects.Contains(id))
                            sounds.Add(id);
                        else
                            break;
                        i++;
                    }

                    result.Sprite.OnFrameChangeEvent += async (int frame) => { if (frame == 0) {
                            var random = this.random.Next(sounds.Count);
                            if (sounds.Count > 0)
                                await audioPlayer.PlaySoundEffect(sounds[random]); 
                        } };
                    return result;
                    

                case Animations.Move:
                    sprite = await GetDirectionSprite(animationConfiguration, "move", new() { 100, 100, 100, 100 }, new() { 0, 1, 2, 3 }, direction == Direction.West ? Direction.East : direction);
                    result = await GetMovingAnimation(animationConfiguration, sprite, xDirection, yDirection, 250, 250);
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

        public async Task<AnimatedSprite> GetDirectionSprite(AnimationConfiguration animationConfiguration, string animationName, List<int> times, List<int> frames, Direction specificDirection = Direction.None)
        {
            var spriteDirection = specificDirection != Direction.None ? specificDirection : animationConfiguration.Direction;
            var name = animationConfiguration.Actor.ToString().ToLower();
            var directionString = spriteDirection != Direction.None ? "_" + this.DirectionToString(spriteDirection) : "";

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

        public async Task<Animation> GetDirectionAnimation(AnimationConfiguration animationConfiguration, string animationName, List<int> times, List<int> frames, Direction direction = Direction.None)
        {
            var sprite = await GetDirectionSprite(animationConfiguration, animationName, times, frames);
            if (sprite == null)
                return null;
            var animation = new Animation(sprite, animationConfiguration, this);
            return animation;
        }


        public async Task<MovingAnimation> GetMovingAnimation(AnimationConfiguration animationConfiguration, AnimatedSprite sprite, int xDistance, int yDistance, int duration, int queueDuration = -1)
        {
            if (sprite == null)
                return null;
            await sprite.SetLoop(true);
            var movingAnimation = new MovingAnimation(this.Application, sprite, animationConfiguration, this, xDistance, yDistance, duration, queueDuration);
            return movingAnimation;
        }
        public async Task<MovingAnimation> GetMovingAnimation(AnimationConfiguration animationConfiguration, string animationName, List<int> times, List<int> frames, int xDistance, int yDistance, int duration, int queueDuration = - 1)
        {
            var sprite = await GetDirectionSprite(animationConfiguration, animationName, times, frames);
            if (sprite == null)
                return null;
            return await GetMovingAnimation(animationConfiguration, sprite, xDistance, yDistance, duration, queueDuration);
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

        private List<string> GetSounds(Actors actor, string soundId)
        {
            return GetSounds(actor.ToString(), soundId);
        }

        private List<string> GetSounds(string actorId, string soundId)
        {
            var sounds = new List<string>();
            var baseId = actorId + "_" + soundId;
            var i = 1;
            if (Audioplayer.SoundEffects.Contains(baseId))
                sounds.Add(baseId);
            while (true)
            {
                var id = baseId + i;
                if (Audioplayer.SoundEffects.Contains(id))
                    sounds.Add(id);
                else
                    break;
                i++;
            }
            return sounds;
        }

        private string GetWeaponString(Actors actor)
        {
            switch (actor)
            {
                case Actors.Hero:
                    return "Sword";
                case Actors.Grunt:
                    return "Axe";
                case Actors.Goblin:
                    return "Dagger";
                case Actors.Slime:
                    return "Slime";
            }
                

            return "";

        }

        public void Recycle(AnimationConfiguration animationConfiguration, Animation animation)
        {
            Stack<Animation> animationStack;

            if (!RecycledAnimations.TryGetValue(animationConfiguration, out animationStack))
                animationStack = RecycledAnimations[animationConfiguration] = new();

            animationStack.Push(animation);
        }

        


    }
}
