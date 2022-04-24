using Lupus;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.UI
{
    public interface IUI
    {
        BlazorGame BlazorGame { get; set; }
        IUnitUI UnitUI { get; }
        IGatherChestsWinConditionUI GatherChestsWinConditionUI { get; }
        void SetPlayers(List<Player> players, List<Player> activePlayers, Player currentPlayer);
        void DoneLoading();
        void ShowMessage(string text);
        void UpdateEndTurnButton(object sender, EventArgs e);
        void StartTurn(object sender, List<Player> players);

        event EventHandler EndTurnButtonClickedEvent;
        event EventHandler<MouseEventArgs> MouseClickEvent;
        event EventHandler MouseRightClickEvent;
        event EventHandler UndoClickEvent;
    }
}
