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
            var pixiUnit = BlazorUnit.PixiUnit;

            if (pixiUnit != null && this.CurrentPoints == 0 && pixiUnit.UnitAnimations.TryGetValue(pixiUnit.BaseAnimation, out var baseAnimation) && baseAnimation?.Sprite != null)           
                await baseAnimation.Sprite.AddFilter(PixiFilters.Filters[PixiFilter.Desaturate]);
        }

        public override async Task StartTurn(List<Player> activePlayers)
        {
            var pixiUnit = BlazorUnit.PixiUnit;

            if (pixiUnit != null && pixiUnit.UnitAnimations.TryGetValue(pixiUnit.BaseAnimation, out var baseAnimation) && baseAnimation?.Sprite != null)
                await baseAnimation.Sprite.RemoveFilter(PixiFilters.Filters[PixiFilter.Desaturate]);
            await base.StartTurn(activePlayers);
            
        }
    }
}
