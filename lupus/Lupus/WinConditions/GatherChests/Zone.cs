using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.WinConditions.GatherChests
{
    public class Zone
    {
        public string Id { get; set; }
        private Game Game { get; set; }
        public Tile Tile { get; set; }
        public Player Owner { get; set; }

        public Zone(Game game, Player owner, string id, Tile tile)
        {
            this.Tile = tile;
            this.Owner = owner;
            this.Game = game;
            this.Id = id;
        }
    }
}
