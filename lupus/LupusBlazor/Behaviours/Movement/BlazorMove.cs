using Lupus;
using Lupus.Behaviours.Movement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Lupus.Tiles;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using LupusBlazor.Units;
using LupusBlazor.Interaction;
using Microsoft.AspNetCore.SignalR.Client;
using LupusBlazor.Animation;
using System.Numerics;
using Lupus.Other.Vector;
using Lupus.Other;
using LupusBlazor.Extensions;
using Lupus.Behaviours;

namespace LupusBlazor.Behaviours.Movement
{
    public class BlazorMove : Move
    {
        private BlazorGame Game { get; }
        private BlazorUnit BlazorUnit { get; }
        public IJSRuntime JSRuntime { get; }
        private int[] Dist { get; set; }
        private int[] Prev { get; set; }
        private readonly TileIndicators MovementIndicators;


        public BlazorMove(BlazorGame game, BlazorMap map, BlazorUnit unit, int speed, IJSRuntime jSRuntime, SkillPoints skillPoints, MovementType movementType = MovementType.Normal) : base(game, map, unit, speed, skillPoints, movementType)
        {
            BlazorUnit = unit;
            JSRuntime = jSRuntime;
            Game = game;
            unit.CurrentPlayerClickActive += ClickUnit;
            MovementIndicators = new TileIndicators(game, map, jSRuntime, System.Drawing.KnownColor.Cyan);
            MovementIndicators.TileClickEvent += ClickTile;
            Game.ClickEvent += Click;
        }

        public void Click(object sender, EventArgs e)
        {
            if (sender == this.Unit)
                return;

             MovementIndicators.RemoveIndicators();
        }

        public void ClickUnit(object sender, EventArgs e)
        {
            if (this.MovementIndicators.Indicators.Count > 0)
                 MovementIndicators.RemoveIndicators();
            else
                 this.SpawnMovementIndicators();
        }

        public void SpawnMovementIndicators()
        {

            if (!CanMove)
                return;

            Dijkstra.Dijkstra.DijkstraAlgorithm(out int[] dist, out int[] prev, Map.Tiles.Cast<Tile>().ToArray(), Unit.Tile, DistanceFunction);
            var indices = new List<int>();
            for (int i = 0; i < dist.Length; i++)
                if (dist[i] > 0 && dist[i] <= Speed)
                    indices.Add(i);
            for (var i = indices.Count - 1; i >= 0; i--)
            {
                var index = indices[i];
                var tile = Map.GetTile(index);
                if (tile.Unit != null)
                    indices.Remove(index);
            }
             MovementIndicators.Spawn(indices.ToArray());
            Prev = prev;
            Dist = dist;
        }

        /// <summary>
        /// Move to tile with input index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public void ClickTile(object sender, int index)
        {
             MovementIndicators.RemoveIndicators();

            var path = new List<int>();
            var currentIndex = index;

            // Start at destination and retrieve previous until entire path is found, then reverse.
            while (true)
            {
                path.Add(currentIndex);
                currentIndex = Prev[currentIndex];
                if (currentIndex == -1)
                    break;
            }
            path.Reverse();
            // Game.Hub.InvokeAsync("Move", Game.Id, Id, path);

             Game.Hub.InvokeAsync("DoMove", Game.Id, this.Unit.Owner.Id, Id, typeof(Move).AssemblyQualifiedName, "MoveOverPath", new object[] { path }, new string[] { typeof(int[]).AssemblyQualifiedName });

            Dist = null;
            Prev = null;
        }

        public override void MoveOverPath(int[] path)
        {
            var previousTile = Unit.Tile;
            for (var i = 1; i < path.Length; i++)
            {
                var index = path[i];
                var tile = Map.GetTile(index);
                var directionVector = tile.ToVector2() - previousTile.ToVector2();
                var direction = directionVector.GetDirectionFromVector();

                BlazorUnit?.PixiUnit?.QueueAnimation(Animations.Move, direction);
                previousTile = tile;
            }

             base.MoveOverPath(path);            
        }

        public override void Destroy()
        {
             base.Destroy();
             MovementIndicators.Destroy();
        }

        
    }
}
