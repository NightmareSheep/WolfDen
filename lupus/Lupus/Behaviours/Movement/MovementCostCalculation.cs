using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Tiles;

namespace Lupus.Behaviours.Movement
{
    public struct MovementCostCalculation
    {
        public int currentCost;
        public Tile from;
        public Tile to;
        public MovementType movementType;
    }
}
