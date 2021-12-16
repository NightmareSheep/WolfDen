using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lobbies;
using Lobbies.LobbyBehaviour;

namespace LupusLobbies
{
    public class LupusLobbySlot : Slot
    {
        public SlotColor Color;

        public LupusLobbySlot(int team, SlotColor color) : base(team)
        {
            Team = team;
            Color = color;
        }
    }
}
