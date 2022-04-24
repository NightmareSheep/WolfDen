using Lupus.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours.Attack
{
    [Serializable]
    public class DamageAndPushHistory : IHistoryMove
    {
#pragma warning disable CA2235 // Mark all non-serializable fields
        public string Id { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields
        public Direction Direction { get; set; }

        public DamageAndPushHistory(string id, Direction direction)
        {
            Id = id;
            Direction = direction;
        }

        public void Execute(Game game)
        {
            DamageAndPush skill = game.GameObjects[Id] as DamageAndPush;
            skill.DamageAndPushUnit(Direction);
        }
    }
}
