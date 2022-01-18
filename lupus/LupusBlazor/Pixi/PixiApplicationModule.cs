using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class PixiApplicationModule
    {
        private IJSObjectReference module;
        private IJSRuntime JSRuntime { get; }

        private static PixiApplicationModule instance;
        public static async Task<PixiApplicationModule> GetInstance(IJSRuntime jSRuntime)
        {
            instance ??= await new PixiApplicationModule(jSRuntime).Initialize();
            return instance;
        }

        private PixiApplicationModule(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public async Task<PixiApplicationModule> Initialize()
        {
            this.module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/PixiApplication.js");
            return this;
        }

        public async Task<IJSObjectReference> InitializePixiApp(DotNetObjectReference<Application> application, string elementId)
        {
            return await module.InvokeAsync<IJSObjectReference>("InitializePixiApp", application, elementId);
        }

        public async Task LoadResources()
        {
            await module.InvokeVoidAsync("LoadResources");
        }

        public async Task<IJSObjectReference> ConstructAnimatedSprite(List<IJSObjectReference> textures, List<int> times)
        {
            return await module.InvokeAsync<IJSObjectReference>("ConstructAnimatedSprite", textures, times);
        }

        public async Task AddFilter(DisplayObject obj, IJSObjectReference filter)
        {
            await module.InvokeVoidAsync("AddFilter", obj.JSInstance, filter);
        }

        public async Task RemoveFilter(DisplayObject obj, IJSObjectReference filter)
        {
            await module.InvokeVoidAsync("RemoveFilter", obj.JSInstance, filter);
        }

        public async Task On<T>(DisplayObject obj, string eventName, DotNetObjectReference<T> csObject, string functionName) where T : class
        {
            await module.InvokeVoidAsync("On", obj.JSInstance, eventName, csObject, functionName);
        }

        public async Task SetOnClick<T>(DisplayObject obj, DotNetObjectReference<T> csObject, string functionName) where T : class
        {
            await module.InvokeVoidAsync("SetOnClick", obj.JSInstance, csObject, functionName);
        }
    }
}
