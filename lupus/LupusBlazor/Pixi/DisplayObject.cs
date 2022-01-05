using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class DisplayObject
    {
        private IJSObjectReference _JSInstance;
        public IJSObjectReference JSInstance
        {
            get
            {
                return _JSInstance;
            }
            protected set
            {
                if (this._JSInstance != null)
                    this._JSInstance.DisposeAsync();
                _JSInstance = value;
            }
        }
        public IJSRuntime JSRuntime { get; }
        protected JavascriptHelper JavascriptHelper { get; set; }

        public DisplayObject(IJSRuntime jSRuntime, IJSObjectReference instance = null, JavascriptHelper javascriptHelper = null)
        {
            JSRuntime = jSRuntime;
            this.JavascriptHelper = javascriptHelper;
            this.JSInstance = instance;
        }

        public virtual async Task Initialize()
        {
            if (this.JavascriptHelper == null)
                this.JavascriptHelper = await new JavascriptHelper(this.JSRuntime).Initialize();
        }


        public async Task SetVisibility(bool value)
        {
            await JavascriptHelper.SetJavascriptProperty(new string[] { "visible" }, value, this.JSInstance);
        }

        public virtual async Task Dispose()
        {
            await this._JSInstance.DisposeAsync();
            await this.JavascriptHelper?.Dispose();
        }
    }
}
