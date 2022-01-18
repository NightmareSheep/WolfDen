using Lupus;
using Lupus.Behaviours.Displacement;
using Lupus.Other;
using Lupus.Tiles;
using Lupus.Units;
using LupusBlazor.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LupusBlazor.Animation;
using Microsoft.JSInterop;
using LupusBlazor.Extensions;
using Lupus.Other.Vector;
using LupusBlazor.Other;

namespace LupusBlazor.Behaviours.Displacement
{
    public class BlazorPushable : Pushable
    {
        public BlazorPushable(IJSRuntime iJSRuntime, BlazorGame game, Map map, BlazorUnit unit) : base(map, unit)
        {
            IJSRuntime = iJSRuntime;
            Game = game;
            BlazorUnit = unit;
        }

        public IJSRuntime IJSRuntime { get; }
        public BlazorGame Game { get; }
        public BlazorUnit BlazorUnit { get; }

        protected override async Task PushIntoTile(Tile from, Direction direction, Tile destination)
        {
            await (this.BlazorUnit?.PixiUnit?.QueueAnimation(Animations.Damaged, direction.OppositeDirection()) ?? Task.CompletedTask);
            await base.PushIntoTile(from, direction, destination);
        }

        protected override  async Task PushAgainstWall(Tile from, Direction direction)
        {
            await (this.BlazorUnit?.PixiUnit?.QueueAnimation(Animations.ShortDamaged, direction.OppositeDirection()) ?? Task.CompletedTask);
            await base.PushAgainstWall(from, direction);
        }

        protected override async Task PushAgainstUnit(Tile from, Direction direction, Tile destination)
        {
            var bumpedUnit = destination.Unit as BlazorUnit;
            await (BlazorUnit?.PixiUnit?.QueueAnimation(Animations.ShortDamaged, direction.OppositeDirection()) ?? Task.CompletedTask);
            await (bumpedUnit?.PixiUnit?.QueueAnimation(Animations.ShortDamaged, direction.OppositeDirection()) ?? Task.CompletedTask);

             

            await base.PushAgainstUnit(from, direction, destination);
        }
    }
}
