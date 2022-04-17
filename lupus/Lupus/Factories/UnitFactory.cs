using System;
using System.Collections.Generic;
using System.Text;
using Lupus.Units.Orcs;
using Lupus.Tiles;
using Lupus.WinConditions.GatherChests;
using System.Linq;
using Lupus.Units;

namespace Lupus.Factories
{
    public class UnitFactory : IUnitFactory
    {
        public Game Game { get; }

        public UnitFactory(Game game)
        {
            Game = game;
        }        

        public void AddGrunt(Player player, string id, Tile tile) {
            var grunt = new Grunt(Game, player, id, tile);
            player.AddGameObject(grunt.Id, grunt);
        }

        public void AddUnit(Player player, uint typeId, string id, Tile tile)
        {
            var unitsFirstGid = 481;
            var markingsFirstGid = 491;


            switch (typeId)
            {
                case 335:
                    var chest = new Chest(Game, id, tile, player);
                    Game.GameObjects.Add(chest.Id, chest);
                    break;
                case 482:
                    var hero = new Hero(Game, player, id, tile);
                    player.AddGameObject(hero.Id, hero);
                    break;
                case 483:
                    var grunt = new Grunt(Game, player, id, tile);
                    player.AddGameObject(grunt.Id, grunt);
                    break;
                case 484:
                    var goblin = new Goblin(Game, player, id, tile);
                    player.AddGameObject(goblin.Id, goblin);
                    break;
                case 485:
                    var slime = new Slime(Game, player, id, tile);
                    player.AddGameObject(slime.Id, slime);
                    break;
                case (>= 491) and (<= 496):
                case (>= 501) and (<= 506):
                case (>= 511) and (<= 516):
                    var zone = new Zone(Game, player, id, tile);
                    player.AddGameObject(zone.Id, zone);
                    break;
            }
        }

        public void AddWinCondition()
        {
            var chests = Game.GameObjects.Values.OfType<Chest>().ToList();
            var zones = Game.GameObjects.Values.OfType<Zone>().ToList();
            var winCondition = new GatherChestsWinCondition(Game, Game.Players, zones, 10, chests.Cast<Unit>().ToList());
            Game.GameObjects.Add("WinCondition", winCondition);
        }
    }
}
