using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class Undo
    {
        private readonly Game game;

        public Undo(Game game, Player owner)
        {
            this.game = game;
            Owner = owner;
            Id = "player " + owner.Id + " undo";
        }

        public Player Owner { get; }
        public string Id { get; }

        public void Execute()
        {
            if (game.History.Moves.Count == 0)
                return;

            var moves = game.History.Moves;
            moves.RemoveAt(moves.Count - 1);
            game.GameInitializer.Initialize();
        }
    }
}
