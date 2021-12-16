using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class History
    {
        private bool stopMoveUpdates;
        public List<IHistoryMove> Moves { get; set; } = new List<IHistoryMove>();
        public Game Game { get; }

        public History(Game game)
        {
            Game = game;
        }

        public void AddMove(IHistoryMove move)
        {
            if (stopMoveUpdates)
                return;

            Moves.Add(move);
        }

        public async Task PlayHistory()
        {
            stopMoveUpdates = true;
            foreach (var move in Moves)
            {
                await move.Execute(Game);
            }
            stopMoveUpdates = false;
        }
    }
}
