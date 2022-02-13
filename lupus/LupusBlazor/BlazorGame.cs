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

namespace LupusBlazor
{
    public class BlazorGame : Game
    {
        public event Func<object ,Task> ClickEvent;

        public Player CurrentPlayer { get; }
        public HubConnection Hub { get; }
        public IJSRuntime JSRuntime { get; }
        public BlazorMap BlazorMap { get; }
        public IUI UI { get; }
        public AudioPlayer AudioPlayer { get; }

        public AnimationPlayer AnimationPlayer;
        public ActionQueue ActionQueue { get; set; }
        public BlazorTurnResolver BlazorTurnResolver { get; }
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

        public BlazorGame(List<Player> players, Player CurrentPlayer, HubConnection hub, IJSRuntime jSRuntime, IUI ui, AudioPlayer audioPlayer) : base(players)
        {            
            this.CurrentPlayer = CurrentPlayer;
            Hub = hub;
            JSRuntime = jSRuntime;
            Map = BlazorMap = new BlazorMap(this);
            AnimationPlayer = new AnimationPlayer(jSRuntime, this);            
            AudioPlayer = audioPlayer;            
            this.ActionQueue = new ActionQueue();
            UI = ui;
            TurnResolver = BlazorTurnResolver = new BlazorTurnResolver(this, players, CurrentPlayer);
            UI.BlazorGame = this;
        }

        public async Task Draw()
        {
            this.LupusPixiApplication = new LupusPixiApplication(this.JSRuntime, this.Map.Width * 16, this.Map.Height * 16);
            await this.LupusPixiApplication.Initialize();

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
