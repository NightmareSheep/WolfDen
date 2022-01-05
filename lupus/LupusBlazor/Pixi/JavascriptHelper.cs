using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class JavascriptHelper
    {
        private IJSObjectReference JavaScriptHelperModule { get; set; }
        private IJSRuntime JSRuntime { get; }

        public JavascriptHelper(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public async Task<JavascriptHelper> Initialize()
        {
            this.JavaScriptHelperModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/JavaScriptHelper.js");
            return this;
        }

        public async Task SetJavascriptProperty(string[] propertyPath, object value, IJSObjectReference obj)
        {
            await JavaScriptHelperModule.InvokeVoidAsync("SetProperty", propertyPath, value, obj);
        }

        public async Task SetJavascriptFunctionProperty<T>(DotNetObjectReference<T> dotNetObj, string functionName, string[] propertyPath, IJSObjectReference obj) where T : class
        {
            await JavaScriptHelperModule.InvokeVoidAsync("SetFunctionProperty", dotNetObj, functionName, propertyPath, obj);
        }

        public async Task<T> GetJavascriptProperty<T>(string[] propertyPath, IJSObjectReference obj = null)
        {
            return await JavaScriptHelperModule.InvokeAsync<T>("GetProperty", propertyPath, obj);
        }

        public async Task<IJSObjectReference> InstantiateJavascriptClass(string[] constructorPath, List<object> args)
        {
            return await JavaScriptHelperModule.InvokeAsync<IJSObjectReference>("InstantiateClass", constructorPath, args);
        }

        public async Task Dispose()
        {
            await this.JavaScriptHelperModule.DisposeAsync();
        }
    }
}
