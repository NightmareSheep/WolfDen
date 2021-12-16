using Dijkstra;
using Lupus.Other.Vector;
using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Text;
using Lupus.Behaviours.Movement;
using Lupus.Behaviours.Displacement;
using System.Numerics;
using Lupus.Other;

namespace Lupus.Tiles
{
    public class Tile : IVector, INode<Tile>
    {     
        public Map Map { get; }
        public int X { get; }
        public int Y { get; }
        private Unit unit;
        public TileType Type { get; set; }
        public MovementCost MovementCost { get; }
        public Pushing Pushing { get; }

        public Tile(Map map, int x, int y, TileType type = TileType.Normal)
        {
            Map = map;
            X = x;
            Y = y;
            Type = type;

            MovementCost = new MovementCost(this);
            Pushing = new Pushing(this);
        }

        public Unit Unit
        {
            get { return unit; }
            set
            {
                if (value != unit)
                {
                    unit = value;

                    if (unit != null)
                        unit.Tile = this;
                }
            }
        }

        public List<Tile> Neighbours { get 
            {
                var neighbours = new List<Tile>();
                var mapWidth = Map.Tiles.GetLength(0);
                var mapHeight = Map.Tiles.GetLength(1);

                if (X > 0)
                    neighbours.Add(Map.Tiles[X - 1, Y]);
                if (Y > 0)
                    neighbours.Add(Map.Tiles[X, Y - 1]);
                if (X < mapWidth - 1)
                    neighbours.Add(Map.Tiles[X + 1, Y]);
                if (Y < mapHeight - 1)
                    neighbours.Add(Map.Tiles[X, Y + 1]);

                return neighbours;
            }
        }

        public int Index => Y * Map.Width + X;
        public static void IndexToCoords(out int x, out int y, int index, Map map)
        {
            x = index % map.Width;
            y = index / map.Width;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public Tile GetNeigbour(Direction direction)
        {
            var positionVector = this.ToVector2();
            var directionVector = VectorHelper.GetDirectionVector(direction);
            var neigbourVector = positionVector + directionVector;
            return Helper.ReturnObjectAtIndexOrDefault(Map.Tiles, (int)neigbourVector.X, (int)neigbourVector.Y);
        }
    }
}
