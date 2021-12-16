using Lupus.Behaviours.Defend;
using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Units
{
    public interface IUnit
    {
        string Id { get; }
        Health Health { get; }
        Tile Tile { get; set; }
        Player Owner { get; }
    }
}
