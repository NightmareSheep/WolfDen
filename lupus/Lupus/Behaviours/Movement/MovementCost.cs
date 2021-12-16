using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Tiles;

namespace Lupus.Behaviours.Movement
{
    public class MovementCost
    {
        public Tile Tile { get; }
        public event EventHandler<MovementCostCalculation> CalculatingMovementCost;

        public MovementCost(Tile tile)
        {
            Tile = tile;
        }        

        public int GetMovementCost(MovementType movementType, Tile from)
        {
            var movementCostCalculation = new MovementCostCalculation();
            movementCostCalculation.from = from;
            movementCostCalculation.to = Tile;
            movementCostCalculation.movementType = movementType;
            movementCostCalculation.currentCost = Tile.Type == TileType.Normal && Tile.Unit == null ? 1 : 9999;
            CalculatingMovementCost?.Invoke(this, movementCostCalculation);

            return movementCostCalculation.currentCost;
        }
    }
}
