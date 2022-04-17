using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;

namespace Lupus
{
    public class Player
    {
        public Player() { }

        public Player(Game game, PlayerInfo playerInfo)
        {
            GameObjects = new ReadOnlyDictionary<string, object>(PrivateGameObjectsDictionary);
            Id = playerInfo.Id;
            Name = playerInfo.Name;
            Team = playerInfo.Team;
            Color = playerInfo.Color;
            this.game = game;
        }

        public string Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        private Dictionary<string, object> PrivateGameObjectsDictionary = new Dictionary<string, object>();

        [JsonIgnore]
        public ReadOnlyDictionary<string, object> GameObjects;
        private readonly Game game;

        public int Team { get; set; }
        public KnownColor Color { get; set; }

        public void AddGameObject(string id, object obj)
        {
            PrivateGameObjectsDictionary[id] = obj;

            if (game.GameObjects.TryGetValue(id, out var gameObject) && gameObject != obj)
                throw new Exception("There is an object in the game's gameobject dictionary with the same Id as the object we are trying to add to the player gameobjects dictionary but it is not the same objecter. Duplicate ID's are not allowed. ID: " + id);

            game.GameObjects[id] = obj;
        }

        public void RemoveGameObject(string id)
        {
            if (PrivateGameObjectsDictionary.ContainsKey(id))
                PrivateGameObjectsDictionary.Remove(id);
            if (game.GameObjects.ContainsKey(id))
                game.GameObjects.Remove(id);
        }

    }
}
