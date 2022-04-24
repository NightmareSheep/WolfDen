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
    public class BlazorHero : BlazorUnit
    {
        private BlazorMove BlazorMove { get; set; }
        public BlazorDamageAndPush DamageAndPush { get; }
        public BlazorPushable BlazorPushable { get; }
        public SkillPoints SkillPoints { get; set; }

        public BlazorHero(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime) : base(game, owner, id, tile, jSRuntime,
            new Dictionary<string, string[]>
            {
                { "Idle",  new string[] {                   "units", "hero", "idle" } },
                { "MoveNorth",  new string[] {              "units", "hero", "moveNorth" } },
                { "MoveEast",  new string[] {               "units", "hero", "moveEast" } },
                { "MoveSouth",  new string[] {              "units", "hero", "moveSouth" } },
                { "MoveWest",  new string[] {               "units", "hero", "moveWest" } },
                { "AttackNorth",  new string[] {            "units", "hero", "attackNorth" } },
                { "AttackEast",  new string[] {             "units", "hero", "attackEast" } },
                { "AttackSouth",  new string[] {            "units", "hero", "attackSouth" } },
                { "AttackWest",  new string[] {             "units", "hero", "attackWest" } },
                { "DamagedFromNorth",  new string[] {       "units", "hero", "damagedFromNorth" } },
                { "DamagedFromEast",  new string[] {        "units", "hero", "damagedFromEast" } },
                { "DamagedFromSouth",  new string[] {       "units", "hero", "damagedFromSouth" } },
                { "DamagedFromWest",  new string[] {        "units", "hero", "damagedFromWest" } },
                { "ShortDamagedFromNorth",  new string[] {  "units", "hero", "shortDamagedFromNorth" } },
                { "ShortDamagedFromEast",  new string[] {   "units", "hero", "shortDamagedFromEast" } },
                { "ShortDamagedFromSouth",  new string[] {  "units", "hero", "shortDamagedFromSouth" } },
                { "ShortDamagedFromWest",  new string[] {   "units", "hero", "shortDamagedFromWest" } },
            })
        {
            Health = BlazorHealth = new BlazorHealth(game, jSRuntime, this, 10);
            SkillPoints = new BlazorSkillPoints(game, this, 1);
            DamageAndPush = new BlazorDamageAndPush(game, this, 1, SkillPoints, jSRuntime);
            BlazorMove = new BlazorMove(game, game.BlazorMap, this, 3, jSRuntime, SkillPoints);
            BlazorPushable = new BlazorPushable(jSRuntime, game, game.Map, this);
            Skills.Add(DamageAndPush);
            Destroyables = new List<Lupus.Other.IDestroy>() { BlazorMove, DamageAndPush, BlazorPushable, SkillPoints };
            Name = "Hero";
            Actor = Animation.Actors.Hero;
        }

        public override void Destroy()
        {
             base.Destroy();
             BlazorMove.Destroy();
        }
    }
}
