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
            

            var makeInvisible = new ChangeVisibilityAnimation(IJSRuntime, BlazorUnit.Id + " Idle", false);
            var damagedAnimation = new MoveAnimation(Game, IJSRuntime, BlazorUnit.Id + " DamagedFrom" + direction.OppositeDirection().ToString(), 520, 520,
                from.XCoord(), from.YCoord(), destination.XCoord(), destination.YCoord(), true, Audio.Effects.none, async () => { await BlazorUnit.UpdatePosition(); });
            var makevisible = new ChangeVisibilityAnimation(IJSRuntime, BlazorUnit.Id + " Idle", true);

            await Game.AnimationPlayer.QueueAnimation(makeInvisible);
            await Game.AnimationPlayer.QueueAnimation(damagedAnimation);
            await Game.AnimationPlayer.QueueAnimation(makevisible);

            await base.PushIntoTile(from, direction, destination);
        }

        protected override  async Task PushAgainstWall(Tile from, Direction direction)
        {
            

            var directionVector = VectorHelper.GetDirectionVector(direction);
            directionVector *= LupusBlazor.Other.Statics.tileWidth / 4; 
            var positionVector = from.ToVector2();
            positionVector *= LupusBlazor.Other.Statics.tileWidth;
            var destination = positionVector + directionVector;


            var makeInvisible = new ChangeVisibilityAnimation(IJSRuntime, BlazorUnit.Id + " Idle", false);
            var damagedAnimation = new MoveAnimation(Game, IJSRuntime, BlazorUnit.Id + " DamagedFrom" + direction.OppositeDirection().ToString(), 300, 300,
                from.XCoord(), from.YCoord(), (int)destination.X, (int)destination.Y, true);
            var movingAnimation = new MoveAnimation(Game, IJSRuntime, BlazorUnit.Id + " DamagedFrom" + direction.OppositeDirection().ToString(), 100, 100,
                (int)destination.X, (int)destination.Y, from.XCoord(), from.YCoord(), false);
            var makevisible = new ChangeVisibilityAnimation(IJSRuntime, BlazorUnit.Id + " Idle", true);

            await Game.AnimationPlayer.QueueAnimation(makeInvisible);
            await Game.AnimationPlayer.QueueAnimation(damagedAnimation);
            await Game.AnimationPlayer.QueueAnimation(movingAnimation);
            await Game.AnimationPlayer.QueueAnimation(makevisible);

            await base.PushAgainstWall(from, direction);
        }

        protected override async Task PushAgainstUnit(Tile from, Direction direction, Tile destination)
        {
            

            var directionVector = VectorHelper.GetDirectionVector(direction);
            directionVector *= LupusBlazor.Other.Statics.tileWidth / 4;
            var positionVector = from.ToVector2();
            positionVector *= LupusBlazor.Other.Statics.tileWidth;
            var halfway = positionVector + directionVector;

            var bumpedUnit = destination.Unit;

            var makeInvisible = new ChangeVisibilityAnimation(IJSRuntime, BlazorUnit.Id + " Idle", false);


            var movingAnimation = new MoveAnimation(Game, IJSRuntime, BlazorUnit.Id + " DamagedFrom" + direction.OppositeDirection().ToString(), 100, 100,
                (int)halfway.X, (int)halfway.Y, from.XCoord(), from.YCoord(), false);
            var damagedAnimation = new MoveAnimation(Game, IJSRuntime, BlazorUnit.Id + " DamagedFrom" + direction.OppositeDirection().ToString(), 300, 100,
                from.XCoord(), from.YCoord(), (int)halfway.X, (int)halfway.Y, true, Audio.Effects.none, 
                async () => { await movingAnimation.Play( 
                    async () => { await PixiHelper.SetSpriteVisible(IJSRuntime, BlazorUnit.Id + " Idle", true); }); });

            var makeBumpedInvisible = new ChangeVisibilityAnimation(IJSRuntime, bumpedUnit.Id + " Idle", false);
            var bumpedUnitAnimation = new MoveAnimation(Game, IJSRuntime, bumpedUnit.Id + " ShortDamagedFrom" + direction.OppositeDirection().ToString(), 360, 360,
                destination.XCoord(), destination.YCoord(), destination.XCoord(), destination.YCoord(), true, Audio.Effects.none, async () => { await PixiHelper.SetSpriteVisible(IJSRuntime, bumpedUnit.Id + " Idle", true); });

            

            await Game.AnimationPlayer.QueueAnimation(makeInvisible);
            await Game.AnimationPlayer.QueueAnimation(damagedAnimation);
            await Game.AnimationPlayer.QueueAnimation(makeBumpedInvisible);
            await Game.AnimationPlayer.QueueAnimation(bumpedUnitAnimation);

            await base.PushAgainstUnit(from, direction, destination);
        }
    }
}
