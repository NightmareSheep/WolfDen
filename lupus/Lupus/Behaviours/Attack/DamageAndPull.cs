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

namespace Lupus.Behaviours.Attack
{
    public class DamageAndPull : IDestroy
    {
        public int Strength { get; }
        public SkillPoints SkillPoints { get; }

        protected readonly Game game;
        protected readonly Unit unit;
        protected string Id { get; set; }
        public string Name { get; } = "DamageAndPull";

        public DamageAndPull(Game game, Unit unit, int strength, SkillPoints skillPoints)
        {
            this.game = game;
            this.unit = unit;
            Strength = strength;
            SkillPoints = skillPoints;
            Id = unit.Id + " DamageAndPull";
            game.GameObjects.Add(Id, this);
            unit.Owner.GameObjects.Add(Id, this);
        }

        public virtual async Task DamageAndPullUnit(Direction direction)
        {
            if (SkillPoints.CurrentPoints < 1)
                return;

            await SkillPoints.UseSkillPoints(1, this);
            var target = this.unit.Tile.GetTileInDirection(this.game, direction);
            var target2 = target?.GetTileInDirection(this.game, direction);

            if (target2 != null)
                await target2?.Pushing?.Push(direction.OppositeDirection());

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
        }

        public virtual Task Destroy()
        {
            game.RemoveObject(Id);
            return Task.CompletedTask;
        }
    }
}
