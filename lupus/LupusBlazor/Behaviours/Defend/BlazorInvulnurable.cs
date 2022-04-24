using Lupus.Units;
using LupusBlazor.Units;
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
        public BlazorInvulnurable(BlazorGame blazorGame, IJSRuntime jSRuntime, BlazorUnit unit) : base(blazorGame, jSRuntime, unit, 9999)
        {
        }

        public override void Draw()
        {
        }

        public override void Damage(int damage)
        {
        }
    }
}
