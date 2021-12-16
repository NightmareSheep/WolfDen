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
        IUnitUI UnitUI { get; }
        IGatherChestsWinConditionUI GatherChestsWinConditionUI { get; }
        Task SetPlayers(List<Player> players, List<Player> activePlayers, Player currentPlayer);
        Task DoneLoading();
        Task ShowMessage(string text);

        event Func<Task> EndTurnButtonClickedEvent;
        event Func<MouseEventArgs, Task> MouseClickEvent;
        event Func<Task> MouseRightClickEvent;
    }
}
