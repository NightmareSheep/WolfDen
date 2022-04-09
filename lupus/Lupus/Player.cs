using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;

namespace Lupus
{
    public class Player
    {
        public Player(Game game, PlayerInfo playerInfo)
        {
            Id = playerInfo.Id;
            Name = playerInfo.Name;
            Team = playerInfo.Team;
            Color = playerInfo.Color;
            this.game = game;
            var undo = new Undo(game);
            GameObjects["player " + Id + " undo"] = undo;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public Dictionary<string, object> GameObjects = new Dictionary<string, object>();
        private readonly Game game;

        public int Team { get; set; }
        public KnownColor Color { get; set; }

    }
}
