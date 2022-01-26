using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class TurnResolver
    {
        private Game Game { get; set; }
        private List<Player> Players { get; set; }
        public List<Player> ActivePlayers { get; set; } = new List<Player>();
        public List<List<Player>> TurnOrder { get; set; } = new List<List<Player>>();
        public event Func<List<Player>, Task> EndTurnEvent;
        public event Func<List<Player>, Task> StartTurnEvent;
        public int ActiveGroupIndex { get; set; }

        public TurnResolver(Game game, List<Player> players)
        {
            Game = game;
            Players = players;
            var playersGroupedByTeam = players.GroupBy(player => player.Team, player => player);
            foreach (var playerGroup in playersGroupedByTeam)
                TurnOrder.Add(playerGroup.ToList());
        }

        public virtual async Task<bool> EndTurn(Player player)
        {
            if (!ActivePlayers.Contains(player))
                return false;

            Game.History.AddMove(new EndTurnHistory(player.Id));
            ActivePlayers.Remove(player);
            if (ActivePlayers.Count == 0)
            {
                await RaiseEndTurnEvent(this.TurnOrder[this.ActiveGroupIndex]);
                ActiveGroupIndex = (ActiveGroupIndex + 1) % TurnOrder.Count;
                await StartTurn();
            }
            return true;
        }

        public virtual async Task StartTurn()
        {
            ActivePlayers = TurnOrder[ActiveGroupIndex].ToList();
            await RaiseStartTurnEvent();
        }

        protected virtual async Task RaiseStartTurnEvent()
        {
            if (StartTurnEvent != null)
                await StartTurnEvent.Invoke(ActivePlayers);
        }

        protected virtual async Task RaiseEndTurnEvent(List<Player> players)
        {
            if (EndTurnEvent != null)
                await EndTurnEvent.Invoke(players);
        }
    }
}
