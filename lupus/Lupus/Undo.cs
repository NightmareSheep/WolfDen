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

        public Undo(Game game)
        {
            this.game = game;
        }

        public async Task Execute()
        {
            if (game.History.Moves.Count == 0)
                return;

            var moves = game.History.Moves;
            moves.RemoveAt(moves.Count - 1);
            await game.GameInitializer.Initialize();
        }
    }
}
