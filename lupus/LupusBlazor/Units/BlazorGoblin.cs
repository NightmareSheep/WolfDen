using Lupus;
using Lupus.Behaviours;
using Lupus.Behaviours.Attack;
using Lupus.Behaviours.Defend;
using Lupus.Behaviours.Movement;
using Lupus.Tiles;
using LupusBlazor.Behaviours;
using LupusBlazor.Behaviours.Attack;
using LupusBlazor.Behaviours.Defend;
using LupusBlazor.Behaviours.Displacement;
using LupusBlazor.Behaviours.Movement;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Units
{
    public class BlazorGoblin : BlazorUnit
    {
        private BlazorMove BlazorMove { get; set; }
        public BlazorDamageAndPush DamageAndPush { get; }
        public BlazorPushable BlazorPushable { get; }
        public SkillPoints SkillPoints { get; set; }

        public BlazorGoblin(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime) : base(game, owner, id, tile, jSRuntime,
            new Dictionary<string, string[]>
            {
                { "Idle",  new string[] {                   "units", "goblin", "idle" } },
                { "MoveNorth",  new string[] {              "units", "goblin", "moveNorth" } },
                { "MoveEast",  new string[] {               "units", "goblin", "moveEast" } },
                { "MoveSouth",  new string[] {              "units", "goblin", "moveSouth" } },
                { "MoveWest",  new string[] {               "units", "goblin", "moveWest" } },
                { "AttackNorth",  new string[] {            "units", "goblin", "attackNorth" } },
                { "AttackEast",  new string[] {             "units", "goblin", "attackEast" } },
                { "AttackSouth",  new string[] {            "units", "goblin", "attackSouth" } },
                { "AttackWest",  new string[] {             "units", "goblin", "attackWest" } },
                { "DamagedFromNorth",  new string[] {       "units", "goblin", "damagedFromNorth" } },
                { "DamagedFromEast",  new string[] {        "units", "goblin", "damagedFromEast" } },
                { "DamagedFromSouth",  new string[] {       "units", "goblin", "damagedFromSouth" } },
                { "DamagedFromWest",  new string[] {        "units", "goblin", "damagedFromWest" } },
                { "ShortDamagedFromNorth",  new string[] {  "units", "goblin", "shortDamagedFromNorth" } },
                { "ShortDamagedFromEast",  new string[] {   "units", "goblin", "shortDamagedFromEast" } },
                { "ShortDamagedFromSouth",  new string[] {  "units", "goblin", "shortDamagedFromSouth" } },
                { "ShortDamagedFromWest",  new string[] {   "units", "goblin", "shortDamagedFromWest" } },
            })
        {
            Health = BlazorHealth = new BlazorHealth(game, jSRuntime, this, 10);
            SkillPoints = new BlazorSkillPoints(game, this, 1);
            DamageAndPush = new BlazorDamageAndPush(game, this, 1, SkillPoints, jSRuntime);
            BlazorMove = new BlazorMove(game, game.BlazorMap, this, 5, jSRuntime, SkillPoints);
            BlazorPushable = new BlazorPushable(jSRuntime, game, game.Map, this);
            Skills.Add(DamageAndPush);
            Destroyables = new List<Lupus.Other.IDestroy>() { BlazorMove, DamageAndPush, BlazorPushable, SkillPoints };
            Name = "Goblin";
            Actor = Animation.Actors.Goblin;
        }

        public async override Task Destroy()
        {
            await base.Destroy();
            await BlazorMove.Destroy();
        }
    }
}
