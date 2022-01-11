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
        private int x;
        public int X { 
            get {
                return x; 
            } 
            set {
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "x" }, value, this._JSInstance);
                x = value; 
            } 
        }

        private int y;
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "y" }, value, this._JSInstance);
                y = value;
            }
        }

        private float scaleX = 1;
        public float ScaleX
        {
            get
            {
                return scaleX;
            }
            set
            {
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "scale", "x" }, value, this._JSInstance);
                scaleX = value;
            }
        }

        private float scaleY;
        public float ScaleY
        {
            get
            {
                return scaleY;
            }
            set
            {
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "scale", "y" }, value, this._JSInstance);
                scaleY = value;
            }
        }

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
        public IJSObjectReference PixiApplicationModule { get; set; }

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
            PixiApplicationModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/PixiApplication.js");
            await InstantiateJSInstance();
        }

        public virtual Task InstantiateJSInstance() { return Task.CompletedTask; }


        public async Task SetVisibility(bool value)
        {
            await JavascriptHelper.SetJavascriptProperty(new string[] { "visible" }, value, this.JSInstance);
        }

        public virtual async Task Dispose()
        {
            await this._JSInstance.DisposeAsync();
            await this.JavascriptHelper?.Dispose();
            await this.PixiApplicationModule.DisposeAsync();
        }

        public async Task AddFilter(IJSObjectReference filter)
        {
            await PixiApplicationModule.InvokeVoidAsync("AddFilter", this.JSInstance, filter);
        }

        public async Task RemoveFilter(IJSObjectReference filter)
        {
            await PixiApplicationModule.InvokeVoidAsync("RemoveFilter", this.JSInstance, filter);
        }
    }
}
