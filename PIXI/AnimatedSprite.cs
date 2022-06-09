using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class AnimatedSprite : Sprite
    {
        public bool Loop { get; private set; } = true;
        public List<IJSInProcessObjectReference> Textures { get;}
        public List<int> Times { get; }
        public event EventHandler OnCompleteEvent;
        

        public AnimatedSprite(IJSRuntime jSRuntime, List<IJSInProcessObjectReference> textures, List<int> times, IJSInProcessObjectReference instance = null, JavascriptHelperModule javascriptHelper = null, bool instantiateJSInstance = true) : base(jSRuntime, null, instance, javascriptHelper, false)
        {
            this.Textures = textures;
            this.Times = times;

            if (instance == null && instantiateJSInstance)
            {
                this.JSInstance = this.PixiApplicationModule.ConstructAnimatedSprite(Textures, Times);
                this.JavascriptHelper.SetJavascriptFunctionProperty(this.ObjRef, "RaiseOnCompleteEvent", new string[] { "onComplete" }, this.JSInstance);
                this.JavascriptHelper.SetJavascriptFunctionProperty(this.ObjRef, "RaiseOnFrameChangeEvent", new string[] { "onFrameChange" }, this.JSInstance);
            }
        }

        [JSInvokable]
        public void RaiseOnCompleteEvent()
        {
            OnCompleteEvent?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<int> OnFrameChangeEvent;

        [JSInvokable]
        public void RaiseOnFrameChangeEvent(int frame)
        {
            OnFrameChangeEvent?.Invoke(this, frame);
        }

        public void Play()
        {
             this.JSInstance.InvokeVoid("play");
        }

        public void GotoAndPlay(int frameNumber)
        {
             this.JSInstance.InvokeVoid("gotoAndPlay", frameNumber);
        }

        public void GotoAndStop(int frameNumber)
        {
             this.JSInstance.InvokeVoid("gotoAndStop", frameNumber);
        }

        public void Stop()
        {
             this.JSInstance.InvokeVoid("stop");
        }

        public void SetLoop(bool value)
        {
            this.Loop = value;
             this.JavascriptHelper.SetJavascriptProperty(new string[] { "loop" }, value, this.JSInstance);
        }
        public override void Dispose()
        {
             base.Dispose();
            
        }
    }


}
