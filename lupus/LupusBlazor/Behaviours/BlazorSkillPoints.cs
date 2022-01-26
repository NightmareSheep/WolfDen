using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus;
using Lupus.Behaviours;
using Lupus.Units;
using LupusBlazor.Pixi;
using LupusBlazor.Pixi.LupusPixi;
using LupusBlazor.Units;

namespace LupusBlazor.Behaviours
{
    internal class BlazorSkillPoints : SkillPoints
    {
        private BlazorGame BlazorGame { get; set; }
        public BlazorUnit BlazorUnit { get; }

        public BlazorSkillPoints(BlazorGame game, BlazorUnit unit, int points) : base(game, unit, points)
        {
            this.BlazorGame = game;
            this.BlazorUnit = unit;
        }

        public override async Task UseSkillPoints(int amount, object spender)
        {
            await base.UseSkillPoints(amount, spender);

            if (this.CurrentPoints == 0)
                await (BlazorUnit?.PixiUnit?.AnimationContainer?.AddFilter(PixiFilters.Filters[PixiFilter.Desaturate]) ?? Task.CompletedTask);
        }

        public override async Task StartTurn(List<Player> activePlayers)
        {
            await (BlazorUnit?.PixiUnit?.AnimationContainer?.RemoveFilter(PixiFilters.Filters[PixiFilter.Desaturate]) ?? Task.CompletedTask);
            await base.StartTurn(activePlayers);
            
        }
    }
}
