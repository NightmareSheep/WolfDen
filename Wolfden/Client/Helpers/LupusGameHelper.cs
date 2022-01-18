using Lupus;
using Lupus.Behaviours.Movement;
using LupusBlazor;
using LupusBlazor.Behaviours.Movement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wolfden.Client.Models;
using LupusBlazor.Factories;
using Newtonsoft.Json;
using System.Net.Http;
using Wolfden.Client.Pages.Game;
using Lupus.Other;
using LupusBlazor.Behaviours.Attack;
using System.Text.Json;
using System.Reflection;
using LupusBlazor.Other;
using LupusBlazor.UI;
using Wolfden.Shared;
using System.Net.Http.Json;
using Lupus.Other.MapLoading;

namespace WolfDen.Client.Helpers
{
    public class LupusGameHelper
    {
        public HubConnection HubConnection { get; }
        private BlazorGame Game { get; set; }
        public HttpClient HttpClient { get; }
        public IJSRuntime JSRuntime { get; }
        public string GameId { get; }
        public string MapName { get; set; }
        private BlazorGameFactory GameFactory { get; set; }
        private DotNetObjectReference<LupusGameHelper> objRef;

        public LupusGameHelper(HttpClient httpClient, IJSRuntime jSRuntime, NavigationManager navigationManager, ILoggerProvider loggerProvider, string gameId, IUI UI, string currentPlayerId)
        {
            HttpClient = httpClient;
            JSRuntime = jSRuntime;
            GameId = gameId;
            HubConnection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/lupusgamehub"))
            .ConfigureLogging(logging =>
            {
                logging.AddProvider(loggerProvider);
                //logging.AddConsole();
                // This will set ALL logging to Debug level
                logging.SetMinimumLevel(LogLevel.Information);
            })
            .Build();

            // Move
            HubConnection.On<string, int[]>("Move", async (id, path) => {
                var move = Game.GetGameObject<BlazorMove>(id);
                await move.MoveOverPath(path);
            });

            HubConnection.On<string, Direction>("DamageAndPush", async (id, direction) => {
                var skill = Game.GetGameObject<BlazorDamageAndPush>(id);
                await skill.DamageAndPushUnit(direction);
            });

            HubConnection.On<string>("EndTurn", async (playerId) => {
                await Game.BlazorTurnResolver.EndTurn(Game.Players.FirstOrDefault(p => p.Id == playerId));
            });

            HubConnection.On<string, string, string, string, JsonElement[], string[]>("DoMove", async (playerId, objectId, typeName, methodName, parameters, parameterTypeNames) =>
            {
                var type = Type.GetType(typeName);
                var parameterTypes = parameterTypeNames.Select(n => Type.GetType(n)).ToArray();
                var gameObject = Game.GetGameObject<object>(playerId, objectId);

                if (gameObject == null)
                    return;

                MethodInfo method = type.GetMethod(methodName, parameterTypes);

                var deserializedParamers = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    deserializedParamers[i] = JsonConvert.DeserializeObject(parameters[i].GetRawText(), parameterTypes[i]);

                method.Invoke(gameObject, deserializedParamers);
            });

            HubConnection.On<string, string, string>("Start", async (mapId, serializedHistory, serializedPlayers) => {
                Wolfden.Client.Other.Statics.AudioPlayer.SoundEnabled = false;
                var colorHelper = new ColorHelper();
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                var players = JsonConvert.DeserializeObject<List<Player>>(serializedPlayers, settings);
                GameFactory = new BlazorGameFactory(HttpClient, JSRuntime, HubConnection, UI, players, currentPlayerId, Wolfden.Client.Other.Statics.AudioPlayer);

                var mapString = await HttpClient.GetStringAsync("/api/map/" + mapId);
                var jsonMap = JsonConvert.DeserializeObject<JsonMap>(mapString);
                Game = await GameFactory.GetBlazorGame(jsonMap, mapId);
                Game.Id = new Guid(GameId);
                
                var moveHistory = JsonConvert.DeserializeObject<List<IHistoryMove>>(serializedHistory, settings);
                Game.History.Moves = moveHistory;
                await Game.History.PlayHistory();
                this.Game.AudioPlayer.SoundEnabled = true;
                await Game.Draw();
                await Game.UI.DoneLoading();
                await Wolfden.Client.Other.Statics.AudioPlayer.PlayMusic(LupusBlazor.Audio.Tracks.ExploringTheUnkown);

            });

            objRef = DotNetObjectReference.Create(this);
        }

        

        public async Task InitializeConnection()
        {
            await HubConnection.StartAsync();            
            await HubConnection.InvokeAsync("Join", GameId);            
        }

        public void Dispose()
        {
            objRef?.Dispose();
        }
    }
}
