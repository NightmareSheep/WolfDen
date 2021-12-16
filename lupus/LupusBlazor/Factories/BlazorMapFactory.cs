using Lupus;
using Lupus.Factories;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Factories
{
    public class BlazorMapFactory : MapFactory
    {
        public BlazorMapFactory(BlazorGame game, IJSRuntime jSRuntime) : base(game)
        {
            this.UnitFactory = new BlazorUnitFactory(game, jSRuntime);
        }
    }
}
