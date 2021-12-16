using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lobbies
{
    public abstract class Lobby
    {
        public List<Slot> Slots { get; set; }        
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<LobbyPlayer> Spectators { get; set; } = new();

        public event EventHandler StateHasChangedEvent;
        public string Name { get; set; }
        public string MapId { get; set; }

        public Slot GetSlot(Guid id) => Slots.FirstOrDefault(s => s.Id == id);

        public bool IsEmpty() => Slots.All(slot => slot.LobbyPlayer == null);

        public void RaiseStateHasChangedEvent() => StateHasChangedEvent?.Invoke(this, new EventArgs());
        
        public Slot GetPlayerSlot(string connectionId) => Slots.FirstOrDefault(slot => slot.LobbyPlayer?.ConnectionId == connectionId);
        public Slot GetPlayerSlotByConnectionId(string connectionId) => Slots.FirstOrDefault(slot => slot.LobbyPlayer?.ConnectionId == connectionId);
        public LobbyPlayer GetLobbyPlayer(string connectionId) => GetPlayerSlot(connectionId)?.LobbyPlayer;

        private void StateHasChanged()
        {
            foreach (var slot in Slots.Where(slot => slot.LobbyPlayer != null)) {
                slot.LobbyPlayer.Ready = false;
            }
            RaiseStateHasChangedEvent();
        }

        public Lobby() { }

        public Lobby(string name, List<Slot> slots, string mapId)
        {
            Name = name;
            Slots = slots;
            this.MapId = mapId;
        }

        public bool Join(LobbyPlayer lobbyPlayer, int index = -1)
        {
            if (index >= this.Slots.Count)
                return false;

            this.Leave(lobbyPlayer.ConnectionId);

            var slots = index == -1 ? this.Slots : new() { this.Slots[index] };
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot.LobbyPlayer == null)
                {
                    slot.LobbyPlayer = lobbyPlayer;
                    StateHasChanged();
                    return true;
                }
            }

            JoinAsSpectator(lobbyPlayer);
            return false;
        }

        public void JoinAsSpectator(LobbyPlayer lobbyPlayer)
        {
            this.Leave(lobbyPlayer.ConnectionId);
            Spectators.Add(lobbyPlayer);
            StateHasChanged();
        }

        public void Leave(string connectionId)
        {
            var playerSlot = Slots.FirstOrDefault(slot => slot.LobbyPlayer?.ConnectionId == connectionId);
            if (playerSlot != null)
            {
                playerSlot.LobbyPlayer = null;
                StateHasChanged();
            }

            var spectator = this.Spectators.FirstOrDefault(s => s.ConnectionId == connectionId);
            if (spectator != null)
            {
                this.Spectators.Remove(spectator);
                StateHasChanged();
            }
        }

        public async Task<bool> Ready(string connectionId, bool ready)
        {
            var positionSlot = Slots.FirstOrDefault(slot => slot.LobbyPlayer?.ConnectionId?.Equals(connectionId) ?? false);
            if (positionSlot?.LobbyPlayer == null) return false;
            positionSlot.LobbyPlayer.Ready = ready;

            if (Slots.All(slot => slot.LobbyPlayer == null || slot.LobbyPlayer.Ready) && Slots.Where(slot => slot.LobbyPlayer != null).Select(slot => slot.Team).Distinct().Count() > 1)
            {
                await StartGame();
                return true;
            }
            return false;
        }

        public bool Switch(string connectionId, LobbyPlayer player, Guid destinationId)
        {
            var destinationSlot = Slots.FirstOrDefault(Slot => Slot.Id == destinationId);
            if (destinationSlot != null && destinationSlot.LobbyPlayer == null)
            {
                Leave(connectionId);
                destinationSlot.LobbyPlayer = player;
                RaiseStateHasChangedEvent();
                return true;
            }
            return false;
        }

        public abstract Task StartGame();
    }
}