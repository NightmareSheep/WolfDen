using Lupus;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.JSInterop;
using LupusBlazor.UI;
using LupusBlazor.Animation;
using LupusBlazor.Audio;
using LupusBlazor.Pixi;
using LupusBlazor.Pixi.LupusPixi;
using Lupus.Other.MapLoading;

namespace LupusBlazor
{
    public class BlazorGame : Game
    {
        private string _currentPlayerId;
        private BlazorGameInitializer _initializer;
        public Player CurrentPlayer { get { return Players.FirstOrDefault(p => p.Id == _currentPlayerId); } }
        public HubConnection Hub { get; }
        public IJSRuntime JSRuntime { get; }
        public BlazorMap BlazorMap { get; }
        public IUI UI { get; }
        public BlazorUI BlazorUI { get; set; }
        public AudioPlayer AudioPlayer { get; }
        public BlazorTurnResolver BlazorTurnResolver { get; set; }
        public LupusPixiApplication LupusPixiApplication { get; set; }

        public event EventHandler DrawEvent;
        public event EventHandler ClickEvent;

        private void RaiseDrawEvent() => DrawEvent?.Invoke(this, EventArgs.Empty);
        public void RaiseClickEvent(object sender) => ClickEvent?.Invoke(sender, EventArgs.Empty);

        public BlazorGame(Guid id, List<PlayerInfo> players, string CurrentPlayerId, HubConnection hub, IJSRuntime jSRuntime, IUI ui, AudioPlayer audioPlayer, JsonMap map, List<IHistoryMove> moveHistory = null) : base(id, players, map)
        {
            _currentPlayerId = CurrentPlayerId;
            Hub = hub;
            JSRuntime = jSRuntime;
            Map = BlazorMap = new BlazorMap(this);
            AudioPlayer = audioPlayer;
            UI = ui;
            GameInitializer = new BlazorGameInitializer(this, map);
            History.Moves = moveHistory;
        }

        public void Draw()
        {
            if (this.LupusPixiApplication == null)
            {
                this.LupusPixiApplication = new LupusPixiApplication(this.JSRuntime, this.Map.Width * 16, this.Map.Height * 16);
            }
            else
            {
                 LupusPixiApplication.ViewPort.RemoveChildren();
            }

             BlazorMap.Draw(JSRuntime);
            var drawables = GameObjects.Values.OfType<IDrawable>();
            foreach (var drawable in drawables)
            {
                 drawable.Draw();
            }

             RaiseDrawEvent();
        }
    }
}
