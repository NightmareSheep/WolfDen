using Lupus;
using Lupus.Tiles;
using Lupus.WinConditions.GatherChests;
using LupusBlazor.Extensions;
using LupusBlazor.Pixi;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.WinConditions.GatherChests
{
    public class BlazorZone : Zone, IDrawable
    {
        Sprite sprite;

        public BlazorZone(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime iJSRuntime, int zoneId) : base(game, owner, id, tile)
        {
            BlazorGame = game;
            IJSRuntime = iJSRuntime;
            ZoneId = zoneId;
        }

        public BlazorGame BlazorGame { get; }
        public IJSRuntime IJSRuntime { get; }
        public int ZoneId { get; }

        public void Draw()
        {
            var jsHelper = JavascriptHelperModule.Instance;
            var texture =  jsHelper.GetJavascriptProperty<IJSInProcessObjectReference?>(new string[] { "PIXI", "Loader", "shared", "resources", "sprites", "spritesheet", "textures", "zone " + ZoneId + ".png" });
            sprite = new Sprite(this.IJSRuntime, texture);
            sprite.X = this.Tile.XCoord();
            sprite.Y = this.Tile.YCoord();
            sprite.Tint = this.Owner.Color;
            this.BlazorGame.LupusPixiApplication.ViewPort.AddChild(sprite);
            texture.DisposeAsync();
        }
    }
}
