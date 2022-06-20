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
using PIXI;
using BlazorJavascriptHelper;

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
        public event EventHandler<int> TileClickEvent;


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

        public void StartTurn(object sender, List<Player> activePlayers)
        {
             this.RemoveIndicators();
        }

        public void Spawn(IEnumerable<int> indices)
        {
            var indicesList = indices.ToList();
             RemoveIndicators();

            for (var i = 0; i < indicesList.Count(); i++)
            {
                var index = indicesList[i];
                var tile = Map.GetTile(index);
                var indicator = new TileIndicator(this.Game, this, tile, IndicatorSprites[i], this.Tint, JSRuntime);
                Indicators.Add(indicator);
                 indicator.Draw();
            }

            
        }

        public void ClickTile(int index)
        {
             Game.RaiseClickEvent(this);
             RaiseTileClickEvent(index);
        }

        public void RemoveIndicators(object sender, EventArgs e)
        {
            RemoveIndicators();
        }

        public void RemoveIndicators()
        {
            foreach (var indicator in Indicators ?? Enumerable.Empty<TileIndicator>())
                 indicator.Destroy();
            Indicators = new List<TileIndicator>();
        }

        public void Destroy()
        {
            Game.UI.MouseRightClickEvent -= this.RemoveIndicators;
            Game.TurnResolver.StartTurnEvent -= StartTurn;
             RemoveIndicators();
        }

        private void RaiseTileClickEvent(int index)
        {
            TileClickEvent?.Invoke(this, index);
        }

        private void InitializeIndicatorSprites(object sender, EventArgs e)
        {
            if (TileIndicators.IndicatorSprites != null)
                return;


            var jsHelper =  JavascriptHelper.Instance;
            var texture =  jsHelper.GetJavascriptProperty<IJSInProcessObjectReference>(new string[] { "PIXI", "Texture", "WHITE" });

            TileIndicators.IndicatorSprites = new List<Sprite>();
            for (int i = 0; i < 100; i++)
            {
                
                var sprite = new Sprite(texture);
                sprite.Interactive = true;
                sprite.Width = 16;
                sprite.Height = 16;
                sprite.Alpha = 0.5f;
                IndicatorSprites.Add(sprite);
            }

             texture.DisposeAsync();
        }
    }
}
