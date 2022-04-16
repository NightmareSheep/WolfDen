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
        public BlazorGame(Guid id, List<PlayerInfo> players, string CurrentPlayerId, HubConnection hub, IJSRuntime jSRuntime, IUI ui, AudioPlayer audioPlayer, JsonMap map) : base(id, players, map)
        {
            _currentPlayerId = CurrentPlayerId;
            Hub = hub;
            JSRuntime = jSRuntime;
            Map = BlazorMap = new BlazorMap(this);
            AudioPlayer = audioPlayer;
            UI = ui;
            GameInitializer = new BlazorGameInitializer(this, map);
        }

        public event Func<object ,Task> ClickEvent;

        private string _currentPlayerId;
        public Player CurrentPlayer { get { return Players.FirstOrDefault(p => p.Id == _currentPlayerId); } }
        public HubConnection Hub { get; }
        public IJSRuntime JSRuntime { get; }
        public BlazorMap BlazorMap { get; }
        public IUI UI { get; }
        public BlazorUI BlazorUI { get; set; }
        public AudioPlayer AudioPlayer { get; }
        public BlazorTurnResolver BlazorTurnResolver { get; set; }
        public LupusPixiApplication LupusPixiApplication { get; set; }

        public event Func<Task> DrawEvent;

        private async Task RaiseDrawEvent()
        {
            if (DrawEvent != null)
            {
                var invocationList = DrawEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public async Task RaiseClickEvent(object sender)
        {
            if (ClickEvent != null)
            {
                var invocationList = ClickEvent.GetInvocationList().Cast<Func<object, Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber(sender);
            }
        }

        

        public async Task Draw()
        {
            if (this.LupusPixiApplication == null)
            {
                this.LupusPixiApplication = new LupusPixiApplication(this.JSRuntime, this.Map.Width * 16, this.Map.Height * 16);
                await this.LupusPixiApplication.Initialize();
            }
            else
            {
                await LupusPixiApplication.ViewPort.RemoveChildren();
            }

            await BlazorMap.Draw(JSRuntime);
            var drawables = GameObjects.Values.OfType<IDrawable>();
            foreach (var drawable in drawables)
            {
                await drawable.Draw();
            }

            await RaiseDrawEvent();
        }
    }
}
