using Lupus.Behaviours;
using Lupus.Behaviours.Attack;
using Lupus.Behaviours.Defend;
using Lupus.Behaviours.Displacement;
using Lupus.Behaviours.Movement;
using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Units
{
    public class Goblin : Unit
    {
        public DamageAndPush DamageAndPush { get; }
        public Pushable Pushable { get; }
        public Move Move { get; }
        public SkillPoints SkillPoints { get; set; }

        public Goblin(Game game, Player owner, string id, Tile tile) : base(game, owner, id, tile)
        {
            Health = new Health(this, 5);

            SkillPoints = new SkillPoints(game, this, 1);
            Move = new Move(game, game.Map, this, 5, SkillPoints);
            DamageAndPush = new DamageAndPush(game, this, 1, SkillPoints);
            Pushable = new Pushable(game.Map, this);

            this.Destroyables = new List<Other.IDestroy>() { Move, DamageAndPush, Pushable, SkillPoints };
        }



        public override async Task Destroy()
        {
            await base.Destroy();
            await Move.Destroy();
        }
    }
}
