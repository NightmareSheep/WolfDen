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
        public BlazorZone(Game game, Player owner, string id, Tile tile, IJSRuntime iJSRuntime, int zoneId) : base(game, owner, id, tile)
        {
            IJSRuntime = iJSRuntime;
            ZoneId = zoneId;
        }

        public IJSRuntime IJSRuntime { get; }
        public int ZoneId { get; }

        public async Task Draw()
        {
            await PixiHelper.CreateSprite(IJSRuntime, new string[] { "other", "zones", "zone" + ZoneId }, Id, Tile.XCoord(), Tile.YCoord(), null, true, ColorTranslator.ToHtml(Color.FromArgb(Color.FromKnownColor(Owner.Color).ToArgb())));
        }
    }
}
