using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.WinConditions.GatherChests
{
    public class GatherChestsWinCondition
    {
        protected Game Game { get; }
        protected List<List<Player>> Teams { get; }
        protected List<Zone> Zones { get; set; }
        protected int Turns { get; }
        protected List<Unit> Chests { get; }
        protected int CurrentTurn { get; set; } = -1;
        protected int[] Scores { get; set; }

        public GatherChestsWinCondition(Game game, List<Player> players, List<Zone> zones, int turns, List<Unit> Chests)
        {
            Game = game;
            this.Teams = players.GroupBy(p => p.Team).Select(group => group.ToList()).ToList();
            this.Zones = zones;
            Turns = turns;
            this.Chests = Chests;
            Scores = new int[Teams.Count];

            game.TurnResolver.EndTurnEvent += EndTurn;
            game.TurnResolver.StartTurnEvent += StartTurn;
        }

        protected virtual void StartTurn(object sender, List<Player> activePlayers)
        {
            CurrentTurn++;
            if (CurrentTurn >= Turns * Teams.Count)
                AnnounceVictor();
        }

        protected virtual void EndTurn(object sender, List<Player> activePlayers)
        {

            foreach (var chest in Chests)
            {
                foreach (var zone in Zones)
                {
                    if (chest.Tile == zone.Tile && activePlayers.Contains(zone.Owner))
                    {
                        var owner = zone.Owner;                       
                        var team = Teams.FirstOrDefault(t => t.Contains(owner));
                        var teamIndex = Teams.IndexOf(team);
                        Scores[teamIndex]++;
                    }
                }
            }

            
        }

        protected virtual void AnnounceVictor()
        {
            Game.EndGame();
        }
    }
}
