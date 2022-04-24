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
        public Sprite Sprite { get; set; }

        public TileIndicator(BlazorGame game, TileIndicators tileIndicators, Tile tile, Sprite sprite, KnownColor tint, IJSRuntime jSRuntime)
        {
            Clickable = new Clickable();
            Game = game;
            TileIndicators = tileIndicators;
            Tile = tile;
            Tint = tint;
            JSRuntime = jSRuntime;
            ObjRef = DotNetObjectReference.Create(Clickable);
            Clickable.ClickEvent += Click;
            this.Sprite = sprite;
        }

        public void Click(object sender, EventArgs e)
        {
             TileIndicators.ClickTile(Tile.Index);
        }

        public void Draw()
        {
            Sprite.X = Tile.XCoord();
            Sprite.Y = Tile.YCoord();
            Sprite.Tint = this.Tint;
            Sprite.ClickEvent += Click;
             this.Game.LupusPixiApplication.ViewPort.AddChild(Sprite);
        }

        public void Destroy()
        {
            this.Sprite.ClickEvent -= Click;
             this.Game.LupusPixiApplication.ViewPort.RemoveChild(Sprite);
        }
    }
}
