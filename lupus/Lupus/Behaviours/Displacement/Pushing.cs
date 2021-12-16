using Lupus.Other;
using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours.Displacement
{
    public class Pushing
    {
        public Tile Tile { get; }

        public event Func<Push, Task> PushEvent;

        public Pushing(Tile tile)
        {
            Tile = tile;
        }

        public async Task Push(Direction direction)
        {
            if (PushEvent != null)
                await PushEvent?.Invoke(new Push(direction, 1, Tile));
        }
    }
}
