using Lupus.Actions;
using Lupus.Factories;
using Lupus.Other.MapLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public class GameInitializer : IGameInitializer
    {    
        public GameInitializer(Game game, JsonMap jsonMap)
        {
            mapFactory = new MapFactory(game);
            this.jsonMap = jsonMap;
            this.game = game;
        }

        protected MapFactory mapFactory;
        protected readonly JsonMap jsonMap;
        protected readonly Game game;

        public virtual async Task Initialize()
        {
            await game.EndGame();

            game.ActionTracker = new ActionTracker();
            game.GameObjects = new Dictionary<string, object>();

            var players = new List<Player>();
            foreach (var playerInfo in game.PlayerInfos)
                players.Add(new Player(game, playerInfo));
            game.Players = players;

            game.TurnResolver = new TurnResolver(game, game.Players);
            

            mapFactory.LoadMap(game, jsonMap, jsonMap.Name);
            await game.History.PlayHistory();
        }
    }
}
