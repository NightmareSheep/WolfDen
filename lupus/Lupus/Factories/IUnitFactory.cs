using System;
using System.Collections.Generic;
using System.Text;
using Lupus.Tiles;
using Lupus.Units.Orcs;

namespace Lupus.Factories
{
    public interface IUnitFactory
    {
        void AddUnit(Player player, uint typeId, string id, Tile tile);
        void AddGrunt(Player player, string id, Tile tile);
        void AddWinCondition();
    }
}
