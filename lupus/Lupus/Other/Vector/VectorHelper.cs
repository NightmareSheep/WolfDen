using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Lupus.Other.Vector
{
    public static class VectorHelper
    {
        public static Vector2 GetDirectionVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Vector2(0, -1);
                case Direction.East:
                    return new Vector2(1, 0);
                case Direction.South:
                    return new Vector2(0, 1);
                case Direction.West:
                    return new Vector2(-1, 0);
            }
            throw new Exception("Unkown direction");
        }
    }
}
