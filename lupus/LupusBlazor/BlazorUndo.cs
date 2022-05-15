using Lupus;
using LupusBlazor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using LupusBlazor.Audio;

namespace LupusBlazor
{
    public class BlazorUndo : Undo, IDisposable
    {
        public BlazorGame Game { get; }
        public BlazorUI UI { get; }
        public AudioPlayer AudioPlayer { get; }

        public BlazorUndo(BlazorGame game, BlazorUI UI, AudioPlayer audioPlayer, BlazorPlayer owner) : base(game, owner)
        {
            Game = game;
            this.UI = UI;
            AudioPlayer = audioPlayer;
            if (game.CurrentPlayer == Owner)
                UI.UI.UndoButton.PressButtonEvent += CallUndo;
        }

        public void CallUndo(object sender, EventArgs e)
        {
             Game.Hub.InvokeAsync("DoMove", Game.Id, Owner.Id, Id, typeof(Undo).AssemblyQualifiedName, "Execute", new object[] { }, new string[] {});
        }

        public override void Execute()
        {
            if (!CanUndo())
            {
                UI?.UI?.TextMessage.ShowMessage("Cannot undo");
                AudioPlayer.PlaySound(Effects.Fail);
                return;
            }

            UI?.UI?.UndoMessage?.ShowMessage();
            AudioPlayer?.PlaySound(Effects.Undo);
            base.Execute();
        }

        public void Dispose()
        {
            UI.UI.UndoButton.PressButtonEvent -= CallUndo;
        }
    }
}
