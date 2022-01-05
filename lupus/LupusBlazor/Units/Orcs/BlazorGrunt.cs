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

namespace LupusBlazor.Units.Orcs
{
    public class BlazorGrunt : BlazorUnit
    {
        private BlazorMove BlazorMove { get; set; }
        public BlazorDamageAndPush DamageAndPush { get; }
        public BlazorPushable BlazorPushable { get; }
        public SkillPoints SkillPoints { get; set; }

        public BlazorGrunt(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime) : base(game, owner, id, tile, jSRuntime, 
            new Dictionary<string, string[]>
            {
                { "Idle",  new string[] {                   "units", "grunt", "idle" } },
                { "MoveNorth",  new string[] {              "units", "grunt", "moveNorth" } },
                { "MoveEast",  new string[] {               "units", "grunt", "moveEast" } },
                { "MoveSouth",  new string[] {              "units", "grunt", "moveSouth" } },
                { "MoveWest",  new string[] {               "units", "grunt", "moveWest" } },
                { "AttackNorth",  new string[] {            "units", "grunt", "attackNorth" } },
                { "AttackEast",  new string[] {             "units", "grunt", "attackEast" } },
                { "AttackSouth",  new string[] {            "units", "grunt", "attackSouth" } },
                { "AttackWest",  new string[] {             "units", "grunt", "attackWest" } },
                { "DamagedFromNorth",  new string[] {       "units", "grunt", "damagedFromNorth" } },
                { "DamagedFromEast",  new string[] {        "units", "grunt", "damagedFromEast" } },
                { "DamagedFromSouth",  new string[] {       "units", "grunt", "damagedFromSouth" } },
                { "DamagedFromWest",  new string[] {        "units", "grunt", "damagedFromWest" } },
                { "ShortDamagedFromNorth",  new string[] {  "units", "grunt", "shortDamagedFromNorth" } },
                { "ShortDamagedFromEast",  new string[] {   "units", "grunt", "shortDamagedFromEast" } },
                { "ShortDamagedFromSouth",  new string[] {  "units", "grunt", "shortDamagedFromSouth" } },
                { "ShortDamagedFromWest",  new string[] {   "units", "grunt", "shortDamagedFromWest" } },
            })
        {
            Health = BlazorHealth = new BlazorHealth(game, jSRuntime, this, 25);
            SkillPoints = new BlazorSkillPoints(game, this, 1);
            DamageAndPush = new BlazorDamageAndPush(game, this, 1, SkillPoints, jSRuntime);
            BlazorMove = new BlazorMove(game, game.BlazorMap, this, 3, jSRuntime, SkillPoints);
            BlazorPushable = new BlazorPushable(jSRuntime, game, game.Map, this);
            Skills.Add(DamageAndPush);
            Destroyables = new List<Lupus.Other.IDestroy>() { BlazorMove, DamageAndPush, BlazorPushable, SkillPoints };
            Name = "Grunt";
            Actor = Animation.Actors.Grunt;
        }

        public async override Task Destroy()
        {
            await base.Destroy();
            await BlazorMove.Destroy();
        }
    }
}
