using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class AnimationFactory
    {
        public IJSRuntime JSRuntime { get; set; }
        public Application Application { get; }

        public AnimationFactory(Application application, IJSRuntime jSRuntime)
        {
            this.Application = application;
            JSRuntime = jSRuntime;
        }
        public async Task<Animation> GetGoblinIdleAnimation()
        {
            var spritesheet = this.Application?.SpriteSheets["sprites"];

            var textureNames = new List<string>() {
                "goblin_idle_0.png",
                "goblin_idle_1.png",
                "goblin_idle_2.png",
                "goblin_idle_1.png",
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
            foreach (var texture in textures)
                await texture.DisposeAsync();

            return new Animation(sprite);
        }

        public async Task<Animation> GetGoblinAttackUpAnimation()
        {
            var spritesheet = this.Application?.SpriteSheets["sprites"];

            var textureNames = new List<string>() {
                "goblin_attack_up_0.png",
                "goblin_attack_up_1.png",
                "goblin_attack_up_2.png",
                "goblin_attack_up_3.png",
            };

            var times = new List<int>()
            {
                100,
                100,
                100,
                100
            };

            var textures = new List<IJSObjectReference>();

            foreach (var textureName in textureNames)
                textures.Add(await this.Application?.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new String[] { "textures", textureName }, spritesheet));

            var sprite = new AnimatedSprite(this.Application, this.JSRuntime, textures, times);
            await sprite.Initialize();
            await sprite.SetVisibility(false);
            await sprite.SetLoop(false);

            foreach (var texture in textures)
                await texture.DisposeAsync();

            return new Animation(sprite);
        }
    }
}
