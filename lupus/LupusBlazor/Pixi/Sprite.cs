using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class Sprite : Container
    {
        public Application Application { get; }
        public IJSObjectReference Texture { get; }

        public Sprite(Application application, IJSRuntime jSRuntime, IJSObjectReference texture, IJSObjectReference instance = null, JavascriptHelper javascriptHelper = null) : base(jSRuntime, instance, javascriptHelper)
        {
            Application = application;
            this.Texture = texture;
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            if (this.JSInstance != null)
                this.JSInstance?.DisposeAsync();

            
            this.JSInstance = await this.JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Sprite" }, new() { this.Texture });
        }

        
    }
}
