using Lupus.Units;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Behaviours.Defend
{
    public class BlazorInvulnurable : BlazorHealth
    {
        public BlazorInvulnurable(BlazorGame blazorGame, IJSRuntime jSRuntime, Unit unit) : base(blazorGame, jSRuntime, unit, 9999)
        {
        }

        public override Task Draw()
        {
            return Task.CompletedTask;
        }

        public override Task Damage(int damage)
        {
            return Task.CompletedTask;
        }
    }
}
