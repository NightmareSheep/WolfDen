using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus;
using Microsoft.JSInterop;

namespace LupusBlazor
{
    public class BlazorMap : Map
    {
        public async Task Draw(IJSRuntime jSRuntime)
        {
            await PixiHelper.CreateSprite(jSRuntime, new string[] { "Maps", this.Name }, "Map", 0, 0);
        }
    }
}
