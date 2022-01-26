using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lupus;
using Lupus.Factories;
using Lupus.Tiles;
using Lupus.Units;
using Lupus.Units.Orcs;
using Lupus.WinConditions.GatherChests;
using LupusBlazor.Units;
using LupusBlazor.Units.Orcs;
using LupusBlazor.WinConditions.GatherChests;
using Microsoft.JSInterop;

namespace LupusBlazor.Factories
{
    public class BlazorUnitFactory : IUnitFactory
    {
        public BlazorGame Game { get; }
        public IJSRuntime JSRuntime { get; }

        public BlazorUnitFactory(BlazorGame game, IJSRuntime jSRuntime)
        {
            Game = game;
            JSRuntime = jSRuntime;
        }        

        public void AddGrunt(Player player, string id, Tile tile)
        {
            var grunt = new BlazorGrunt(Game, player, id, tile, JSRuntime);
            Game.GameObjects.Add(grunt.Id, grunt);
            player.GameObjects.Add(grunt.Id, grunt);
        }

        public void AddUnit(Player player, uint typeId, string id, Tile tile)
        {
            switch (typeId)
            {
                case 335:
                    var chest = new BlazorChest(Game, player, id, tile, JSRuntime);
                    Game.GameObjects.Add(chest.Id, chest);
                    break;
                case 482:
                    var hero = new BlazorHero(Game, player, id, tile, this.JSRuntime);
                    Game.GameObjects.Add(hero.Id, hero);
                    player.GameObjects.Add(hero.Id, hero);
                    break;
                case 483:
                    var grunt = new BlazorGrunt(Game, player, id, tile, this.JSRuntime);
                    Game.GameObjects.Add(grunt.Id, grunt);
                    player.GameObjects.Add(grunt.Id, grunt);
                    break;
                case 484:
                    var goblin = new BlazorGoblin(Game, player, id, tile, this.JSRuntime);
                    Game.GameObjects.Add(goblin.Id, goblin);
                    player.GameObjects.Add(goblin.Id, goblin);
                    break;
                case 485:
                    var slime = new BlazorSlime(Game, player, id, tile, this.JSRuntime);
                    Game.GameObjects.Add(slime.Id, slime);
                    player.GameObjects.Add(slime.Id, slime);
                    break;
                case (>= 491) and (<= 496):
                case (>= 501) and (<= 506):
                case (>= 511) and (<= 516):
                    var zone = new BlazorZone(Game, player, id, tile, JSRuntime, (int)typeId - 491);
                    Game.GameObjects.Add(zone.Id, zone);
                    player.GameObjects.Add(zone.Id, zone);
                    break;
            }
        }

        public void AddWinCondition()
        {
            var chests = Game.GameObjects.Values.OfType<BlazorChest>().ToList();
            var zones = Game.GameObjects.Values.OfType<Zone>().ToList();
            var winCondition = new BlazorGatherChestsWinCondition(Game, Game.Players, zones, 10, chests.Cast<Unit>().ToList());
            Game.GameObjects.Add("WinCondition", winCondition);
        }
    }
}
