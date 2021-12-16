using Lupus;
using Lupus.Behaviours.Attack;
using Lupus.Units;
using LupusBlazor.Interaction;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Lupus.Other.Vector;
using Lupus.Other;
using LupusBlazor.Units;
using Microsoft.AspNetCore.SignalR.Client;
using LupusBlazor.Animation;
using LupusBlazor.Extensions;
using Lupus.Behaviours;

namespace LupusBlazor.Behaviours.Attack
{
    public class BlazorDamageAndPull : DamageAndPull, ISkill
    {
        private TileIndicators AttackIndicators;
        private BlazorGame BlazorGame { get; set; }
        public Unit Unit { get; }
        public IJSRuntime IJSRuntime { get; }

        public BlazorDamageAndPull(BlazorGame game, Unit unit, int strength, SkillPoints skillPoints, IJSRuntime iJSRuntime) : base(game, unit, strength, skillPoints)
        {
            BlazorGame = game;
            Unit = unit;
            IJSRuntime = iJSRuntime;
            AttackIndicators = new TileIndicators(game, game.Map, iJSRuntime, new string[] { "indicators", "red" });
            AttackIndicators.TileClickEvent += ClickIndicator;
            game.ClickEvent += Click;

        }

        public async Task Click(object sender)
        {
            if (sender == this.AttackIndicators)
                return;

            await AttackIndicators.RemoveIndicators();
        }

        public async Task SpawnAttackIndicators()
        {
            if (SkillPoints.CurrentPoints < 1)
                return;

            await BlazorGame.RaiseClickEvent(this);
            var indices = new List<int>();
            var directions = new Direction[] { Direction.North, Direction.East, Direction.South, Direction.West };
            foreach (var direction in directions)
            {
                var tile1 = this.unit.Tile.GetTileInDirection(this.game, direction);
                var tile2 = tile1?.GetTileInDirection(this.game, direction);

                if (tile1 != null)
                    indices.Add(tile1.Index);
                if (tile2 != null)
                    indices.Add(tile2.Index);
            }

            await AttackIndicators.Spawn(indices);
        }

        public async Task ClickIndicator(int index)
        {
            await AttackIndicators.RemoveIndicators();

            var clickedTile = game.Map.GetTile(index);
            var clickedTileVector = new Vector2(clickedTile.X, clickedTile.Y);
            var unitVector = new Vector2(this.unit.Tile.X, this.unit.Tile.Y);
            var directionVector = clickedTileVector - unitVector;
            var direction = directionVector.GetDirectionFromVector();


            await this.BlazorGame.Hub.InvokeAsync("DoMove", BlazorGame.Id, this.Unit.Owner.Id, Id, typeof(DamageAndPull).AssemblyQualifiedName, "DamageAndPullUnit", new object[] { direction }, new string[] { typeof(Direction).AssemblyQualifiedName });
        }

        public override async Task DamageAndPullUnit(Direction direction)
        {
            var target = this.unit.Tile.GetTileInDirection(BlazorGame, direction);
            var target2 = target?.GetTileInDirection(BlazorGame, direction);
            var soundEffect = Audio.Effects.TakeASipOfWater;

            //if (target.Unit != null && target2?.Unit == null)
            //    await BlazorGame.AnimationPlayer.PlayUnitAnimation(target.Unit as BlazorUnit, "ShortDamagedFrom" + direction.OppositeDirection().ToString(), 360);

            await PixiHelper.SetSpriteVisible(IJSRuntime, Unit.Id + " Idle", false);
            var tile = Unit.Tile;
            var directionVector = VectorHelper.GetDirectionVector(direction) * (LupusBlazor.Other.Statics.tileWidth / 2);
            var x = tile.XCoord() + (int)directionVector.X;
            var y = tile.YCoord() + (int)directionVector.Y;

            var animation = new MoveAnimation(BlazorGame, IJSRuntime, Unit.Id + " Pull" + direction.ToString(), 400, 100,
                    x, y, x, y, true, soundEffect,
                    async () => { await PixiHelper.SetSpriteVisible(IJSRuntime, Unit.Id + " Idle", true); }
                    );
            await BlazorGame.AnimationPlayer.QueueAnimation(animation);
            await base.DamageAndPullUnit(direction);
        }

        public override async Task Destroy()
        {
            await base.Destroy();
            AttackIndicators.TileClickEvent -= ClickIndicator;
            this.BlazorGame.ClickEvent -= Click;
            await AttackIndicators.Destroy();
        }

        public async Task ClickSkill()
        {
            if (unit.Owner == BlazorGame.CurrentPlayer)
                await SpawnAttackIndicators();
        }
    }
}
