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
using LupusBlazor.Animation;
using LupusBlazor.Pixi.LupusPixi;
using PIXI;

namespace LupusBlazor.Units
{
    public class BlazorUnit : Unit, IDrawable
    {
        public BlazorGame Game { get; }
        private IJSRuntime JSRuntime { get; }
        public DotNetObjectReference<Clickable> ObjRef { get; }
        public Dictionary<string, string[]> Assets { get; set; }
        protected List<ISkill> Skills { get; set; } = new List<ISkill>();
        public BlazorHealth BlazorHealth { get; set; }
        protected string Name { get; set; }
        public Actors Actor { get; set; }
        public PixiUnit PixiUnit { get; set; }

        public event EventHandler CurrentPlayerClickActive;
        public event EventHandler CurrentPlayerCLickInactive;
        public event EventHandler OtherPlayerClick;
        

        public BlazorUnit(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime, Dictionary<string, string[]> assets = null) : base(game, owner, id, tile)
        {
            Game = game;
            JSRuntime = jSRuntime;
            Assets = assets;
            game.ClickEvent += Click;
            Game.TurnResolver.StartTurnEvent += this.StartTurn;
            Game.UI.MouseRightClickEvent += RightClick;
        }

        public void RightClick(object sender, EventArgs e)
        {
             this.PixiUnit.AnimationContainer.RemoveFilter(PixiFilters.Filters[PixiFilter.GlowFilter]);
        }

        public void Click(object sender, EventArgs e)
        {        
            if (sender != this && sender is BlazorUnit)
                 this.PixiUnit?.AnimationContainer?.RemoveFilter(PixiFilters.Filters[PixiFilter.GlowFilter]);
        }

        private void StartTurn(object sender, List<Player> players)
        {
            if (PixiUnit != null)
                 this.PixiUnit?.AnimationContainer?.RemoveFilter(PixiFilters.Filters[PixiFilter.GlowFilter]);
        }

        public void ClickUnit(object sender, EventArgs e)
        {
             Game.AudioPlayer.PlaySound(Audio.Effects.CoolInterfaceClickTone);
             PixiUnit.QueueAnimation(Animations.Cheer);
             Game.RaiseClickEvent(this);
             this.PixiUnit.AnimationContainer.AddFilter(PixiFilters.Filters[PixiFilter.GlowFilter]);
             Game.UI.UnitUI.ResetCharacterUI();
             Game.UI.UnitUI.SetCharacterUI(this.Name);
            foreach (var skill in Skills)
            {
                 Game.UI.UnitUI.SetCharacterSkill(skill.Name, skill.ClickSkill);
            }

            
            if (Owner == Game.CurrentPlayer && Game.TurnResolver.ActivePlayers.Contains(Game.CurrentPlayer))
            {
                CurrentPlayerClickActive?.Invoke(sender, e);
            }

            if (Owner == Game.CurrentPlayer && !Game.TurnResolver.ActivePlayers.Contains(Game.CurrentPlayer))
            {
                CurrentPlayerCLickInactive?.Invoke(sender, e);
            }

            if (Owner != Game.CurrentPlayer)
            {
                OtherPlayerClick.Invoke(sender, e);
            }
        }

        public void Draw()
        {
            this.PixiUnit = new PixiUnit(this.JSRuntime, this.Game.LupusPixiApplication.Application, this.Game.AudioPlayer, this.Actor);
            PixiUnit.ClickEvent += ClickUnit;
             this.PixiUnit.Initialize();
            this.PixiUnit.Container.X = this.Tile.X * 16 + 8;
            this.PixiUnit.Container.Y = this.Tile.Y * 16 + 8;
            PixiUnit.PlayBaseAnimation(this, EventArgs.Empty);
            var teamFilter =  PixiFilters.GetTeamFilter(this.Owner.Color);
             this.PixiUnit.AnimationContainer.AddFilter(teamFilter);
             BlazorHealth.Draw();
             this.Game.LupusPixiApplication.ViewPort.AddChild(this.PixiUnit.Container);
        }

        public async override void Destroy()
        {
             base.Destroy();
            PixiUnit.ClickEvent -= ClickUnit;
            Game.ClickEvent -= Click;
            Game.TurnResolver.StartTurnEvent -= this.StartTurn;
            Game.UI.MouseRightClickEvent -= RightClick;
            if (BlazorHealth != null)
                 BlazorHealth.Dispose();
            if (PixiUnit != null)
                 PixiUnit.Dispose();
            ObjRef?.Dispose();
            
        }
    }
}
