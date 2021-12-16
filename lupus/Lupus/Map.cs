using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lupus
{
    public class Map
    {
        public int Width { get { return Tiles?.GetLength(0) ?? 0; } }
        public int Height { get { return Tiles?.GetLength(1) ?? 0; } }
        public Tile[,] Tiles { get; set; }
        public string Name { get; set; }
        public Tile GetTile(int index)
        {
            Tile.IndexToCoords(out int x, out int y, index, this);
            return Tiles[x, y];
        }
    }
}
