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
    public class BlazorSlime : BlazorUnit
    {
        private BlazorMove BlazorMove { get; set; }
        public BlazorDamageAndPull DamageAndPull { get; }
        public BlazorPushable BlazorPushable { get; }
        public SkillPoints SkillPoints { get; set; }

        public BlazorSlime(BlazorGame game, Player owner, string id, Tile tile, IJSRuntime jSRuntime) : base(game, owner, id, tile, jSRuntime,
            new Dictionary<string, string[]>
            {
                { "Idle",  new string[] {                   "units",    "slime",    "idle" } },
                { "MoveNorth",  new string[] {              "units",    "slime",    "moveNorth" } },
                { "MoveEast",  new string[] {               "units",    "slime",    "moveEast" } },
                { "MoveSouth",  new string[] {              "units",    "slime",    "moveSouth" } },
                { "MoveWest",  new string[] {               "units",    "slime",    "moveWest" } },
                { "AttacNorth",  new string[] {            "units",    "slime",    "attackNorth" } },
                { "AttackEast",  new string[] {             "units",    "slime",    "attackEast" } },
                { "AttackSouth",  new string[] {            "units",    "slime",    "attackSouth" } },
                { "AttackWest",  new string[] {             "units",    "slime",    "attackWest" } },
                { "DamagedFromNorth",  new string[] {       "units",    "slime",    "damagedFromNorth" } },
                { "DamagedFromEast",  new string[] {        "units",    "slime",    "damagedFromEast" } },
                { "DamagedFromSouth",  new string[] {       "units",    "slime",    "damagedFromSouth" } },
                { "DamagedFromWest",  new string[] {        "units",    "slime",    "damagedFromWest" } },
                { "ShortDamagedFromNorth",  new string[] {  "units",    "slime",    "shortDamagedFromNorth" } },
                { "ShortDamagedFromEast",  new string[] {   "units",    "slime",    "shortDamagedFromEast" } },
                { "ShortDamagedFromSouth",  new string[] {  "units",    "slime",    "shortDamagedFromSouth" } },
                { "ShortDamagedFromWest",  new string[] {   "units",    "slime",    "shortDamagedFromWest" } },
                { "PullNorth",  new string[] {              "units",    "slime",    "pullNorth" } },
                { "PullEast",  new string[] {               "units",    "slime",    "pullEast" } },
                { "PullSouth",  new string[] {              "units",    "slime",    "pullSouth" } },
                { "PullWest",  new string[] {               "units",    "slime",    "pullWest" } },
            })
        {
            Health = BlazorHealth = new BlazorHealth(game, jSRuntime, this, 10);
            SkillPoints = new BlazorSkillPoints(game, this, 1);
            DamageAndPull = new BlazorDamageAndPull(game, this, 1, SkillPoints, jSRuntime);
            BlazorMove = new BlazorMove(game, game.BlazorMap, this, 3, jSRuntime, SkillPoints);
            BlazorPushable = new BlazorPushable(jSRuntime, game, game.Map, this);
            Skills.Add(DamageAndPull);
            Destroyables = new List<Lupus.Other.IDestroy>() { BlazorMove, DamageAndPull, BlazorPushable, SkillPoints };
            Name = "Slime";
        }

        public async override Task Destroy()
        {
            await base.Destroy();
            await BlazorMove.Destroy();
        }
    }
}
