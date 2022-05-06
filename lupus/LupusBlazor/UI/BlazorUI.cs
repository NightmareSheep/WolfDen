using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.UI
{
    public class BlazorUI : IDisposable
    {
        public BlazorGame Game { get; }
        public BlazorTurnResolver TurnResolver { get; }
        public IUI UI { get; }

        public BlazorUI(BlazorGame game, BlazorTurnResolver turnResolver, IUI UI)
        {
            Game = game;
            TurnResolver = turnResolver;
            this.UI = UI;

            UI.BlazorGame = game;
            turnResolver.StartTurnEvent += UI.StartTurn;
            game.ActionTracker.ActionUsedEvent += UI.UpdateEndTurnButton;
        }

        public void Dispose()
        {
            UI.BlazorGame = null;
            TurnResolver.StartTurnEvent -= UI.StartTurn;
            Game.ActionTracker.ActionUsedEvent -= UI.UpdateEndTurnButton;
        }
    }
}
