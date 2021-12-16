using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Other;
using Lupus.Tiles;

namespace Lupus.Behaviours.Displacement
{
    public struct Push
    {
        public Direction direction;

        public Push(Direction direction, int distance, Tile tile) : this()
        {
            this.direction = direction;
            this.distance = distance;
            this.tile = tile;
        }

        public int distance;
        public Tile tile;
    }
}
