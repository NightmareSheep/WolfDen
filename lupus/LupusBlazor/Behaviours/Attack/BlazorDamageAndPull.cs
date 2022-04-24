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
using LupusBlazor.Pixi.LupusPixi;

namespace LupusBlazor.Behaviours.Attack
{
    public class BlazorDamageAndPull : DamageAndPull, ISkill
    {
        private TileIndicators AttackIndicators;
        private BlazorGame BlazorGame { get; set; }
        public BlazorUnit Unit { get; }
        public IJSRuntime IJSRuntime { get; }

        public BlazorDamageAndPull(BlazorGame game, BlazorUnit unit, int strength, SkillPoints skillPoints, IJSRuntime iJSRuntime) : base(game, unit, strength, skillPoints)
        {
            BlazorGame = game;
            Unit = unit;
            IJSRuntime = iJSRuntime;
            AttackIndicators = new TileIndicators(game, game.Map, iJSRuntime, System.Drawing.KnownColor.Red);
            AttackIndicators.TileClickEvent += ClickIndicator;
            game.ClickEvent += Click;

        }

        public void Click(object sender, EventArgs e)
        {
            if (sender == this.AttackIndicators)
                return;
            
            AttackIndicators.RemoveIndicators();
        }

        public void SpawnAttackIndicators()
        {
            if (SkillPoints.CurrentPoints < 1)
                return;

             BlazorGame.RaiseClickEvent(this);
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

             AttackIndicators.Spawn(indices);
        }

        public void ClickIndicator(object sender, int index)
        {
             AttackIndicators.RemoveIndicators();

            var clickedTile = game.Map.GetTile(index);
            var clickedTileVector = new Vector2(clickedTile.X, clickedTile.Y);
            var unitVector = new Vector2(this.unit.Tile.X, this.unit.Tile.Y);
            var directionVector = clickedTileVector - unitVector;
            var direction = directionVector.GetDirectionFromVector();


             this.BlazorGame.Hub.InvokeAsync("DoMove", BlazorGame.Id, this.Unit.Owner.Id, Id, typeof(DamageAndPull).AssemblyQualifiedName, "DamageAndPullUnit", new object[] { direction }, new string[] { typeof(Direction).AssemblyQualifiedName });
        }

        public override void DamageAndPullUnit(Direction direction)
        {
            var target = unit?.Tile?.GetNeigbour(direction)?.GetNeigbour(direction)?.Unit as BlazorUnit;
            var targetPixiUnit = target?.PixiUnit;

            if (targetPixiUnit == null)
                Unit?.PixiUnit?.QueueAnimation(Animations.Pull, direction);
            else
                Unit?.PixiUnit?.QueueInteraction(Animations.Pull, direction, new List<PixiUnit>() { targetPixiUnit });

             base.DamageAndPullUnit(direction);
        }

        public override void Destroy()
        {
             base.Destroy();
            AttackIndicators.TileClickEvent -= ClickIndicator;
            this.BlazorGame.ClickEvent -= Click;
             AttackIndicators.Destroy();
        }

        public void ClickSkill()
        {
            if (unit.Owner == BlazorGame.CurrentPlayer)
                 SpawnAttackIndicators();
        }
    }
}
