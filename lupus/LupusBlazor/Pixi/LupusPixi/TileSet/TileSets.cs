using PIXI.Loading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.TileSet
{
    public static class TileSets
    {
        public static Dictionary<string,TileSet> Sets { get; set; } = new();

        public static void Initialize()
        {
            var loader = Loader.Shared;
            var resource = loader.Resources["dungeon"];
            var texture = resource.Texture;
            var tileset = new TileSet("dungeon", 20, 24, texture, 16, 16);
            Sets.Add("dungeon",tileset);
        }
    }
}
