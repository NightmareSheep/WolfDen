using Microsoft.AspNetCore.SignalR;
using Wolfden.Server.Other;
using Lupus;
using System;
using Lupus.Behaviours.Movement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Lupus.Other;
using Lupus.Behaviours.Attack;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wolfden.Server.Hubs.Lupus
{
    public class LupusGameHub : Hub<ILupusGameClient>
    {
        private const string GamePrefix = "game_";
        private const string GuestPrefix = "Guest_";

        public void Join(Guid gameId)
        {
            ConcurrencyObjects.ConcurentOperation(gameId, (Game game) => {
                Groups.AddToGroupAsync(Context.ConnectionId, GamePrefix + game.Id.ToString());
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

                var serializedHistory = JsonConvert.SerializeObject(game.History.Moves, settings);
                var serializedPlayers = System.Text.Json.JsonSerializer.Serialize(game.Players);
                Clients.Client(Context.ConnectionId).Start(game.Map.Name, serializedHistory, serializedPlayers);

            });
        }

        public void Move(Guid gameId, string playerId, string objectId, int[] path)
        {
            DoMoveOnGameObject<Move>(gameId, playerId, objectId, (Game game, Move gameObject) =>
            {
                gameObject?.MoveOverPath(path);
                Clients.Group(GamePrefix + game.Id.ToString()).Move(objectId, path);
            });
        }

        public void DamageAndPush(Guid gameId, string playerId, string skillId, Direction direction)
        {
            DoMoveOnGameObject<DamageAndPush>(gameId, playerId, skillId, (Game game, DamageAndPush gameObject) =>
            {
                gameObject?.DamageAndPushUnit(direction);
                Clients.Group(GamePrefix + game.Id.ToString()).DamageAndPush(skillId, direction);
            });
        }

        public void EndTurn(Guid gameId, string playerId)
        {
            var user = playerId.StartsWith(GuestPrefix) ? playerId : Context?.User?.Identity?.Name ?? GuestPrefix + playerId;
            ConcurrencyObjects.ConcurentOperation(gameId, (Game game) => {
                if (game?.TurnResolver?.EndTurn(game.Players.FirstOrDefault(p => p.Id == user)).Result ?? false)
                    Clients.Group(GamePrefix + game.Id.ToString()).EndTurn(playerId);
            });
        }

        private void DoMoveOnGameObject<T>(Guid gameId, string playerId, string objectId, Action<Game, T> action) where T : class
        {
            var user = playerId.StartsWith(GuestPrefix) ? playerId : Context?.User?.Identity?.Name ?? GuestPrefix + playerId;
            ConcurrencyObjects.ConcurentOperation(gameId, (Game game) => {
                var gameObject = game.GetGameObject<T>(user, objectId);
                if (gameObject != null)
                    action(game, gameObject);
            });
        }

        public void DoMove(Guid gameId, string playerId, string objectId, string typeName, string methodName, JsonElement[] parameters, string[] parameterTypeNames)
        {
            var user = playerId.StartsWith(GuestPrefix) ? playerId : Context?.User?.Identity?.Name ?? GuestPrefix + playerId;
            var type = Type.GetType(typeName);
            var parameterTypes = parameterTypeNames.Select(n => Type.GetType(n)).ToArray();

            ConcurrencyObjects.ConcurentOperation(gameId, (Game game) => {
                var gameObject = game.GetGameObject<object>(user, objectId);
                if (gameObject == null)
                    return;

                MethodInfo method = type.GetMethod(methodName, parameterTypes);

                var deserializedParamers = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    deserializedParamers[i] = JsonConvert.DeserializeObject(parameters[i].GetRawText(), parameterTypes[i]);

                method.Invoke(gameObject, deserializedParamers);
                Clients.Group(GamePrefix + game.Id.ToString()).DoMove(playerId, objectId, typeName, methodName, parameters, parameterTypeNames);
            });
        }
    }
}
