using BlazorJavascriptHelper;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIXI
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

        private IJSInProcessObjectReference _JSInstance;
        public IJSInProcessObjectReference JSInstance
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
                if (value != null && this.ObjRef != null)
                    OnClick(this.ObjRef, "RaiseClickEvent");
            }
        }
        protected JavascriptHelper JavascriptHelper { get { return JavascriptHelper.Instance; } }
        public PixiApplicationModule PixiApplicationModule { get; set; }
        protected DotNetObjectReference<DisplayObject> ObjRef { get; set; }

        public event EventHandler ClickEvent;

        [JSInvokable]
        public void RaiseClickEvent()
        {
            this.ClickEvent?.Invoke(this, EventArgs.Empty);
        }

        public void On<T>(string id, DotNetObjectReference<T> csObject, string functionName) where T : class
        {
             this.PixiApplicationModule.On(this, id, csObject, functionName);
        }

        public void OnClick<T>(DotNetObjectReference<T> csObject, string functionName) where T : class
        {
             this.PixiApplicationModule.SetOnClick(this, csObject, functionName);
        }

        public DisplayObject(IJSInProcessObjectReference instance = null)
        {
            this.JSInstance = instance;
            this.ObjRef = DotNetObjectReference.Create(this);
            PixiApplicationModule = PixiApplicationModule.Instance;
            
        }

        public bool Visible { get; private set; }
        public void SetVisibility(bool value)
        {
            Visible = value;
             JavascriptHelper.SetJavascriptProperty(new string[] { "visible" }, value, this.JSInstance);
        }

        public virtual void Dispose()
        {
             this.JSInstance.InvokeVoid("destroy");
             this._JSInstance.DisposeAsync();
            this.ObjRef.Dispose();
        }

        public void AddFilter(IJSInProcessObjectReference filter)
        {
             PixiApplicationModule.AddFilter(this, filter);
        }

        public void RemoveFilter(IJSInProcessObjectReference filter)
        { 
             PixiApplicationModule.RemoveFilter(this, filter);
        }
    }
}
