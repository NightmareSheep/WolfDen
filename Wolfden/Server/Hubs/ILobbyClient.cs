using Lobbies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wolfden.Server.Hubs
{
    public interface ILobbyClient
    {
        Task GetMessage(string message);
        Task Ready(string connectionId);
        Task Leave(string connectionId);
        Task Join(LobbyPlayer player, int slot);
        Task JoinAsSpectator(LobbyPlayer player);
        Task Switch(Guid destinationId, LobbyPlayer player);        
        Task StartGame();
        Task Initialize(string name, Guid id, string mapId, string serializedSlots, string serializedSpectators);
    }
}
