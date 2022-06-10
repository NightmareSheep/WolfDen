using Lupus.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public struct AnimationConfiguration
    {
        public AnimationConfiguration(Actors actor, Animations animation, Direction direction)
        {
            Actor = actor;
            Animation = animation;
            Direction = direction;
        }

        public Actors Actor { get; }
        public Animations Animation { get; }
        public Direction Direction { get; }
    }
}
