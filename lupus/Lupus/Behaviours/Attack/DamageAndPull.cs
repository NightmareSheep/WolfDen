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
    public class DamageAndPull : IDestroy, IAction
    {
        public int Strength { get; }
        public SkillPoints SkillPoints { get; }

        protected readonly Game game;
        protected readonly Unit unit;
        protected string Id { get; set; }
        public string Name { get; } = "DamageAndPull";

        public ActionAgent Agent { get; }

        public DamageAndPull(Game game, Unit unit, int strength, SkillPoints skillPoints)
        {
            this.game = game;
            this.unit = unit;
            Strength = strength;
            SkillPoints = skillPoints;
            Id = unit.Id + " DamageAndPull";
            unit.Owner.AddGameObject(Id, this);
            Agent = new ActionAgent(game, this);
        }

        public virtual void DamageAndPullUnit(Direction direction)
        {
            if (SkillPoints.CurrentPoints < 1)
                return;

            SkillPoints.UseSkillPoints(1, this);
            var target = this.unit.Tile.GetTileInDirection(this.game, direction);
            var target2 = target?.GetTileInDirection(this.game, direction);

            if (target2 != null)
                target2?.Pushing?.Push(direction.OppositeDirection());

            game.History.AddMove(
                new HistoryMove(
                    this.unit.Owner.Id,
                    this.Id,
                    typeof(DamageAndPull).AssemblyQualifiedName,
                    "DamageAndPullUnit",
                    new object[] { direction },
                    new string[] { typeof(Direction).AssemblyQualifiedName }
                )
                );

            Agent.ActionUsed();
        }

        public virtual void Destroy()
        {
            game.RemoveObject(Id);
        }

        public int GetAvailableActions()
        {
            if (SkillPoints.CurrentPoints >= 1)
                return 1;
            return 0;
        }
    }
}
