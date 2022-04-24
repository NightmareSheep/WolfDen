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
        private IJSInProcessObjectReference module;
        private IJSRuntime JSRuntime { get; }

        public static PixiApplicationModule Instance { get; private set; }

        public static async Task<PixiApplicationModule> Initialize(IJSRuntime jsRuntime)
        {
            Instance = new PixiApplicationModule();
            Instance.module = await jsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./js/modules/PixiApplication.js");
            return Instance;
        }

        public IJSInProcessObjectReference InitializePixiApp(DotNetObjectReference<Application> application, string elementId)
        {
            return module.Invoke<IJSInProcessObjectReference>("InitializePixiApp", application, elementId);
        }

        public async Task LoadResources()
        {
             await module.InvokeVoidAsync("LoadResources");
        }

        public IJSInProcessObjectReference ConstructAnimatedSprite(List<IJSInProcessObjectReference> textures, List<int> times)
        {
            var texturesCast = textures.Cast<IJSObjectReference>().ToList();
            return module.Invoke<IJSInProcessObjectReference>("ConstructAnimatedSprite", texturesCast, times);
        }

        public void AddFilter(DisplayObject obj, IJSInProcessObjectReference filter)
        {
             module.InvokeVoid("AddFilter", obj.JSInstance, filter);
        }

        public void RemoveFilter(DisplayObject obj, IJSInProcessObjectReference filter)
        {
             module.InvokeVoid("RemoveFilter", obj.JSInstance, filter);
        }

        public void On<T>(DisplayObject obj, string eventName, DotNetObjectReference<T> csObject, string functionName) where T : class
        {
             module.InvokeVoid("On", obj.JSInstance, eventName, csObject, functionName);
        }

        public void SetOnClick<T>(DisplayObject obj, DotNetObjectReference<T> csObject, string functionName) where T : class
        {
             module.InvokeVoid("SetOnClick", obj.JSInstance, csObject, functionName);
        }
    }
}
