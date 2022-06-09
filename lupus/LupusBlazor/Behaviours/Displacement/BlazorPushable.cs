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
using LupusBlazor.Pixi.LupusPixi;

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

        protected override void PushIntoTile(Tile from, Direction direction, Tile destination)
        {
             BlazorUnit?.PixiUnit?.QueueAnimation(Animations.Damaged, direction.OppositeDirection());
             base.PushIntoTile(from, direction, destination);
        }

        protected override  void PushAgainstWall(Tile from, Direction direction)
        {
             BlazorUnit?.PixiUnit?.QueueAnimation(Animations.ShortDamaged, direction.OppositeDirection());
             base.PushAgainstWall(from, direction);
        }

        protected override void PushAgainstUnit(Tile from, Direction direction, Tile destination)
        {
            var bumpedUnit = destination.Unit as BlazorUnit;
            BlazorUnit?.PixiUnit?.QueueInteraction(Animations.ShortDamaged, direction.OppositeDirection(), new List<PixiUnit>() { bumpedUnit?.PixiUnit });
            bumpedUnit?.PixiUnit?.QueueAnimation(Animations.ShortDamaged, direction.OppositeDirection());

             

             base.PushAgainstUnit(from, direction, destination);
        }
    }
}
