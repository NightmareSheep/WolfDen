using Lupus.Behaviours.Defend;
using Lupus.Behaviours.Displacement;
using Lupus.Tiles;
using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.WinConditions.GatherChests
{
    public class Chest : Unit
    {
        public Pushable Pushable { get; }

        public Chest(Game game, string id, Tile tile, Player neutralPlayer) : base(game, neutralPlayer, id, tile)
        {           
            Pushable = new Pushable(game.Map, this);
            Health = new Invulnurable(this);
            this.Destroyables = new List<Other.IDestroy>() { Pushable };
        }
    }
}
