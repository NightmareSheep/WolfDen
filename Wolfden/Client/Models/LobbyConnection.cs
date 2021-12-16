using Lobbies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LupusLobbies;
using Newtonsoft.Json;
//using Microsoft.AspNetCore.SignalR.Protocols.NewtonsoftJson;

namespace Wolfden.Client.Models
{
    public class LobbyConnection
    {
        public Lobby Lobby { get; protected set; }
        public HubConnection Connection { get; }
        public NavigationManager NavigationManager { get; }
        protected string UserName { get; set; }
        protected string UserId { get; set; }

        public event EventHandler StateHasChanged;

        public LobbyConnection(NavigationManager navigationManager, ILoggerProvider loggerProvider, string userName, string userId)
        {
            this.UserName = userName;
            this.UserId = userId;

            Connection = new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri("/lupusLobbyhub"))
            .ConfigureLogging(logging =>
            {
                logging.AddProvider(loggerProvider);
                //logging.AddConsole();
                // This will set ALL logging to Debug level
                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .Build();

            Connection.On<string>("Ready", async (connectionId) =>
            {
                await Lobby?.Ready(connectionId, true);
                OnStateHasChanged(new EventArgs());
            });

            Connection.On<string>("Leave", (connectionId) =>
            {
                Lobby?.Leave(connectionId);
                OnStateHasChanged(new EventArgs());
            });

            Connection.On<LobbyPlayer, int>("Join", (player, slot) =>
            {
                Lobby?.Join(player, slot);
                OnStateHasChanged(new EventArgs());
            });

            Connection.On<LobbyPlayer>("JoinAsSpectator", (player) =>
            {
                Lobby?.JoinAsSpectator(player);
                OnStateHasChanged(new EventArgs());
            });

            Connection.On<Guid, LobbyPlayer>("Switch", (destinationId, player) =>
            {
                Lobby?.Switch(player.ConnectionId, player, destinationId);
                OnStateHasChanged(new EventArgs());
            });

            LobbyInitialization();
            NavigationManager = navigationManager;
        }

        protected virtual void LobbyInitialization()
        {
            Connection.On<string>("Initialize", async (lobby) =>
            {
                Lobby = JsonConvert.DeserializeObject<Lobby>(lobby);
                OnStateHasChanged(new EventArgs());
                await Connection.SendAsync("Join", Lobby.Id, UserId, UserName);
            });
        }

        protected void OnStateHasChanged(EventArgs e) => StateHasChanged.Invoke(this, e);
        public async Task Ready() => await Connection.SendAsync("Ready");
        public async Task Join(int slot = -1) => await Connection.SendAsync("Join", Lobby.Id, UserId, UserName, slot);
        public async Task JoinAsSpectator() => await Connection.SendAsync("JoinAsSpectator", Lobby.Id, UserId, UserName);

        public async Task Initialize(string lobbyId, string UserId, string UserName)
        {
            await Connection.StartAsync();
            await Connection.SendAsync("ConnectToLobby", lobbyId, UserId, UserName);
        }


    }
}
