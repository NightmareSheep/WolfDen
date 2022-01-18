using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LupusBlazor.Pixi;

namespace LupusBlazor.Pixi
{
    public class Application
    {
        public Dictionary<string, IJSObjectReference> SpriteSheets { get; set; } = new Dictionary<string, IJSObjectReference>();
        private IJSRuntime JSRuntime { get; set; }
        private DotNetObjectReference<Application> ObjRef { get; set; }
        public JavascriptHelperModule JavascriptHelper { get; set; }
        public IJSObjectReference PixiApp { get; private set; }
        public Container Stage { get; private set; }

        public string ElementId { get; private set; }
        public event Func<Task> ResourcesLoadedEvent;
        public event Func<Task> TickEvent;

        private ScaleMode scaleMode;
        public ScaleMode ScaleMode { 
            get { return scaleMode; } 
            set { 
                scaleMode = value;
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "renderer", "options", "scaleMode" }, value, this.PixiApp);
            } 
        }

        public Application(IJSRuntime jSRuntime)
        {
            this.JSRuntime = jSRuntime;
            this.ObjRef = DotNetObjectReference.Create(this);
        }

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

        [JSInvokable]
        public async Task Tick()
        {
            if (TickEvent != null)
            {
                var invocationList = TickEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        

        public async Task<Application> Initialize(string elementId)
        {
            this.JavascriptHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            this.ElementId = elementId;

            var pixiModule = await PixiApplicationModule.GetInstance(JSRuntime);
            this.PixiApp = await pixiModule.InitializePixiApp(this.ObjRef, elementId);

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
            this.PixiApp?.DisposeAsync();
            await Stage.Dispose();

            foreach (var key in this.SpriteSheets.Keys)
            {
                await this.SpriteSheets[key].DisposeAsync();
            }
        }
    }
}
