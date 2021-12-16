using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class EndTurnHistory : IHistoryMove
    {

        public EndTurnHistory(string playerId)
        {
            PlayerId = playerId;
        }

        public string PlayerId { get; }

        public async Task Execute(Game game)
        {
            await game.TurnResolver.EndTurn(game.Players.FirstOrDefault(p => p.Id == PlayerId));
        }
    }
}
