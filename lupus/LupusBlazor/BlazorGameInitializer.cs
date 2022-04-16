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
            mapFactory = new BlazorMapLoader(game, game.JSRuntime);
        }

        BlazorGame blazorGame;

        public override async Task Initialize()
        {
            await game.EndGame();
            blazorGame?.BlazorUI?.Dispose();
            var players = new List<Player>();
            foreach (var playerInfo in game.PlayerInfos)
                players.Add(new BlazorPlayer(blazorGame, playerInfo));
            game.Players = players;
            game.TurnResolver = blazorGame.BlazorTurnResolver = new BlazorTurnResolver(blazorGame, blazorGame.Players, blazorGame.CurrentPlayer);
            game.ActionTracker = new ActionTracker();
            game.GameObjects = new Dictionary<string, object>();
            blazorGame.BlazorUI = new UI.BlazorUI(blazorGame, blazorGame.BlazorTurnResolver, blazorGame.UI);

            foreach (var player in game.Players)
            {
                var undo = new BlazorUndo(blazorGame, blazorGame.BlazorUI, player as BlazorPlayer);
                player.GameObjects.Add("player " + player.Id + " undo", undo);
            }

            mapFactory.LoadMap(game, jsonMap, jsonMap.Name);
            await game.TurnResolver.StartTurn();
            await game.History.PlayHistory();
            await blazorGame.Draw();
        }
    }
}
