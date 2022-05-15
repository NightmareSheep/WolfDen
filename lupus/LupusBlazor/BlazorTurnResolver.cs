using Lupus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace LupusBlazor
{
    public class BlazorTurnResolver : TurnResolver
    {
        private BlazorGame BlazorGame { get; }
        private Player CurrentPlayer { get; }

        public BlazorTurnResolver(BlazorGame blazorGame, List<Player> players, Player currentPlayer) : base(blazorGame, players)
        {
            BlazorGame = blazorGame;
            CurrentPlayer = currentPlayer;
            blazorGame.UI.EndTurnButtonClickedEvent += this.EndTurnButtonClicked;
        }

        private void EndTurnButtonClicked(object sender, EventArgs e)
        {
             BlazorGame.Hub.InvokeAsync("EndTurn", BlazorGame.Id, CurrentPlayer.Id);
        }

        public override bool EndTurn(Player player)
        {
            var playerIsDone = ActivePlayers.Contains(player) && ActivePlayers.Count > 1;
            var result = base.EndTurn(player);
            if (playerIsDone)
            {
                BlazorGame.UI.TextMessage.ShowMessage(player.Name + " is done.");
                this.BlazorGame.AudioPlayer.PlaySound(Audio.Effects.PlayerDone);
                RaisePlayerInTeamIsDoneEvent(player);
            }
            return result;
        }

        public override void StartTurn()
        {
             base.StartTurn();
             this.BlazorGame.UI.SetPlayers(this.BlazorGame.Players, this.ActivePlayers, this.CurrentPlayer);

            if (this.ActivePlayers.Count == 1)
                 this.BlazorGame.UI.TextMessage.ShowMessage(ActivePlayers[0].Name + "'s turn!");
            if (this.ActivePlayers.Count > 1)
                 this.BlazorGame.UI.TextMessage.ShowMessage("Team " + ActivePlayers[0].Team + "'s turn!");

             this.BlazorGame.AudioPlayer.PlaySound(Audio.Effects.TurnEnd);
        }

        public override void Dispose()
        {
            BlazorGame.UI.EndTurnButtonClickedEvent -= this.EndTurnButtonClicked;
        }
    }
}
