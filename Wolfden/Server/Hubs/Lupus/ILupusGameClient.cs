using Lupus.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Wolfden.Server.Hubs.Lupus
{
    public interface ILupusGameClient
    {
        Task Start(string mapId, string serializedHistory, string serializedPlayers);
        Task Move(string moveId, int[] path);
        Task DamageAndPush(string skillId, Direction direction);
        Task EndTurn(string playerId);
        Task DoMove(string playerId, string objectId, string objectTypeName, string methodName, JsonElement[] parameters, string[] parameterTypeNames);
    }
}
