using Lupus.Other;
using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lupus
{
    public class Game
    {
        public Guid Id { get; set; }
        public Map Map { get; set; }
        public Dictionary<string, object> GameObjects = new Dictionary<string, object>();
        public List<Player> Players { get; set; }
        public History History { get; set; }
        public TurnResolver TurnResolver { get; set; }

        public Game(List<Player> players)
        {
            Id = Guid.NewGuid();
            History = new History(this);
            Map = new Map();
            Players = players;
            TurnResolver = new TurnResolver(this, players);
        }

        public T GetGameObject<T>(string playerId, string objectId) where T : class
        {
            var player = Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
                return null;

            player.GameObjects.TryGetValue(objectId, out var obj);
            var cast = obj as T;
            return cast;
        }

        public T GetGameObject<T>(string objectId) where T : class
        {
            GameObjects.TryGetValue(objectId, out var obj);
            var cast = obj as T;
            return cast;
        }

        public void RemoveObject(string id)
        {
            GameObjects.Remove(id);
            foreach (var player in Players)
            {
                player.GameObjects.Remove(id);
            }
        }

        public async Task EndGame()
        {
            var allObjects = this.GameObjects.Values.OfType<IDestroy>().ToList();
            for (var i = allObjects.Count - 1; i >= 0; i--)
            {
                i = Math.Min(i, allObjects.Count - 1);
                var obj = allObjects[i];
                await obj.Destroy();
            }

            this.GameObjects = null;
            this.TurnResolver = null;
            this.History = null;
            this.Map = null;
            this.Players = null;
        }
    }
}
