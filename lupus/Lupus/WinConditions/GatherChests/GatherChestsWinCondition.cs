﻿using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.WinConditions.GatherChests
{
    public class GatherChestsWinCondition
    {
        protected Game Game { get; }
        protected List<List<Player>> Teams { get; }
        protected List<Zone> Zones { get; set; }
        protected int Turns { get; }
        protected List<Unit> Chests { get; }
        protected int CurrentTurn { get; set; }
        protected int[] Scores { get; set; }

        public GatherChestsWinCondition(Game game, List<Player> players, List<Zone> zones, int turns, List<Unit> Chests)
        {
            Game = game;
            this.Teams = players.GroupBy(p => p.Team).Select(group => group.ToList()).ToList();
            this.Zones = zones;
            Turns = turns;
            this.Chests = Chests;
            Scores = new int[Teams.Count];

            game.TurnResolver.StartTurnEvent += StartTurn;
        }

        protected virtual async Task StartTurn(List<Player> activePlayers)
        {
            CurrentTurn++;
            if (CurrentTurn == 1)
                return;

            foreach (var chest in Chests)
            {
                foreach (var zone in Zones)
                {
                    if (chest.Tile == zone.Tile)
                    {
                        var owner = zone.Owner;
                        var team = Teams.FirstOrDefault(t => t.Contains(owner));
                        var teamIndex = Teams.IndexOf(team);
                        Scores[teamIndex]++;
                    }
                }
            }

            if (CurrentTurn > Turns * Teams.Count)
                await AnnounceVictor();
        }

        protected virtual async Task AnnounceVictor()
        {
            await this.Game.EndGame();
        }
    }
}