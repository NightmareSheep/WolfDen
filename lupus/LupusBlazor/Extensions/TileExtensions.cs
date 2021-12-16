using Lupus.Tiles;
using LupusBlazor.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace LupusBlazor.Extensions
{
    public static class TileExtensions
    {
        public static int XCoord(this Tile t) => t.X * Statics.tileWidth;
        public static int YCoord(this Tile t) => t.Y * Statics.tileHeight;
    }
}
