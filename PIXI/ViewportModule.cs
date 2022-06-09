using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class ViewportModule
    {
        private IJSInProcessObjectReference _module;

        public static ViewportModule Instance { get; private set; }

        public static async Task Initialize(IJSRuntime jSRuntime)
        {
            Instance = new ViewportModule();
            Instance._module = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./js/modules/PixiViewport.js");
        }

        public IJSInProcessObjectReference InstantiateViewport(IJSInProcessObjectReference pixiApp, string elementId, int worldWidth, int worldHeight)
        {
            return _module.Invoke<IJSInProcessObjectReference>("InstantiateViewport", pixiApp, elementId, worldWidth, worldHeight);
        }
    }
}
