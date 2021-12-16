using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus;
using Lupus.Behaviours;
using Lupus.Units;

namespace LupusBlazor.Behaviours
{
    internal class BlazorSkillPoints : SkillPoints
    {
        private BlazorGame BlazorGame { get; set; }

        public BlazorSkillPoints(BlazorGame game, Unit unit, int points) : base(game, unit, points)
        {
            this.BlazorGame = game;
        }

        public override async Task UseSkillPoints(int amount, object spender)
        {
            await base.UseSkillPoints(amount, spender);

            if (this.CurrentPoints == 0)
                await PixiHelper.SetFilter(this.BlazorGame.JSRuntime, this.Unit.Id + " Idle", Pixi.PixiFilter.Desaturate, true);
        }

        public override async Task StartTurn(List<Player> activePlayers)
        {
            await PixiHelper.SetFilter(this.BlazorGame.JSRuntime, this.Unit.Id + " Idle", Pixi.PixiFilter.Desaturate, false);
            await base.StartTurn(activePlayers);
            
        }
    }
}
