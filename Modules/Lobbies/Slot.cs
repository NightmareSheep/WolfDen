using System;

namespace Lobbies
{
    public class Slot
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Team { get; set; }
        public LobbyPlayer LobbyPlayer { get; set; }

        public Slot(int team) {
            Team = team;
        }
    }
}