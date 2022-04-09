using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LupusBlazor.Pixi
{
    public class Container : DisplayObject
    {

        public Container(IJSRuntime jSRuntime, IJSObjectReference instance = null, JavascriptHelperModule javascriptHelper = null) : base(jSRuntime, instance, javascriptHelper)
        {
        }

        public override async Task Initialize()
        {
            await base.Initialize();                
        }

        public override async Task InstantiateJSInstance()
        {
            this.JSInstance = await JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Container" }, null);
        }

        public async Task AddChild(Container child)
        {
            await this.JSInstance.InvokeVoidAsync("addChild", child.JSInstance);
        }

        public async Task RemoveChild(Container child)
        {
            if (JSInstance != null && child?.JSInstance != null)
                await this.JSInstance.InvokeVoidAsync("removeChild", child.JSInstance);
        }

        public async Task RemoveChildren()
        {
            if (JSInstance != null)
                await this.JSInstance.InvokeVoidAsync("removeChilden");
        }


    }
}
