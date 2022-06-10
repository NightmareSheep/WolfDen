using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus;
using Lupus.Behaviours;
using Lupus.Units;
using PIXI;
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

        public override void UseSkillPoints(int amount, object spender)
        {
             base.UseSkillPoints(amount, spender);
            var pixiUnit = BlazorUnit.PixiUnit;
            if (pixiUnit != null)
                 pixiUnit.AnimationContainer.AddFilter(PixiFilters.Filters[PixiFilter.Desaturate]);
        }

        public override void StartTurn(object sender, List<Player> activePlayers)
        {
            var pixiUnit = BlazorUnit.PixiUnit;
            if (pixiUnit != null)
                 pixiUnit?.AnimationContainer?.RemoveFilter(PixiFilters.Filters[PixiFilter.Desaturate]);
             base.StartTurn(sender, activePlayers);
            
        }
    }
}
