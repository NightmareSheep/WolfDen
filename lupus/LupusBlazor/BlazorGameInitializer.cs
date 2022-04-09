using Lupus;
using Lupus.Actions;
using Lupus.Other.MapLoading;
using LupusBlazor.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor
{
    public class BlazorGameInitializer : GameInitializer
    {
        public BlazorGameInitializer(BlazorGame game, JsonMap jsonMap) : base(game, jsonMap)
        {
            blazorGame = game;
            mapFactory = new BlazorMapFactory(game, game.JSRuntime);
        }

        BlazorGame blazorGame;

        public override async Task Initialize()
        {
            await game.EndGame();
            var players = new List<Player>();
            foreach (var playerInfo in game.PlayerInfos)
                players.Add(new BlazorPlayer(game, playerInfo));
            game.Players = players;
            game.TurnResolver = blazorGame.BlazorTurnResolver = new BlazorTurnResolver(blazorGame, blazorGame.Players, blazorGame.CurrentPlayer);
            game.ActionTracker = new ActionTracker();
            game.GameObjects = new Dictionary<string, object>();

            mapFactory.LoadMap(game, jsonMap, jsonMap.Name);
            await game.History.PlayHistory();
            await blazorGame.Draw();
        }
    }
}
