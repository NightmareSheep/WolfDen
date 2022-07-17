using Lupus.Other.MapLoading;
using LupusBlazor.Pixi.LupusPixi.TileSet;
using Microsoft.JSInterop;
using PIXI;
using PIXI.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class TileMapLoader
    {
        public IJSRuntime JSRuntime { get; }

        public TileMapLoader(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }        

        public void LoadTileMap(JsonMap map, Container container)
        {
            foreach (var layer in map.layers)
                LoadLayer(layer, container, map);
        }

        private void LoadLayer(Layer layer, Container container, JsonMap map)
        {
            var compositeTileMap = new CompositeTilemap();
            container.AddChild(compositeTileMap);

            foreach (var innerLayer in layer.layers ?? Enumerable.Empty<Layer>())
                LoadLayer(innerLayer, container, map);

            for (var i = 0; i <layer.Data.Count(); i++)
            {
                var gid = layer.Data[i];
                if (gid == 0)
                    continue;

                var tileSet = GetTileSet(map, gid, out var firstGid);

                var id = gid - firstGid;
                var texture = tileSet.GetTexture(id);

                if (texture == null)
                    continue;

                var posX = i % map.Width * map.TileWidth;
                var posY = i / map.Height * map.TileHeight;

                compositeTileMap.Tile(texture, posX, posY);
            }
        }

        private TileSet.TileSet GetTileSet(JsonMap map, uint id, out uint firstGid)
        {
            var tileSet = TileSets.Sets["dungeon"];
            firstGid = 1;
            return tileSet;
        }
    }
}
