using Lupus;
using Lupus.Tiles;
using LupusBlazor.Units;
using LupusBlazor.Units.Orcs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lupus.Factories;
using System.Net.Http;
using Lupus.Other.MapLoading;
using System.Net.Http.Json;
using LupusBlazor.UI;
using System.Drawing;
using System.Linq;

namespace LupusBlazor.Factories
{
    public class BlazorGameFactory : GameFactory
    {
        public HttpClient HttpClient { get; }
        public IJSRuntime JSRuntime { get; }
        public HubConnection HubConnection { get; }
        public BlazorGame BlazorGame { get; }

        public BlazorGameFactory(HttpClient httpClient, IJSRuntime jSRuntime, HubConnection hubConnection, IUI UI, List<Player> players, string currentPlayerId, Audio.AudioPlayer audioPlayer) : base(players)
        {
            HubConnection = hubConnection;
            var currentPlayer = players.FirstOrDefault(p => p.Id == currentPlayerId) ?? players.FirstOrDefault(p => p.Id == "Guest_Test");
            BlazorGame = new BlazorGame(players, currentPlayer, HubConnection, jSRuntime, UI, audioPlayer);
            Game = BlazorGame;
            HttpClient = httpClient;
            JSRuntime = jSRuntime;
            HubConnection = hubConnection;
            this.MapFactory = new BlazorMapFactory(BlazorGame, this.JSRuntime);
        }

        public async Task<BlazorGame> GetBlazorGame(JsonMap jsonMap, string mapName)
        {
            return await GetGame(jsonMap, mapName) as BlazorGame;
        }

        protected override async Task<JsonMap> LoadMap(string assetPath)
        {
            var map = await HttpClient.GetFromJsonAsync<JsonMap>(assetPath);
            return map;
        }
    }
}
