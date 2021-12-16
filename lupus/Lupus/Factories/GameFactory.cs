using Lupus.Other.MapLoading;
using Lupus.Tiles;
using Lupus.WinConditions.GatherChests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace Lupus.Factories
{
    public class GameFactory
    {
        protected MapFactory MapFactory { get; set; }
        protected Game Game { get; set; }
        

        public GameFactory(List<Player> players)
        {
            Game = new Game(players);
            MapFactory = new MapFactory(Game);
        }

        public async Task<Game> GetGame(JsonMap jsonMap, string mapName)
        {
            MapFactory.LoadMap(Game, jsonMap, mapName);
            await Game.TurnResolver.StartTurn();
            return Game;
        }

        protected async virtual Task<JsonMap> LoadMap(string assetPath)
        {
            var mapString = await File.ReadAllTextAsync(assetPath);
            return JsonConvert.DeserializeObject<JsonMap>(mapString);
        }
    }
}
