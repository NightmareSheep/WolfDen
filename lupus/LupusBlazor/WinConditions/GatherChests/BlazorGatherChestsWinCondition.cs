using Lupus;
using Lupus.Units;
using Lupus.WinConditions.GatherChests;
using LupusBlazor.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LupusBlazor.Extensions;

namespace LupusBlazor.WinConditions.GatherChests
{
    public class BlazorGatherChestsWinCondition : GatherChestsWinCondition
    {
        private BlazorGame BlazorGame { get; set; }

        public BlazorGatherChestsWinCondition(BlazorGame game, List<Player> players, List<Zone> zones, int turns, List<Unit> Chests) : base(game, players, zones, turns, Chests)
        {
            this.BlazorGame = game;
        }

        protected override void StartTurn(object sender, List<Player> activePlayers)
        {
            
            base.StartTurn(sender, activePlayers);
            UpdateUI();

        }

        private void UpdateUI()
        {
            BlazorGame.UI.GatherChestsWinConditionUI.SetScores(this.Teams, this.Scores);
            BlazorGame.UI.GatherChestsWinConditionUI.SetTurn((CurrentTurn / Teams.Count) + 1, Turns);
        }

        protected override void EndTurn(object sender, List<Player> activePlayers)
        {
             base.EndTurn(sender, activePlayers);

            

            foreach (var chest in Chests)
            {
                foreach (var zone in Zones)
                {
                    if (chest.Tile == zone.Tile && activePlayers.Contains(zone.Owner))
                    {
                        var blazorUnit = chest as BlazorUnit;
                        if (blazorUnit.PixiUnit != null)
                             blazorUnit.PixiUnit.QueueAnimation(Animation.Animations.Open);
                    }
                }
            }

        }

        protected override void AnnounceVictor()
        {
            var highestScore = Scores.Max();
            var indices = new List<int>();
            for (var i = 0; i < Scores.Length; i++)
                if (Scores[i] == highestScore)
                    indices.Add(i);

            var winningTeams = indices.Select(i => Teams[i]);
            var names = "";
            foreach (var team in winningTeams)
                foreach (var player in team)
                    names += player.Name + " ";

            BlazorGame.UI.GatherChestsWinConditionUI.AnnounceVictor(names);
             base.AnnounceVictor();
        }
    }
}
