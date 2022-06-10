using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIXI
{
    public class JavascriptHelperModule
    {
        private IJSInProcessObjectReference JavaScriptHelperModule { get; set; }
        private IJSRuntime JSRuntime { get; }

        public static JavascriptHelperModule Instance { get; private set; }

        public static async Task<JavascriptHelperModule> Initialize(IJSRuntime jSRuntime)
        {
            Instance = new JavascriptHelperModule();
            Instance.JavaScriptHelperModule = (IJSInProcessObjectReference) (await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/modules/JavaScriptHelper.js"));
            return Instance;
        }

        public void SetJavascriptProperty(string[] propertyPath, object value, IJSObjectReference obj = null)
        {
             JavaScriptHelperModule.InvokeVoid("SetProperty", propertyPath, value, obj);            
        }

        public void SetJavascriptFunctionProperty<T>(DotNetObjectReference<T> dotNetObj, string functionName, string[] propertyPath, IJSObjectReference obj) where T : class
        {
             JavaScriptHelperModule.InvokeVoid("SetFunctionProperty", dotNetObj, functionName, propertyPath, obj);
        }

        public T GetJavascriptProperty<T>(string[] propertyPath, IJSObjectReference obj = null) where T : class
        {
            return  JavaScriptHelperModule.Invoke<T>("GetProperty", propertyPath, obj);            
        }

        public IJSInProcessObjectReference InstantiateJavascriptClass(string[] constructorPath, List<object> args)
        {
            return JavaScriptHelperModule.Invoke<IJSInProcessObjectReference>("InstantiateClass", constructorPath, args);
        }
    }
}
