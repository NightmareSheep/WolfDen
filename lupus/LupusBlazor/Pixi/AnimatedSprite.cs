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
        private IJSObjectReference PixiModule { get; set; }
        public List<IJSObjectReference> Textures { get;}
        public List<int> Times { get; }
        public event Func<Task> OnCompleteEvent;
        private DotNetObjectReference<AnimatedSprite> ObjRef { get; set; }

        public AnimatedSprite(Application application, IJSRuntime jSRuntime, List<IJSObjectReference> textures, List<int> times, IJSObjectReference instance = null, JavascriptHelper javascriptHelper = null) : base(application, jSRuntime, null, instance, javascriptHelper)
        {
            this.Textures = textures;
            this.Times = times;
        }

        public async override Task Initialize()
        {
            if (this.JavascriptHelper == null)
                this.JavascriptHelper = await new JavascriptHelper(this.JSRuntime).Initialize();            

            this.PixiModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/PixiApplication.js");
            this.JSInstance = await this.PixiModule.InvokeAsync<IJSObjectReference>("ConstructAnimatedSprite", this.Textures, this.Times);

            this.ObjRef = DotNetObjectReference.Create(this);
            await this.JavascriptHelper.SetJavascriptFunctionProperty(this.ObjRef, "RaiseOnCompleteEvent", new string[] { "onComplete" }, this.JSInstance);

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

        public event Func<Task<int>> OnFrameChangeEvent;

        [JSInvokable]
        public async Task RaiseOnFrameChangeEvent(int frame)
        {
            if (OnCompleteEvent != null)
            {
                var invocationList = OnCompleteEvent.GetInvocationList().Cast<Func<int, Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber(frame);
            }
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
            await this.JavascriptHelper.SetJavascriptProperty(new string[] { "loop" }, value, this.JSInstance);
        }
        public override async Task Dispose()
        {
            await base.Dispose();
            this.ObjRef.Dispose();
        }
    }


}
