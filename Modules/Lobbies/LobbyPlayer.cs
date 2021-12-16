using System;

namespace Lobbies
{
    public class LobbyPlayer
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public bool Ready { get; set; }
        public bool LoggedIn { get; set; }

        public LobbyPlayer() { }
        public LobbyPlayer(string id, string connectionId, string name)
        {
            Id = id;
            ConnectionId = connectionId;
            Name = name;
        }
    }
}