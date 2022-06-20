using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorJavascriptHelper
{
    public class JavascriptHelper : IDisposable
    {
        private const string _exceptionMessage = "Javascript helper module is null. Did you initialize the module? If not then call - await module.initialize.";
        private IJSInProcessObjectReference? _module;
        public static JavascriptHelper Instance { get; private set; }

        /// <summary>
        /// Initializes the modules. This method contains IO and is slow. Initialize once and reuse the module if possible.
        /// </summary>
        /// <param name="jSRuntime"></param>
        /// <returns></returns>
        public static async Task Initialize(IJSRuntime jSRuntime)
        {
            Instance = new JavascriptHelper();
            Instance._module = await jSRuntime.InvokeAsync<IJSInProcessObjectReference>("import", "./_content/BlazorJavascriptHelper/JavaScriptHelper.js");
        }

        /// <summary>
        /// Sets the property of a javascript object.
        /// </summary>
        /// <param name="propertyPath">Path to the javascript property. Example: new string[] { "Car", "Owner", "Name" }.</param>
        /// <param name="value">New value of the property.</param>
        /// <param name="obj">Object that contains the property. Global window object is used if left undefined.</param>
        /// <exception cref="Exception"></exception>
        public void SetJavascriptProperty(string[] propertyPath, object value, IJSInProcessObjectReference? obj = null)
        {
            if (_module == null)
                throw new Exception(_exceptionMessage);

            _module?.InvokeVoid("SetProperty", propertyPath, value, obj);
            
        }

        /// <summary>
        /// Sets a function property on an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dotNetObj">DotNet reference object that contains the function.</param>
        /// <param name="functionName">Name of the function on the DotNet reference object.</param>
        /// <param name="propertyPath">Path to the javascript property. Example: new string[] { "Car", "OnStartDriving" }. </param>
        /// <param name="obj">Object that contains the property. Global window object is used if left undefined.</param>
        /// <exception cref="Exception"></exception>
        public void SetJavascriptFunctionProperty<T>(DotNetObjectReference<T> dotNetObj, string functionName, string[] propertyPath, IJSInProcessObjectReference obj) where T : class
        {
            if (_module == null)
                throw new Exception(_exceptionMessage);

            _module?.InvokeVoid("SetFunctionProperty", dotNetObj, functionName, propertyPath, obj);
        }


        public T GetJavascriptProperty<T>(string property, IJSInProcessObjectReference? obj = null)
        {
            return GetJavascriptProperty<T>(new string[] { property }, obj);
        }

        /// <summary>
        /// Gets the property of a javascript object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyPath">Path to the javascript property. Example: new string[] { "Car", "Owner", "Name" }.</param>
        /// <param name="obj">Object that contains the property. Global window object is used if left undefined.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T GetJavascriptProperty<T>(string[] propertyPath, IJSInProcessObjectReference? obj = null)
        {
            if (_module == null)
                throw new Exception(_exceptionMessage);

            try
            {
                return _module.Invoke<T>("GetProperty", propertyPath, obj);
            }
            catch
            {
                return default(T);
            }            
        }

        /// <summary>
        /// Instantiate javascript class.
        /// </summary>
        /// <param name="constructorPath">Path to the constructor</param>
        /// <param name="args">Arguments inserted into the constructor</param>
        /// <returns>Instance of the class.</returns>
        /// <exception cref="Exception"></exception>
        public IJSInProcessObjectReference? InstantiateJavascriptClass(string[] constructorPath, List<object> args)
        {
            if (_module == null)
                throw new Exception(_exceptionMessage);

            return _module?.Invoke<IJSInProcessObjectReference>("InstantiateClass", constructorPath, args);
        }

        public async Task AwaitFunction(IJSObjectReference instance, string functionName)
        {
            await _module.InvokeVoidAsync("AwaitFunction", instance, functionName);
        }

        public void Dispose()
        {
            _module?.DisposeAsync();
        }
    }
}
