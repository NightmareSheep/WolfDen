using System;
using System.Collections.Generic;
using System.Text;
using Lupus.Tiles;
using System.Numerics;
using Tiled;

namespace Lupus.Factories
{
    public class TileFactory
    {
        public Tile AddTile(Map map, int x, int y)
        {
            var tile = new Tile(map, x, y) { Type = TileType.Normal };
            map.Tiles[x, y] = tile;
            return tile;
        }

        public Tile AddTile(Map map, int x, int y, uint id)
        {
            if (id == 0)
                return null;

            var tilesetId = id - 1;
            uint tilesetX = tilesetId % 20;
            uint tilesetY = tilesetId / 20;

            var position = new Vector2(tilesetX, tilesetY);


            var type = DungeonTileset.Tiles[tilesetId];


            switch (type)
            {
                case "Normal":
                    map.Tiles[x, y] = new Tile(map, x, y) { Type = TileType.Normal };
                    break;
                case "Wall":
                    map.Tiles[x, y] = new Tile(map, x, y) { Type = TileType.Wall };
                    break;
            }

            return map.Tiles[x, y];
        }
    }
}
