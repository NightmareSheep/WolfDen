using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using Lobbies;
using Wolfden.Server.Other;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Wolfden.Server.Hubs
{
    public class LobbyHub<T> : Hub<T> where T : class, ILobbyClient
    {
        protected virtual string HubPrefix { get { return "Lobby_"; } }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

        }

        public void ConnectToLobby(Guid lobbyId, string userId, string username)
        {
            var lobbyplayer = new LobbyPlayer(userId, Context.ConnectionId, username);
            ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) =>
            {
                Context.Items["lobby"] = lobbyId;
                Groups.AddToGroupAsync(Context.ConnectionId, HubPrefix + lobby.Id.ToString());
                var serializedSlots = JsonConvert.SerializeObject(lobby.Slots);
                var serializedSpectators = JsonConvert.SerializeObject(lobby.Spectators);
                Clients.Caller.Initialize(lobby.Name, lobby.Id, lobby.MapId, serializedSlots, serializedSpectators);
            });
        }

        public void Join(Guid lobbyId, string userId, string username, int slot = -1)
        {
            var lobbyplayer = new LobbyPlayer(userId, Context.ConnectionId, username);
            ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) => {
                if (lobby.Join(lobbyplayer, slot))
                    Clients.Group(HubPrefix + lobby.Id.ToString()).Join(lobbyplayer, slot);
            });
        }

        public void JoinAsSpectator(Guid lobbyId, string userId, string username)
        {
            var lobbyplayer = new LobbyPlayer(userId, Context.ConnectionId, username);
            ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) => {
                lobby.JoinAsSpectator(lobbyplayer);
                Clients.Group(HubPrefix + lobby.Id.ToString()).JoinAsSpectator(lobbyplayer);
            });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            if (Context.Items.TryGetValue("lobby", out var value))
            {
                var lobbyId = (Guid)value;
                await Clients.Group(HubPrefix + lobbyId.ToString()).Leave(Context.ConnectionId);
                
                ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) => {
                    lobby.Leave(Context.ConnectionId);                    
                });
            }
        }

        public void Ready()
        {
            if (Context.Items.TryGetValue("lobby", out var value))
            {
                var lobbyId = (Guid)value;
                ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) => {
                    lobby.Ready(Context.ConnectionId, true);
                    Clients.Group(HubPrefix + lobby.Id.ToString()).Ready(Context.ConnectionId);
                });
            }
        }

        public void Switch(Guid destinationId, string userId, string username)
        {
            var lobbyplayer = new LobbyPlayer(userId, Context.ConnectionId, username);
            if (Context.Items.TryGetValue("lobby", out var value))
            {
                var lobbyId = (Guid)value;
                ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) => {
                    if (lobby.Switch(Context.ConnectionId, lobbyplayer, destinationId))
                        Clients.Group(HubPrefix + lobby.Id.ToString()).Switch(destinationId, lobbyplayer);
                });
            }
        }

        public void Leave()
        {
            if (Context.Items.TryGetValue("lobby", out var value))
            {
                var lobbyId = (Guid)value;
                ConcurrencyObjects.ConcurentOperation<Lobby>(lobbyId, (lobby) => {
                    lobby.Leave(Context.ConnectionId);
                    Clients.Group(HubPrefix + lobby.Id.ToString()).Leave(Context.ConnectionId);
                });
            }
        }
    }
}
