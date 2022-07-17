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

        public Texture GetTexture(uint baseId)
        {
            var id = RemoveAndReturnTransformationBits(baseId, out var horizontalFlip, out var verticalFlip);
            var rotation = GetRotation(horizontalFlip, verticalFlip);

            if (rotation != 0)
            {
                var text = "";
            }

            if (id >= Collums * Rows)
                return null;

            

            if (Textures.ContainsKey(id))
                return Textures[id];            

            var x = (int)id % Collums;
            var y = (int)id / Collums;

            var texture = new Texture(BaseTexture, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight), null, null, rotation);
            Textures[id] = texture;

            return texture;
        }

        /// <summary>
        /// Takes the ID and sets the first four significant bits to 0 and extracting whether the texture should be flipped horizontaly or verticaly. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="horizontalFlip"></param>
        /// <param name="verticalFlip"></param>
        /// <returns>The new ID</returns>
        public uint RemoveAndReturnTransformationBits(uint id, out bool horizontalFlip, out bool verticalFlip)
        {
            string binary = Convert.ToString(id, 2).PadLeft(32, '0');
            horizontalFlip = binary[0] == '1';
            verticalFlip = binary[1] == '1';

            var characterString = binary.ToCharArray();
            characterString[0] = '0';
            characterString[1] = '0';
            characterString[2] = '0';
            characterString[3] = '0';
            binary = new string(characterString);

            var newId = Convert.ToUInt32(binary, 2);
            return newId;
        }

        public int GetRotation(bool horizontalFlip, bool verticalFlip)
        {
            // PIXI specific. Check http://gameofbombs.github.io/pixi-bin/index.html?s=legacy&f=texture-rotate.js&title=Texture%20Rotate

            var rotate = 0;
            if (horizontalFlip) rotate = 12;
            if (verticalFlip) rotate = 8;
            if (horizontalFlip && verticalFlip) rotate = 4;

            return rotate;
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
