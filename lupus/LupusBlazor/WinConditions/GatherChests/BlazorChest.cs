using Lupus;
using Lupus.Tiles;
using LupusBlazor.Behaviours.Defend;
using LupusBlazor.Behaviours.Displacement;
using LupusBlazor.Pixi.LupusPixi;
using LupusBlazor.Units;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.WinConditions.GatherChests
{
    public class BlazorChest : BlazorUnit
    {
        public BlazorPushable BlazorPushable { get; }

        public BlazorChest(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime) : base(game, owner, id, tile, jSRuntime, null)
        {
            this.Assets = new Dictionary<string, string[]>()
            {
                { "Idle",  new string[] { "objects", "chest", "idle" } },
                { "DamagedFromNorth",  new string[] { "objects", "chest", "idle" } },
                { "DamagedFromEast",  new string[] { "objects", "chest", "idle" } },
                { "DamagedFromSouth",  new string[] { "objects", "chest", "idle" } },
                { "DamagedFromWest",  new string[] { "objects", "chest", "idle" } },
                { "ShortDamagedFromNorth",  new string[] { "objects", "chest", "idle" } },
                { "ShortDamagedFromEast",  new string[] { "objects", "chest", "idle" } },
                { "ShortDamagedFromSouth",  new string[] { "objects", "chest", "idle" } },
                { "ShortDamagedFromWest",  new string[] { "objects", "chest", "idle" } },
                { "Opening",  new string[] { "objects", "chest", "opening" } },
            };

            BlazorPushable = new BlazorPushable(jSRuntime, game, game.Map, this);
            this.Health = this.BlazorHealth = new BlazorInvulnurable(Game, jSRuntime, this);
            Destroyables = new List<Lupus.Other.IDestroy>() { BlazorPushable };
            this.Actor = Actors.Chest;
        }
    }
}
