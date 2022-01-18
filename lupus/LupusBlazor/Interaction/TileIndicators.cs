using Lupus;
using LupusBlazor.Behaviours.Movement;
using LupusBlazor.Units;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Interaction
{
    public class TileIndicators
    {
        private BlazorGame Game { get; }
        public Map Map { get; }
        private IJSRuntime JSRuntime { get; }
        public KnownColor Tint { get; }
        private List<TileIndicator> Indicators { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public event Func<int, Task> TileClickEvent;


        public TileIndicators(BlazorGame game, Map map, IJSRuntime jSRuntime, KnownColor tint)
        {
            Game = game;
            Map = map;
            JSRuntime = jSRuntime;
            Tint = tint;
            game.TurnResolver.StartTurnEvent += StartTurn;
            game.UI.MouseRightClickEvent += this.RemoveIndicators;
        }

        public async Task StartTurn(List<Player> activePlayers)
        {
            await this.RemoveIndicators();
        }

        public async Task Spawn(IEnumerable<int> indices)
        {
            await RemoveIndicators();

            foreach (var index in indices)
            {
                var tile = Map.GetTile(index);
                var indicator = new TileIndicator(this.Game, this, tile, this.Tint, JSRuntime);
                Indicators.Add(indicator);
                await indicator.Draw();
            }

        }

        public async Task ClickTile(int index)
        {
            await Game.RaiseClickEvent(this);
            await RaiseTileClickEvent(index);
        }

        public async Task RemoveIndicators()
        {
            foreach (var indicator in Indicators ?? Enumerable.Empty<TileIndicator>())
                await indicator.Destroy();
            Indicators = new List<TileIndicator>();
        }

        public async Task Destroy()
        {
            Game.UI.MouseRightClickEvent -= this.RemoveIndicators;
            Game.TurnResolver.StartTurnEvent -= StartTurn;
            await RemoveIndicators();
        }

        private async Task RaiseTileClickEvent(int index)
        {
            if (TileClickEvent != null)
                await TileClickEvent?.Invoke(index);
        }
    }
}
