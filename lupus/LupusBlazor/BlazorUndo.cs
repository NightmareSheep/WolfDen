using Lupus;
using LupusBlazor.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace LupusBlazor
{
    public class BlazorUndo : Undo, IDisposable
    {
        public BlazorUndo(BlazorGame game, BlazorUI UI, BlazorPlayer owner) : base(game, owner)
        {
            Game = game;
            this.UI = UI;
            Owner = owner;
            UI.UI.UndoClickEvent += CallUndo;
        }

        public async Task CallUndo()
        {
            await Game.Hub.InvokeAsync("DoMove", Game.Id, Owner.Id, Id, typeof(Undo).AssemblyQualifiedName, "Execute", new object[] { }, new string[] {});
        }

        public void Dispose()
        {
            UI.UI.UndoClickEvent -= CallUndo;
        }

        public BlazorGame Game { get; }
        public BlazorUI UI { get; }
        public BlazorPlayer Owner { get; }
    }
}
