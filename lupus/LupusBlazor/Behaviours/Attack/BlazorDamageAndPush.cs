﻿using Lupus;
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
    public class BlazorDamageAndPush : DamageAndPush, ISkill
    {
        private TileIndicators AttackIndicators;
        private BlazorGame BlazorGame { get; set; }
        public Unit Unit { get; }
        public IJSRuntime IJSRuntime { get; }

        public BlazorDamageAndPush(BlazorGame game, Unit unit, int strength, SkillPoints skillPoints,  IJSRuntime iJSRuntime) : base(game, unit, strength, skillPoints)
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
            var indices = this.unit.Tile.Neighbours.Select(i => i.Index).ToArray();
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

            await BlazorGame.Hub.InvokeAsync("DamageAndPush", BlazorGame.Id, BlazorGame.CurrentPlayer.Id, Id, direction);          
        }

        public override async Task DamageAndPushUnit(Direction direction)
        {
            var target = this.GetTarget(direction);
            var soundEffect = target.Unit == null ? Audio.Effects.DaggerWoosh : Audio.Effects.SwordStrikesArmor;

            await PixiHelper.SetSpriteVisible(IJSRuntime, Unit.Id + " Idle", false);
            var tile = Unit.Tile;
            var animation = new MoveAnimation(BlazorGame, IJSRuntime, Unit.Id + " Attack" + direction.ToString(), 400, 100,
                    tile.XCoord(), tile.YCoord(), tile.XCoord(), tile.YCoord(), true, soundEffect,
                    async ()=> { await PixiHelper.SetSpriteVisible(IJSRuntime, Unit.Id + " Idle", true); }
                    );
            await BlazorGame.AnimationPlayer.QueueAnimation(animation);
            await base.DamageAndPushUnit(direction);
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