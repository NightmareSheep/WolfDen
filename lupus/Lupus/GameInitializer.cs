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
            mapFactory = new MapLoader(game);
            this.jsonMap = jsonMap;
            this.game = game;
        }

        protected MapLoader mapFactory;
        protected readonly JsonMap jsonMap;
        protected readonly Game game;

        public virtual void Initialize()
        {
            game.EndGame();

            game.ActionTracker = new ActionTracker();
            game.GameObjects = new Dictionary<string, object>();

            var players = new List<Player>();
            foreach (var playerInfo in game.PlayerInfos)
                players.Add(new Player(game, playerInfo));
            game.Players = players;

            game.TurnResolver = new TurnResolver(game, game.Players);
            
            foreach (var player in game.Players)
            {
                var undo = new Undo(game, player);
                player.AddGameObject("player " + player.Id + " undo", undo);
            }

            mapFactory.LoadMap(game, jsonMap, jsonMap.Name);
            game.TurnResolver.StartTurn();
            game.History.PlayHistory();
        }
    }
}
