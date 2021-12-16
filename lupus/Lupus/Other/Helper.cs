using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lupus.Other
{
    public static class Helper
    {
        public static T ReturnObjectAtIndexOrDefault<T>(T[,] Array, int x, int y) where T : class
        {
            if (x >= 0 && y >= 0 && x < Array.GetLength(0) && y < Array.GetLength(1))
                return Array[x, y];

            return null;
        }
    }
}
