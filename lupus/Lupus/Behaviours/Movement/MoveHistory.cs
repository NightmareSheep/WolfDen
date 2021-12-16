using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours.Movement
{
    [Serializable]
    public class MoveHistory : IHistoryMove
    {
#pragma warning disable CA2235 // Mark all non-serializable fields
        public string Id { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields
        public int[] Path { get; set; }

        public MoveHistory(string id, int[] path)
        {
            Id = id;
            Path = path;
        }

        public async Task Execute(Game game)
        {
            Move move = game.GameObjects[Id] as Move;
            await move.MoveOverPath(Path);
        }
    }
}
