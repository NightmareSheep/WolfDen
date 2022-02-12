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
        private float alpha;
        public float Alpha
        {
            get { return alpha; }
            set { this.JavascriptHelper.SetJavascriptProperty(new string[] { "alpha" }, value, this.JSInstance); alpha = value; }
        }

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

        private bool interactive;
        public bool Interactive
        {
            get
            {
                return interactive;
            }
            set
            {
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "interactive" }, value, this._JSInstance);
                interactive = value;
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
        protected JavascriptHelperModule JavascriptHelper { get; set; }
        public PixiApplicationModule PixiApplicationModule { get; set; }

        public async Task On<T>(string id, DotNetObjectReference<T> csObject, string functionName) where T : class
        {
            await this.PixiApplicationModule.On(this, id, csObject, functionName);
        }

        public async Task OnClick<T>(DotNetObjectReference<T> csObject, string functionName) where T : class
        {
            await this.PixiApplicationModule.SetOnClick(this, csObject, functionName);
        }

        public DisplayObject(IJSRuntime jSRuntime, IJSObjectReference instance = null, JavascriptHelperModule javascriptHelper = null)
        {
            JSRuntime = jSRuntime;
            this.JavascriptHelper = javascriptHelper;
            this.JSInstance = instance;
        }

        public virtual async Task Initialize()
        {
            JavascriptHelper = await JavascriptHelperModule.GetInstance(JSRuntime);
            PixiApplicationModule = await PixiApplicationModule.GetInstance(JSRuntime);
            await InstantiateJSInstance();
        }

        public virtual Task InstantiateJSInstance() { return Task.CompletedTask; }



        public bool Visible { get; private set; }
        public async Task SetVisibility(bool value)
        {
            Visible = value;
            await JavascriptHelper.SetJavascriptProperty(new string[] { "visible" }, value, this.JSInstance);
        }

        public virtual async Task Dispose()
        {
            await this.JSInstance.InvokeVoidAsync("destroy");
            await this._JSInstance.DisposeAsync();
        }

        public async Task AddFilter(IJSObjectReference filter)
        {
            await PixiApplicationModule.AddFilter(this, filter);
        }

        public async Task RemoveFilter(IJSObjectReference filter)
        { 
            await PixiApplicationModule.RemoveFilter(this, filter);
        }
    }
}
