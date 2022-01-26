using Lupus.Other;
using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using Lupus.Behaviours.Displacement;
using Lupus.Units;
using System.Numerics;
using Lupus.Other.Vector;
using System.Threading.Tasks;
using Lupus.Actions;

namespace Lupus.Behaviours.Attack
{
    public class DamageAndPush : IDestroy, IAction
    {
        public int Strength { get; }
        public SkillPoints SkillPoints { get; }

        protected readonly Game game;
        protected readonly Unit unit;
        protected string Id { get; set; }
        public string Name { get; } = "DamageAndPush";

        public ActionAgent Agent { get; }

        public DamageAndPush(Game game, Unit unit, int strength, SkillPoints skillPoints)
        {
            this.game = game;
            this.unit = unit;
            Strength = strength;
            SkillPoints = skillPoints;
            Id = unit.Id + " DamageAndPush";
            game.GameObjects.Add(Id, this);
            unit.Owner.GameObjects.Add(Id, this);
            Agent = new ActionAgent(game, this);
        }        

        public virtual async Task DamageAndPushUnit(Direction direction)
        {
            if (SkillPoints.CurrentPoints < 1)
                return;

            await SkillPoints.UseSkillPoints(1, this);
            var target = GetTarget(direction);

            var targetUnit = target?.Unit;
            await target?.Pushing?.Push(direction);
            if (targetUnit != null && targetUnit.Health.CurrentHealth >= 0)
                await targetUnit.Health?.Damage(Strength);

            game.History.AddMove(new DamageAndPushHistory(Id, direction));
            await Agent.ActionUsed();
        }

        protected Tile GetTarget(Direction direction)
        {
            var directionVector = VectorHelper.GetDirectionVector(direction);
            var positionVector = new Vector2(unit.Tile.X, unit.Tile.Y);
            var targetVector = positionVector + directionVector;
            return Helper.ReturnObjectAtIndexOrDefault(game.Map.Tiles, (int)targetVector.X, (int)targetVector.Y);
        }

        public virtual Task Destroy()
        {
            game.RemoveObject(Id);
            return Task.CompletedTask;
        }

        public int GetAvailableActions()
        {
            if (this.SkillPoints.CurrentPoints >= 1)
                return 1;
            return 0;
        }
    }
}
