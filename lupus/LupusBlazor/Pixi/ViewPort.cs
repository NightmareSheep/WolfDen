using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LupusBlazor.Pixi
{
    public class ViewPort : Container
    {
        public Application Application { get; }
        public int WorldWidth { get; }
        public int WorldHeight { get; }
        private IJSObjectReference ViewportModule { get; set; }

        public ViewPort(IJSRuntime jSRuntime, Application application, int worldWidth, int worldHeight, JavascriptHelper javascriptHelper = null) : base(jSRuntime, null, javascriptHelper)
        {
            Application = application;
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            this.JavascriptHelper = javascriptHelper;
        }

        public override async Task<ViewPort> Initialize()
        {
            await base.Initialize();
            this.ViewportModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/PixiViewport.js");

            if (this.JSInstance != null)
                await this.JSInstance.DisposeAsync();

            this.JSInstance = await ViewportModule.InvokeAsync<IJSObjectReference>("InstantiateViewport", this.Application.PixiApp, this.Application.ElementId, WorldWidth, WorldHeight);

            await this.JSInstance.InvokeVoidAsync("drag");
            await this.JSInstance.InvokeVoidAsync("pinch");
            await this.JSInstance.InvokeVoidAsync("wheel");
            return this;

        }

        public override async Task Dispose()
        {
            await base.Dispose();
            await this.ViewportModule.DisposeAsync();
        }

    }
}
