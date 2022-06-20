using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIXI;
using BlazorJavascriptHelper;

namespace PIXI
{
    public class Application
    {
        public Dictionary<string, IJSObjectReference> SpriteSheets { get; set; } = new Dictionary<string, IJSObjectReference>();
        private DotNetObjectReference<Application> ObjRef { get; set; }
        public JavascriptHelper JavascriptHelper { get; set; }
        public IJSInProcessObjectReference PixiApp { get; private set; }
        public Container Stage { get; private set; }

        public string ElementId { get; private set; }
        public event EventHandler ResourcesLoadedEvent;
        public event EventHandler TickEvent;

        private ScaleMode scaleMode;
        public ScaleMode ScaleMode { 
            get { return scaleMode; } 
            set { 
                scaleMode = value;
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "renderer", "options", "scaleMode" }, value, this.PixiApp);
            } 
        }

        public Application(IJSRuntime jSRuntime, string elementId)
        {
            this.ObjRef = DotNetObjectReference.Create(this);

            this.JavascriptHelper = JavascriptHelper.Instance;
            this.ElementId = elementId;

            var pixiModule = PixiApplicationModule.Instance;
            this.PixiApp = pixiModule.InitializePixiApp(this.ObjRef, elementId);

            var JSStage = JavascriptHelper.GetJavascriptProperty<IJSInProcessObjectReference>(new string[] { "stage" }, this.PixiApp);
            this.Stage = new Container(JSStage, false);
        }

        [JSInvokable]
        public void RaiseResourcesLoadedEvent()
        {
            foreach (var key in this.SpriteSheets.Keys)
            {
                this.SpriteSheets[key] =  this.JavascriptHelper.GetJavascriptProperty<IJSObjectReference>(new string[] { "loader", "resources", key, "spritesheet" }, PixiApp);
            }
            
            ResourcesLoadedEvent?.Invoke(this, EventArgs.Empty);
        }

        [JSInvokable]
        public void Tick()
        {
            TickEvent?.Invoke(this, EventArgs.Empty);
        }

        public void AddSpriteSheet(string name, string path)
        {
            this.SpriteSheets.Add(name, null);
             this.PixiApp.InvokeVoid("loader.add", name, path);
        }

        public void Load()
        {
             this.PixiApp.InvokeVoidAsync("loadResources");
        }

        public void Dispose()
        {
            this.ObjRef?.Dispose();
            this.PixiApp?.DisposeAsync();
             Stage.Dispose();

            foreach (var key in this.SpriteSheets.Keys)
            {
                 this.SpriteSheets[key].DisposeAsync();
            }
        }
    }
}
