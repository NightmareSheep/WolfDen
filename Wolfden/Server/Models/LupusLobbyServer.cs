using Lupus.Factories;
using LupusLobbies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolfden.Shared;
using Wolfden.Server.Other;
using Newtonsoft.Json;
using Lupus.Other.MapLoading;
using Lupus;

namespace Wolfden.Server.Models
{
    public class LupusLobbyServer : LupusLobby
    {
        public LupusLobbyServer(string name, List<LupusLobbySlot> slots, string mapId, string webRootPath) : base(name, slots, mapId)
        {
            WebRootPath = webRootPath;
        }

        public string WebRootPath { get; }

        public override void StartGame()
        {
            ConcurrencyObjects.RemoveObjectWithoutLocking(Id);
            var mapString = File.ReadAllText(WebRootPath + "/game/maps/" + this.MapId + "/" + this.MapId + ".json");
            var jsonMap = JsonConvert.DeserializeObject<JsonMap>(mapString);
            var game = new Game(Id, this.GetPlayers(), jsonMap);
            game.GameInitializer.Initialize();
            ConcurrencyObjects.AddObject(Id, game);
        }
    }
}
