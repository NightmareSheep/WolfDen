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

        public void Draw(IJSRuntime jSRuntime)
        {
            var mapSprite =  ((IJSInProcessRuntime)jSRuntime).Invoke<IJSInProcessObjectReference>("PIXI.Sprite.from", "/game/maps/" + Name + "/" + Name + ".png");
            var sprite = new Sprite(jSRuntime, null, mapSprite);
            this.Game.LupusPixiApplication.ViewPort.AddChild(sprite);
            mapSprite.DisposeAsync();
        }
    }
}
