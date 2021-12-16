using Lupus;
using Lupus.Tiles;
using Lupus.Units;
using Lupus.Units.Orcs;
using LupusBlazor.Behaviours.Defend;
using LupusBlazor.Behaviours.Movement;
using LupusBlazor.Extensions;
using LupusBlazor.Pixi;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LupusBlazor.Units
{
    public class BlazorUnit : Unit, IDrawable
    {
        public BlazorGame Game { get; }
        private IJSRuntime JSRuntime { get; }
        public Clickable Clickable { get; }
        private DotNetObjectReference<Clickable> ObjRef { get; }
        public Dictionary<string, string[]> Assets { get; set; }
        protected List<ISkill> Skills { get; set; } = new List<ISkill>();
        public BlazorHealth BlazorHealth { get; set; }
        protected string Name { get; set; }

        public event Func<Task> CurrentPlayerClickActive;
        public event Func<Task> CurrentPlayerCLickInactive;
        public event Func<Task> OtherPlayerClick;
        

        public BlazorUnit(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime, Dictionary<string, string[]> assets = null) : base(game, owner, id, tile)
        {
            Game = game;
            JSRuntime = jSRuntime;
            Clickable = new Clickable();
            ObjRef = DotNetObjectReference.Create(Clickable);
            Assets = assets;
            Clickable.ClickEvent += ClickUnit;
            game.ClickEvent += Click;
            Game.TurnResolver.StartTurnEvent += this.StartTurn;
            Game.UI.MouseRightClickEvent += RightClick;
        }

        public async Task RightClick()
        {
            await PixiHelper.SetFilter(JSRuntime, Id + " Idle", Pixi.PixiFilter.GlowFilter, false);
        }

        public async Task Click(object sender)
        {        
            if (sender != this && sender is BlazorUnit)
                await PixiHelper.SetFilter(JSRuntime, Id + " Idle", Pixi.PixiFilter.GlowFilter, false);
        }

        private async Task StartTurn(List<Player> players)
        {
            await PixiHelper.SetFilter(JSRuntime, Id + " Idle", Pixi.PixiFilter.GlowFilter, false);
        }

        public async Task ClickUnit()
        {
            await Game.AudioPlayer.PlaySound(Audio.Effects.CoolInterfaceClickTone);
            await Game.RaiseClickEvent(this);
            await PixiHelper.SetFilter(JSRuntime, Id + " Idle", Pixi.PixiFilter.GlowFilter, true);
            await Game.UI.UnitUI.ResetCharacterUI();
            await Game.UI.UnitUI.SetCharacterUI(this.Name);
            foreach (var skill in Skills)
            {
                await Game.UI.UnitUI.SetCharacterSkill(skill.Name, skill.ClickSkill);
            }

            if (CurrentPlayerClickActive != null && Owner == Game.CurrentPlayer && Game.TurnResolver.ActivePlayers.Contains(Game.CurrentPlayer))
            {
                var invocationList = CurrentPlayerClickActive.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }

            if (CurrentPlayerCLickInactive != null && Owner == Game.CurrentPlayer && !Game.TurnResolver.ActivePlayers.Contains(Game.CurrentPlayer))
            {
                var invocationList = CurrentPlayerCLickInactive.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }

            if (OtherPlayerClick != null && Owner != Game.CurrentPlayer)
            {
                var invocationList = OtherPlayerClick.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public async Task Draw()
        {
            await PixiHelper.CreateSprite(JSRuntime, Assets["Idle"], Id + " Idle", Tile.XCoord(), Tile.YCoord(), ObjRef);
            var teamFilter = Enum.Parse<PixiFilter>(Owner.Color.ToString() + "Team");
            await PixiHelper.SetFilter(JSRuntime, Id + " Idle", teamFilter, true);
            foreach (var pair in Assets)
            {
                if (pair.Key == "Idle")
                    continue;

                await PixiHelper.CreateSprite(JSRuntime, Assets[pair.Key], Id + " " + pair.Key, 0, 0, null, false);
                                
                await PixiHelper.SetFilter(JSRuntime, Id + " " + pair.Key, teamFilter, true);
                await BlazorHealth.Draw();
            }
        }

        public async override Task Destroy()
        {
            await base.Destroy();
            Game.ClickEvent -= Click;
            Game.TurnResolver.StartTurnEvent -= this.StartTurn;
            Game.UI.MouseRightClickEvent -= RightClick;
        }

        public async Task UpdatePosition()
        {
            await PixiHelper.SetSpritePosition(JSRuntime, Id + " Idle", Tile.XCoord(), Tile.YCoord());
            await BlazorHealth.Draw();
        }
    }
}
