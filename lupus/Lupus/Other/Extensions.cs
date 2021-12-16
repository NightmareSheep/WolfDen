using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lupus.Other.Vector;
using Lupus.Tiles;

namespace Lupus.Other
{
    public static class Extensions
    {
        public static Direction OppositeDirection(this Direction direction)
        {
            var directionVector = VectorHelper.GetDirectionVector(direction);
            directionVector *= -1;
            return directionVector.GetDirectionFromVector();
        }

        public static Tile GetTileInDirection(this Tile tile, Game game, Direction direction)
        {
            var directionVector = VectorHelper.GetDirectionVector(direction);
            var positionVector = new Vector2(tile.X, tile.Y);
            var targetVector = positionVector + directionVector;
            return Helper.ReturnObjectAtIndexOrDefault(game.Map.Tiles, (int)targetVector.X, (int)targetVector.Y);
        }
        
    }
}
