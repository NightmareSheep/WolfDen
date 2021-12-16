using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Lupus.Other.Vector
{
    public static class VectorExtensions
    {
        public static int GetDistanceTo(this IVector vector1, IVector vector2)
        {
            return Math.Abs(vector1.X - vector2.X) + Math.Abs(vector1.Y - vector2.Y);
        }

        public static Direction GetDirectionFromVector(this Vector2 v)
        {
            switch (v)
            {
                case Vector2 w when w.X == 0 && w.Y < 0:
                    return Direction.North;
                case Vector2 w when w.X > 0 && w.Y == 0:
                    return Direction.East;
                case Vector2 w when w.X == 0 && w.Y > 0:
                    return Direction.South;
                case Vector2 w when w.X < 0 && w.Y == 0:
                    return Direction.West;
            }

            return Direction.North;
        }
    }
}
