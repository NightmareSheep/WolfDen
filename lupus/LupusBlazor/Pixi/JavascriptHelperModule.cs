using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class JavascriptHelperModule
    {
        private IJSObjectReference JavaScriptHelperModule { get; set; }
        private IJSRuntime JSRuntime { get; }

        private static JavascriptHelperModule instance;
        public static async Task<JavascriptHelperModule> GetInstance(IJSRuntime jSRuntime)
        {
            instance ??= await new JavascriptHelperModule(jSRuntime).Initialize();
            return instance;
        }

        private JavascriptHelperModule(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public async Task<JavascriptHelperModule> Initialize()
        {
            this.JavaScriptHelperModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/JavaScriptHelper.js");
            return this;
        }

        public async Task SetJavascriptProperty(string[] propertyPath, object value, IJSObjectReference obj = null)
        {
            await JavaScriptHelperModule.InvokeVoidAsync("SetProperty", propertyPath, value, obj);
        }

        public async Task SetJavascriptFunctionProperty<T>(DotNetObjectReference<T> dotNetObj, string functionName, string[] propertyPath, IJSObjectReference obj) where T : class
        {
            await JavaScriptHelperModule.InvokeVoidAsync("SetFunctionProperty", dotNetObj, functionName, propertyPath, obj);
        }

        public async Task<T> GetJavascriptProperty<T>(string[] propertyPath, IJSObjectReference obj = null) where T : class
        {
            try
            {
                return await JavaScriptHelperModule.InvokeAsync<T>("GetProperty", propertyPath, obj);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IJSObjectReference> InstantiateJavascriptClass(string[] constructorPath, List<object> args)
        {
            return await JavaScriptHelperModule.InvokeAsync<IJSObjectReference>("InstantiateClass", constructorPath, args);
        }
    }
}
