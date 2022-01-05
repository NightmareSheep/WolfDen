using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class Application
    {
        public Dictionary<string, IJSObjectReference> SpriteSheets { get; set; } = new Dictionary<string, IJSObjectReference>();
        private IJSRuntime JSRuntime { get; set; }
        private DotNetObjectReference<Application> ObjRef { get; set; }
        public JavascriptHelper JavascriptHelper { get; set; }
        private IJSObjectReference PixiApplicationModule { get; set; }
        public IJSObjectReference PixiApp { get; private set; }
        public Container Stage { get; private set; }

        public string ElementId { get; private set; }
        public event Func<Task> ResourcesLoadedEvent;

        [JSInvokable]
        public async void RaiseResourcesLoadedEvent()
        {
            foreach (var key in this.SpriteSheets.Keys)
            {
                this.SpriteSheets[key] = await this.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new string[] { "loader", "resources", key, "spritesheet" }, PixiApp);
            }

            if (ResourcesLoadedEvent != null)
            {
                var invocationList = ResourcesLoadedEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public Application(IJSRuntime jSRuntime)
        {
            this.JSRuntime = jSRuntime;
            this.ObjRef = DotNetObjectReference.Create(this);
        }

        public async Task<Application> Initialize(string elementId, int worldWidth, int worldHeight)
        {
            this.JavascriptHelper = await new JavascriptHelper(this.JSRuntime).Initialize();
            this.ElementId = elementId;
            this.PixiApplicationModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/PixiApplication.js");
            this.PixiApp = await PixiApplicationModule.InvokeAsync<IJSObjectReference>("InitializePixiApp", this.ObjRef, elementId, worldWidth, worldHeight);
            var JSStage = await JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new string[] { "stage" }, this.PixiApp);
            this.Stage = new Container(this.JSRuntime, JSStage, JavascriptHelper);

            return this;
        }

        public async Task AddSpriteSheet(string name, string path)
        {
            this.SpriteSheets.Add(name, null);
            await this.PixiApp.InvokeVoidAsync("loader.add", name, path);
        }

        public async Task Load()
        {
            await this.PixiApp.InvokeVoidAsync("loadResources");
        }

        public async Task Dispose()
        {
            this.ObjRef?.Dispose();
            this.PixiApplicationModule?.DisposeAsync();
            this.PixiApp?.DisposeAsync();
            await this.JavascriptHelper.Dispose();
            await Stage.Dispose();

            foreach (var key in this.SpriteSheets.Keys)
            {
                await this.SpriteSheets[key].DisposeAsync();
            }
        }
    }
}
