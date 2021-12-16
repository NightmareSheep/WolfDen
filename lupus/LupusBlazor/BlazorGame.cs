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
        public BlazorTurnResolver BlazorTurnResolver { get; }

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
            Map = BlazorMap = new BlazorMap();
            AnimationPlayer = new AnimationPlayer(jSRuntime, this);
            UI = ui;
            AudioPlayer = audioPlayer;
            TurnResolver = BlazorTurnResolver = new BlazorTurnResolver(this, players, CurrentPlayer);
        }

        public async Task Draw()
        {
            await BlazorMap.Draw(JSRuntime);
            var drawables = GameObjects.Values.OfType<IDrawable>();
            foreach (var drawable in drawables)
            {
                await drawable.Draw();
            }
        }
    }
}
