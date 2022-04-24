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

        public event EventHandler<Push> PushEvent;

        public Pushing(Tile tile)
        {
            Tile = tile;
        }

        public void Push(Direction direction)
        {
            PushEvent?.Invoke(this, new Push(direction, 1, Tile));
        }
    }
}
