using PIXI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.TileSet
{
    public class TileSet
    {
        public TileSet(string name, int collums, int rows, Texture baseTexture, int tileWidth, int tileHeight)
        {
            Name = name;
            Collums = collums;
            Rows = rows;
            BaseTexture = baseTexture;
            TileHeight = tileHeight;
            TileWidth = tileWidth;
        }

        public string Name { get; }
        public int Collums { get; }
        public int Rows { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public Texture BaseTexture { get; }
        public HashSet<uint> SpecialTiles = new();
        public HashSet<uint> AnimatedTiles = new();
        public Dictionary<uint, Texture> Textures = new();
        public Dictionary<uint, Tuple<List<Texture>, List<int>>> AnimationData = new();

        public TileType GetTileType(uint id)
        {
            if (SpecialTiles.Contains(id))
                return TileType.special;
            if (AnimatedTiles.Contains(id))
                return TileType.animated;

            return TileType.normal;
        }

        public Texture GetTexture(uint id)
        {
            if (Textures.ContainsKey(id))
                return Textures[id];

            var x = (int)id % Collums;
            var y = (int)id / Collums;

            var texture = new Texture(BaseTexture, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight));
            Textures[id] = texture;

            return texture;
        }

        public void AddAnimatedTile(uint id, List<Texture> textures, List<int> times)
        {
            AnimatedTiles.Add(id);
            AnimationData.Add(id, Tuple.Create(textures, times));
        }

        public AnimatedSprite GetAnimatedTile(uint id)
        {
            if (!AnimatedTiles.Contains(id))
                return null;

            var tuple = AnimationData[id];
            return new AnimatedSprite(tuple.Item1.Select(t => t.JSInstance).ToList(), tuple.Item2);
        }
    }
}
