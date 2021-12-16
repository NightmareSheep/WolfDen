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

        private async Task EndTurnButtonClicked()
        {
            await BlazorGame.Hub.InvokeAsync("EndTurn", BlazorGame.Id, CurrentPlayer.Id);
        }

        public override async Task StartTurn()
        {
            await base.StartTurn();
            await this.BlazorGame.UI.SetPlayers(this.BlazorGame.Players, this.ActivePlayers, this.CurrentPlayer);

            if (this.ActivePlayers.Count == 1)
                await this.BlazorGame.UI.ShowMessage(ActivePlayers[0].Name + "'s turn!");
            if (this.ActivePlayers.Count > 1)
                await this.BlazorGame.UI.ShowMessage("Team " + ActivePlayers[0].Team + "'s turn!");

            await this.BlazorGame.AudioPlayer.PlaySound(Audio.Effects.TurnEnd);
        }
    }
}
