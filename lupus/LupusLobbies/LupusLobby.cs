using Lobbies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Lupus;

namespace LupusLobbies
{
    public class LupusLobby : Lobby
    {

        public LupusLobby(string name, List<LupusLobbySlot> slots, string mapId) : base(name, slots.Cast<Slot>().ToList(), mapId)
        {
        }
        
        public LupusLobbySlot GetLupusLobbySlot(Guid id) => Slots.FirstOrDefault(slot => slot.Id == id) as LupusLobbySlot;
        public LupusLobbySlot GetLupusLobbySlot() => Slots[0] as LupusLobbySlot;
        public LupusLobbySlot GetLupusLobbySlot(string connectionId) => GetPlayerSlot(connectionId) as LupusLobbySlot;
        public LupusLobbySlot GetLupusLobbySlotByConnectionId(string connectionId) => GetPlayerSlotByConnectionId(connectionId) as LupusLobbySlot;

        public bool ChangeColor(string connectionId, KnownColor color)
        {
            var slot = GetLupusLobbySlotByConnectionId(connectionId);
            if (slot == null)
                return false;

            return slot.Color.ChangeColor(color);            
        }

        protected List<PlayerInfo> GetPlayers()
        {
            var players = new List<PlayerInfo>();

            foreach (var slot in this.Slots)
            {
                var lupusSlot = slot as LupusLobbySlot;
                var player = new PlayerInfo();
                player.Id = lupusSlot.LobbyPlayer.Id;
                player.Color = lupusSlot.Color.Color;
                player.Name = lupusSlot.LobbyPlayer.Name;
                player.Team = lupusSlot.Team;
                players.Add(player);
            }
            return players;
        }

        public override async Task StartGame()
        {
            throw new NotImplementedException();
        }
    }
}
