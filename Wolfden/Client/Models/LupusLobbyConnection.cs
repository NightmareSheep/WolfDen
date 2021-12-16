using Lobbies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using LupusLobbies;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using LupusBlazor;
using System.Drawing;

namespace Wolfden.Client.Models
{
    public class LupusLobbyConnection : LobbyConnection
    {
        public LupusLobby LupusLobby { get; protected set; }
        

        public LupusLobbyConnection(NavigationManager navigationManager, ILoggerProvider loggerProvider, string userName, string userId) : base(navigationManager, loggerProvider, userName, userId)
        {
            Connection.On<string, KnownColor>("ChangeColor", (connectionId, color) =>
            {
                LupusLobby?.ChangeColor(connectionId, color);
                OnStateHasChanged(new EventArgs());
            });
        }

        protected override void LobbyInitialization()
        {
            Connection.On<string, Guid, string, string, string>("Initialize", async (name, id, mapName, lobbySlots, lobbySpectators) =>
            {
                var slots = JsonConvert.DeserializeObject<List<LupusLobbySlot>>(lobbySlots);
                var spectators = JsonConvert.DeserializeObject<List<LobbyPlayer>>(lobbySpectators);
                LupusLobby = new LupusLobbyBlazor(name, slots, mapName, NavigationManager);
                LupusLobby.Spectators = spectators;
                LupusLobby.Id = id;
                Lobby = LupusLobby;
                OnStateHasChanged(new EventArgs());
                await Connection.SendAsync("Join", LupusLobby.Id, UserId, UserName, -1);
            });
        }
    }
}
