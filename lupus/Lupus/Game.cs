using Lupus.Actions;
using Lupus.Other;
using Lupus.Other.MapLoading;
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
        public Dictionary<string, object> GameObjects;
        public List<Player> Players { get; set; }
        public History History { get; set; }
        public TurnResolver TurnResolver { get; set; }
        public ActionTracker ActionTracker { get; set; }
        public GameInitializer GameInitializer { get; set; }
        public List<PlayerInfo> PlayerInfos { get; set; }

        public Game(Guid id, List<PlayerInfo> playersInfos, JsonMap map)
        {
            Id = id;
            History = new History(this);            
            PlayerInfos = playersInfos;
            Map = new Map();
            GameInitializer = new GameInitializer(this, map);            
        }

        public async Task Initialize() => await GameInitializer.Initialize();

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
                player.RemoveGameObject(id);
            }
        }

        public async Task EndGame()
        {
            TurnResolver?.Dispose();

            var allObjects = this.GameObjects?.Values?.OfType<IDestroy>()?.ToList() ?? new List<IDestroy>();
            for (var i = allObjects.Count - 1; i >= 0; i--)
            {
                var index = Math.Min(i, allObjects.Count - 1);
                var obj = allObjects[index];
                await obj.Destroy();

            }

            var disposableObjects = this.GameObjects?.Values?.OfType<IDisposable>()?.ToList() ?? new List<IDisposable>();
            for (var i = disposableObjects.Count - 1; i >= 0; i--)
            {
                var index = Math.Min(i, disposableObjects.Count - 1);
                var obj = disposableObjects[index];
                obj.Dispose();
            }
            
        }
    }
}
