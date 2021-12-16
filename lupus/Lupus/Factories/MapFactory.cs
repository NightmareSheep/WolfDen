using Lupus.Other.MapLoading;
using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Factories
{
    public class MapFactory
    {
        protected IUnitFactory UnitFactory { get; set; }
        protected TileFactory TileFactory { get; set; }
        private int Counter { get; set; }
        private Player Neutral { get; set; }
        private Player NeutralPlayer = new Player() { Id = "Neutral", Name = "Neutral", Team = -100, Color = KnownColor.Gray };

        public MapFactory(Game game)
        {
            UnitFactory = new UnitFactory(game);
            TileFactory = new();
        }

        public void LoadMap(Game game, JsonMap jsonMap, string mapName)
        {
            var map = game.Map;
            map.Tiles = new Tile[jsonMap.Width, jsonMap.Height];
            for (var x = 0; x < map.Tiles.GetLength(0); x++)
                for (var y = 0; y < map.Tiles.GetLength(1); y++)
                    map.Tiles[x, y] = new Tile(map, x, y);


            foreach (var layer in jsonMap.layers)
            {
                LoadLayer(game, game.Map, jsonMap, layer);
            }
            map.Name = mapName;
            UnitFactory.AddWinCondition();
        }

        private void LoadLayer(Game game, Map map, JsonMap jsonMap, Layer layer)
        {
            if (layer.layers != null)
            {
                foreach (var innerLayer in layer.layers)
                {
                    LoadLayer(game, map, jsonMap, innerLayer);
                }
                return;
            }

            for (var i = 0; i < layer.Data.Length; i++)
            {
                var d = layer.Data[i];
                if (d == 0)
                    continue;
                var x = i % map.Tiles.GetLength(0);
                var y = i / map.Tiles.GetLength(1);
                var tile = map.Tiles[x, y];

                switch (d)
                {
                    case 335:
                        var p = GetPlayer(game, layer);
                        if (p == null)
                            break;
                        UnitFactory.AddUnit(p, d, "Unit " + (Counter++), tile);
                        break;
                    case (>= 1 and <= 480):
                        TileFactory.AddTile(map, x, y, d);
                        break;
                    case (>= 481):
                        var player = GetPlayer(game, layer);
                        if (player == null)
                            break;
                        UnitFactory.AddUnit(player, d, "Unit " + (Counter++), tile);
                        break;                       
                }
            }

            
        }

        private Player GetPlayer(Game game, Layer layer)
        {
            if (layer.Name.Contains("Team"))
            {
                var playerNumber = int.Parse(layer.Name[5].ToString()) - 1;
                return game.Players[playerNumber % game.Players.Count];
            }

            if (layer.Name.Contains("Neutral"))
                return NeutralPlayer;
            
            return null;
        }
    }
}
