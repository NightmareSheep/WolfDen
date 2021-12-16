using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Other;
using Lupus.Other.Vector;
using Lupus.Tiles;
using Lupus.Units;
using System.Numerics;

namespace Lupus.Behaviours.Displacement
{
    public class Pushable : IDestroy
    {
        public Map Map { get; }
        public Unit Unit { get; }
        public Tile CurrentlySubscribedTile { get; set; }
        

        public Pushable(Map map, Unit unit)
        {
            Map = map;
            Unit = unit;
            if (unit?.Tile?.Pushing != null)
            {
                unit.Tile.Pushing.PushEvent += Pushed;
                CurrentlySubscribedTile = unit.Tile;
            }

            unit.TileIsSetEvent += SetTile;
        }
        
        public void SetTile(object sender, Tile tile)
        {

            if (CurrentlySubscribedTile?.Pushing != null)
                CurrentlySubscribedTile.Pushing.PushEvent -= Pushed;
            if (tile != null)
                tile.Pushing.PushEvent += Pushed;
            CurrentlySubscribedTile = tile;
        }

        public async Task Pushed(Push push)
        {

            var positionVector = new Vector2(push.tile.X, push.tile.Y);
            var directionVector = VectorHelper.GetDirectionVector(push.direction);
            var destinationVector = positionVector + directionVector;
            var destination = Helper.ReturnObjectAtIndexOrDefault(Map.Tiles, (int)destinationVector.X, (int)destinationVector.Y);


            if (destination == null || destination.Type == TileType.Wall)
                await PushAgainstWall(push.tile, push.direction);
            else if (destination.Unit != null)
                await PushAgainstUnit(push.tile, push.direction, destination);
            else
                await PushIntoTile(push.tile, push.direction, destination);
        }

        protected virtual async Task PushAgainstWall(Tile from, Direction direction)
        {
            await Unit.Health.Damage(1);
        }

        protected virtual async Task PushAgainstUnit(Tile from, Direction direction, Tile destination)
        {
            await Unit.Health.Damage(1);
            await destination.Unit.Health.Damage(1);
        }

        protected virtual async Task PushIntoTile(Tile from, Direction direction, Tile destination)
        {
           Unit.Tile = destination;
        }

        public Task Destroy()
        {
            if (CurrentlySubscribedTile?.Pushing != null)
                CurrentlySubscribedTile.Pushing.PushEvent -= Pushed;
            return Task.CompletedTask;
        }
    }
}
