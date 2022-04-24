﻿using Lupus.Behaviours;
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
    public class Slime : Unit
    {
        public DamageAndPull DamageAndPull { get; }
        public Pushable Pushable { get; }
        public Move Move { get; }
        public SkillPoints SkillPoints { get; set; }

        public Slime(Game game, Player owner, string id, Tile tile) : base(game, owner, id, tile)
        {
            Health = new Health(this, 5);

            SkillPoints = new SkillPoints(game, this, 1);
            Move = new Move(game, game.Map, this, 3, SkillPoints);
            DamageAndPull = new DamageAndPull(game, this, 1, SkillPoints);
            Pushable = new Pushable(game.Map, this);

            this.Destroyables = new List<Other.IDestroy>() { Move, DamageAndPull, Pushable, SkillPoints };
        }



        public override void Destroy()
        {
            base.Destroy();
            Move.Destroy();
        }
    }
}
