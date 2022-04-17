using Lupus.Actions;
using Lupus.Other;
using Lupus.Other.Vector;
using Lupus.Tiles;
using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours.Movement
{
    public class Move : IDestroy, IAction
    {
        private readonly Game Game;
        protected Map Map { get; }
        protected Unit Unit { get; }
        public int Speed { get; }
        public bool CanMove { get; set; } = false;
        public MovementType MovementType { get; }
        public SkillPoints SkillPoints { get; }
        public string Id { get; }

        public ActionAgent Agent { get; }

        public Move(Game game, Map map, Unit unit, int speed, SkillPoints skillPoints, MovementType movementType = MovementType.Normal)
        {
            Game = game;
            Map = map;
            Unit = unit;
            Speed = speed;
            MovementType = movementType;
            SkillPoints = skillPoints;
            Id = unit.Id + " Move";
            unit.Owner.AddGameObject(Id, this);
            Game.TurnResolver.StartTurnEvent += StartTurn;
            SkillPoints.SkillPointsUsedEvent += SkillPointsUsed;
            Agent = new ActionAgent(game, this);
        }

        public Task SkillPointsUsed(int amount, object spender)
        {
            this.CanMove = false;

            return Task.CompletedTask;
        }

        public Task StartTurn(List<Player> activePlayers)
        {
            if (activePlayers.Contains(this.Unit.Owner))
            {
                CanMove = true;
                return Task.CompletedTask;
            }
            else
            {
                CanMove = false;
            }
            return Task.CompletedTask;
        }


        public virtual Task MoveOverPath(int[] path) => MoveOverPath(path.Select(index => Map.GetTile(index)).ToArray());

        public  virtual Task MoveOverPath(List<Tile> path) => MoveOverPath(path.ToArray());

        public virtual async Task MoveOverPath(Tile[] path)
        {
            if (path == null || path.Length < 2 || path[^1].Unit != null || !CanMove)
                return;

            // Check if the path is legal.
            var previousTile = path[0];
            var currentCost = 0;
            for (int i = 1; i < path.Length; i++)
            {
                var tile = path[i];
                currentCost += DistanceFunction(previousTile, tile);
                if (currentCost > Speed)
                    return;
                previousTile = path[i];
            }

            path[^1].Unit = Unit;
            CanMove = false;
            Game.History.AddMove(new MoveHistory(Id, path.Select(t => t.Index).ToArray()));
            await Agent.ActionUsed();
        }

        public int DistanceFunction(Tile m, Tile n)
        {
            var distance = m.GetDistanceTo(n);
            if (distance == 1)
                return n.MovementCost.GetMovementCost(MovementType, m);


            return 9999;
        }

        public virtual Task Destroy()
        {
            Game.RemoveObject(Id);
            Game.TurnResolver.StartTurnEvent -= StartTurn;
            SkillPoints.SkillPointsUsedEvent -= SkillPointsUsed;
            return Task.CompletedTask;
        }

        public int GetAvailableActions()
        {
            if (CanMove)
                return 1;
            return 0;
        }
    }
}
