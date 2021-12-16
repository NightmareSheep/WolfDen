using Lupus.Tiles;
using LupusBlazor.Extensions;
using LupusBlazor.Interaction;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Behaviours.Movement
{
    public class TileIndicator
    {
        private Clickable Clickable { get; set; }
        private DotNetObjectReference<Clickable> ObjRef { get; }
        private TileIndicators TileIndicators { get; }
        private Tile Tile { get; }
        private string[] IndicatorAsset { get; }
        private IJSRuntime JSRuntime { get; }
        private Guid Id { get; set; } = Guid.NewGuid();

        public TileIndicator(TileIndicators tileIndicators, Tile tile, string[] indicatorAsset, IJSRuntime jSRuntime)
        {
            Clickable = new Clickable();
            TileIndicators = tileIndicators;
            Tile = tile;
            IndicatorAsset = indicatorAsset;
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
            await PixiHelper.CreateSprite(JSRuntime, IndicatorAsset, Id.ToString(), Tile.XCoord(), Tile.YCoord(), ObjRef);
        }

        public async Task Destroy()
        {
            await PixiHelper.DestroySprite(JSRuntime, Id.ToString());
        }
    }
}
