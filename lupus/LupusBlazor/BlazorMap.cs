using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus;
using LupusBlazor.Pixi;
using Microsoft.JSInterop;

namespace LupusBlazor
{
    public class BlazorMap : Map
    {
        public BlazorMap(BlazorGame game) : base()
        {
            Game = game;
        }

        public BlazorGame Game { get; }

        public async Task Draw(IJSRuntime jSRuntime)
        {
            var mapSprite = await jSRuntime.InvokeAsync<IJSObjectReference>("PIXI.Sprite.from", "/game/maps/" + Name + "/" + Name + ".png");
            var sprite = new Sprite(jSRuntime, null, mapSprite);
            await this.Game.LupusPixiApplication.ViewPort.AddChild(sprite);
            await mapSprite.DisposeAsync();
        }
    }
}
