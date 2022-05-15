using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class TurnResolver : IDisposable
    {
        private Game Game { get; set; }
        private List<Player> Players { get; set; }
        public List<Player> ActivePlayers { get; set; } = new List<Player>();
        public List<List<Player>> TurnOrder { get; set; } = new List<List<Player>>();
        public event EventHandler<List<Player>> EndTurnEvent;
        public event EventHandler<Player> PlayerInTeamIsDoneEvent;
        public event EventHandler<List<Player>> StartTurnEvent;
        public int ActiveGroupIndex { get; set; }

        public TurnResolver(Game game, List<Player> players)
        {
            Game = game;
            Players = players;
            var playersGroupedByTeam = players.GroupBy(player => player.Team, player => player);
            foreach (var playerGroup in playersGroupedByTeam)
                TurnOrder.Add(playerGroup.ToList());
        }

        public virtual bool EndTurn(Player player)
        {
            if (!ActivePlayers.Contains(player))
                return false;

            Game.History.AddMove(new EndTurnHistory(player.Id));
            ActivePlayers.Remove(player);
            if (ActivePlayers.Count == 0)
            {
                RaiseEndTurnEvent(this.TurnOrder[this.ActiveGroupIndex]);
                ActiveGroupIndex = (ActiveGroupIndex + 1) % TurnOrder.Count;
                StartTurn();
            }
            else
                RaisePlayerInTeamIsDoneEvent(player);
            return true;
        }

        public virtual void StartTurn()
        {
            ActivePlayers = TurnOrder[ActiveGroupIndex].ToList();
            RaiseStartTurnEvent();
        }

        protected virtual void RaiseStartTurnEvent() => StartTurnEvent?.Invoke(this, ActivePlayers);

        protected virtual void RaiseEndTurnEvent(List<Player> players) => EndTurnEvent?.Invoke(this, players);
        protected virtual void RaisePlayerInTeamIsDoneEvent(Player player) => PlayerInTeamIsDoneEvent?.Invoke(this, player);

        public virtual void Reset()
        {
            ActiveGroupIndex = 0;
        }

        public virtual void Dispose()
        {
            StartTurnEvent = null;
            EndTurnEvent = null;
            PlayerInTeamIsDoneEvent = null;
        }
    }
}
