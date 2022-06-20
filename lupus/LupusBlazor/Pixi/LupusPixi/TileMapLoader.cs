using Lupus.Other.MapLoading;
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

        public void LoadLayer(Layer layer, Container container, JsonMap map)
        {
            var compositeTileMap = new CompositeTilemap();
            container.AddChild(compositeTileMap);

            foreach (var innerLayer in layer.layers ?? Enumerable.Empty<Layer>())
                LoadLayer(innerLayer, container, map);

            for (var i = 0; i <layer.Data.Count(); i++)
            {
                var gid = layer.Data[i];
                if (gid == 0 || gid >= 481)
                    continue;
                gid--;

                var x = (int)(gid % 20);
                var y = (int)gid / 20;

                var baseTexture = new Texture(Loader.Shared.Resources["dungeon"].Texture);
                var texture = new Texture(baseTexture, new Rectangle(x * map.TileWidth, y * map.TileHeight, map.TileWidth, map.TileHeight));

                var posX = i % map.Width * map.TileWidth;
                var posY = i / map.Height * map.TileHeight;

                compositeTileMap.Tile(texture, posX, posY);

                baseTexture.Dispose();
                texture.Dispose();
            }
        }
    }
}
