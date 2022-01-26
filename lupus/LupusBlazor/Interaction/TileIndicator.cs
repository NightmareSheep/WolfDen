using Lupus.Tiles;
using LupusBlazor.Extensions;
using LupusBlazor.Interaction;
using LupusBlazor.Pixi;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Behaviours.Movement
{
    public class TileIndicator
    {
        private Clickable Clickable { get; set; }
        private DotNetObjectReference<Clickable> ObjRef { get; }
        public BlazorGame Game { get; }
        private TileIndicators TileIndicators { get; }
        private Tile Tile { get; }
        public KnownColor Tint { get; }
        private IJSRuntime JSRuntime { get; }
        private Guid Id { get; set; } = Guid.NewGuid();
        private Sprite sprite;

        public TileIndicator(BlazorGame game, TileIndicators tileIndicators, Tile tile, KnownColor tint, IJSRuntime jSRuntime)
        {
            Clickable = new Clickable();
            Game = game;
            TileIndicators = tileIndicators;
            Tile = tile;
            Tint = tint;
            JSRuntime = jSRuntime;
            ObjRef = DotNetObjectReference.Create(Clickable);
            Clickable.ClickEvent += Click;
        }

        public async Task Click()
        {
            await TileIndicators.ClickTile(Tile.Index);
        }

        public async Task Draw()
        {

            var jsHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            var texture = await jsHelper.GetJavascriptProperty<IJSObjectReference>(new string[] { "PIXI", "Texture", "WHITE" });
            sprite = new Sprite(JSRuntime, texture);
            await sprite.Initialize();
            sprite.Interactive = true;
            await sprite.OnClick(ObjRef, "RaisClickEvent");
            sprite.Width = 16;
            sprite.Height = 16;
            sprite.X = Tile.XCoord();
            sprite.Y = Tile.YCoord();
            sprite.Alpha = 0.5f;
            sprite.Tint = this.Tint;
            await this.Game.LupusPixiApplication.ViewPort.AddChild(sprite);
            await texture.DisposeAsync();
        }

        public async Task Destroy()
        {
            await this.Game.LupusPixiApplication.ViewPort.RemoveChild(sprite);
            await (sprite?.Dispose() ?? Task.CompletedTask);
        }
    }
}
