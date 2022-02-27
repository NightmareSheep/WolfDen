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
        public List<IJSObjectReference> Textures { get;}
        public List<int> Times { get; }
        public event Func<Task> OnCompleteEvent;
        

        public AnimatedSprite(IJSRuntime jSRuntime, List<IJSObjectReference> textures, List<int> times, IJSObjectReference instance = null, JavascriptHelperModule javascriptHelper = null) : base(jSRuntime, null, instance, javascriptHelper)
        {
            this.Textures = textures;
            this.Times = times;
        }

        public async override Task Initialize()
        {
            await base.Initialize();            
            

            
            await this.JavascriptHelper.SetJavascriptFunctionProperty(this.ObjRef, "RaiseOnCompleteEvent", new string[] { "onComplete" }, this.JSInstance);
            await this.JavascriptHelper.SetJavascriptFunctionProperty(this.ObjRef, "RaiseOnFrameChangeEvent", new string[] { "onFrameChange" }, this.JSInstance);

        }

        public override async  Task InstantiateJSInstance()
        {
            this.JSInstance = await this.PixiApplicationModule.ConstructAnimatedSprite(Textures, Times);
        }

        [JSInvokable]
        public async Task RaiseOnCompleteEvent()
        {
            if (OnCompleteEvent != null)
            {
                var invocationList = OnCompleteEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public event Func<int, Task> OnFrameChangeEvent;

        [JSInvokable]
        public async Task RaiseOnFrameChangeEvent(int frame)
        {


            if (OnFrameChangeEvent != null && Visible)
            {

                var invocationList = OnFrameChangeEvent.GetInvocationList().Cast<Func<int, Task>>();
                foreach (var subscriber in invocationList)
                {
                    await subscriber(frame);
                }
            }
        }

        public async Task Play()
        {
            await this.JSInstance.InvokeVoidAsync("play");
        }

        public async Task GotoAndPlay(int frameNumber)
        {
            await this.JSInstance.InvokeVoidAsync("gotoAndPlay", frameNumber);
        }

        public async Task GotoAndStop(int frameNumber)
        {
            await this.JSInstance.InvokeVoidAsync("gotoAndStop", frameNumber);
        }

        public async Task Stop()
        {
            await this.JSInstance.InvokeVoidAsync("stop");
        }

        public async Task SetLoop(bool value)
        {
            this.Loop = value;
            await this.JavascriptHelper.SetJavascriptProperty(new string[] { "loop" }, value, this.JSInstance);
        }
        public override async Task Dispose()
        {
            await base.Dispose();
            
        }
    }


}
