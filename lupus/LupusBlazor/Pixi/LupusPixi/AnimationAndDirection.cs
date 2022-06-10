using Lupus.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public struct AnimationAndDirection
    {
        public AnimationAndDirection(Animations animation, Direction direction)
        {
            Animation = animation;
            Direction = direction;
        }

        public Animations Animation { get; }
        public Direction Direction { get; }
    }
}
