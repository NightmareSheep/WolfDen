using Lupus;
using LupusBlazor.Behaviours.Movement;
using LupusBlazor.Pixi;
using LupusBlazor.Units;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Interaction
{
    public class TileIndicators
    {
        private static List<Sprite> IndicatorSprites;
        private BlazorGame Game { get; }
        public Map Map { get; }
        private IJSRuntime JSRuntime { get; }
        public KnownColor Tint { get; }
        public List<TileIndicator> Indicators { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public event Func<int, Task> TileClickEvent;


        public TileIndicators(BlazorGame game, Map map, IJSRuntime jSRuntime, KnownColor tint)
        {
            Game = game;
            Map = map;
            JSRuntime = jSRuntime;
            Tint = tint;
            game.TurnResolver.StartTurnEvent += StartTurn;
            game.UI.MouseRightClickEvent += this.RemoveIndicators;
            game.DrawEvent += InitializeIndicatorSprites;
        }

        public async Task StartTurn(List<Player> activePlayers)
        {
            await this.RemoveIndicators();
        }

        public async Task Spawn(IEnumerable<int> indices)
        {
            var indicesList = indices.ToList();
            await RemoveIndicators();

            for (var i = 0; i < indicesList.Count(); i++)
            {
                var index = indicesList[i];
                var tile = Map.GetTile(index);
                var indicator = new TileIndicator(this.Game, this, tile, IndicatorSprites[i], this.Tint, JSRuntime);
                Indicators.Add(indicator);
                await indicator.Draw();
            }

            
        }

        public async Task ClickTile(int index)
        {
            await Game.RaiseClickEvent(this);
            await RaiseTileClickEvent(index);
        }

        public async Task RemoveIndicators()
        {
            foreach (var indicator in Indicators ?? Enumerable.Empty<TileIndicator>())
                await indicator.Destroy();
            Indicators = new List<TileIndicator>();
        }

        public async Task Destroy()
        {
            Game.UI.MouseRightClickEvent -= this.RemoveIndicators;
            Game.TurnResolver.StartTurnEvent -= StartTurn;
            await RemoveIndicators();
        }

        private async Task RaiseTileClickEvent(int index)
        {
            if (TileClickEvent != null)
                await TileClickEvent?.Invoke(index);
        }

        private async Task InitializeIndicatorSprites()
        {
            if (TileIndicators.IndicatorSprites != null)
                return;


            var jsHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            var texture = await jsHelper.GetJavascriptProperty<IJSObjectReference>(new string[] { "PIXI", "Texture", "WHITE" });

            TileIndicators.IndicatorSprites = new List<Sprite>();
            for (int i = 0; i < 100; i++)
            {
                
                var sprite = new Sprite(JSRuntime, texture);
                await sprite.Initialize();
                sprite.Interactive = true;
                sprite.Width = 16;
                sprite.Height = 16;
                sprite.Alpha = 0.5f;
                IndicatorSprites.Add(sprite);
            }

            await texture.DisposeAsync();
        }
    }
}
