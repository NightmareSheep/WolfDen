using LupusLobbies;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor
{
    public class LupusLobbyBlazor : LupusLobby
    {
        public NavigationManager NavigationManager { get; }

        public LupusLobbyBlazor(string name, List<LupusLobbySlot> slots, string mapId, NavigationManager navigationManager) : base(name, slots, mapId)
        {
            NavigationManager = navigationManager;
        }        

        public override void StartGame()
        {
            NavigationManager.NavigateTo("game/" + Id);
        }
    }
}
