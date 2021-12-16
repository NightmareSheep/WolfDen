using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolfden.Server.Other;
using LupusLobbies;
using System.Drawing;

namespace Wolfden.Server.Hubs
{
    public class LupusLobbyHub : LobbyHub<IlupusLobbyClient>
    {
        public void ChangeColor(KnownColor color)
        {
            if (Context.Items.TryGetValue("lobby", out var value))
            {
                var lobbyId = (Guid)value;
                ConcurrencyObjects.ConcurentOperation<LupusLobby>(lobbyId, (lobby) => {
                    if (lobby.ChangeColor(Context.ConnectionId, color))
                        Clients.Group(HubPrefix + lobby.Id.ToString()).ChangeColor(Context.ConnectionId, color);
                });
            }
        }
    }
}
