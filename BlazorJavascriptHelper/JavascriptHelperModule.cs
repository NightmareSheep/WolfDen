using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavascriptHelperModule
{
    public class BlazorJavascriptHelperModule : IDisposable
    {
        private const string _exceptionMessage = "Javascript helper module is null. Did you initialize the module? If not then call - await module.initialize.";
        private IJSInProcessObjectReference? JavaScriptHelperModule { get; set; }

        /// <summary>
        /// Initializes the modules. This method contains IO and is slow. Initialize once and reuse the module if possible.
        /// </summary>
        /// <param name="jSRuntime"></param>
        /// <returns></returns>
        public async Task Initialize(IJSRuntime jSRuntime)
        {
            JavaScriptHelperModule = (IJSInProcessObjectReference) (await jSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorJavascriptHelper/JavaScriptHelper.js"));
        }

        /// <summary>
        /// Sets the property of a javascript object.
        /// </summary>
        /// <param name="propertyPath">Path to the javascript property. Example: new string[] { "Car", "Owner", "Name" }.</param>
        /// <param name="value">New value of the property.</param>
        /// <param name="obj">Object that contains the property. Global window object is used if left undefined.</param>
        /// <exception cref="Exception"></exception>
        public void SetJavascriptProperty(string[] propertyPath, object value, IJSObjectReference? obj = null)
        {
            if (JavaScriptHelperModule == null)
                throw new Exception(_exceptionMessage);

             JavaScriptHelperModule?.InvokeVoid("SetProperty", propertyPath, value, obj);            
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
        public void SetJavascriptFunctionProperty<T>(DotNetObjectReference<T> dotNetObj, string functionName, string[] propertyPath, IJSObjectReference obj) where T : class
        {
            if (JavaScriptHelperModule == null)
                throw new Exception(_exceptionMessage);

            JavaScriptHelperModule?.InvokeVoid("SetFunctionProperty", dotNetObj, functionName, propertyPath, obj);
        }

        /// <summary>
        /// Gets the property of a javascript object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyPath">Path to the javascript property. Example: new string[] { "Car", "Owner", "Name" }.</param>
        /// <param name="obj">Object that contains the property. Global window object is used if left undefined.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T? GetJavascriptProperty<T>(string[] propertyPath, IJSObjectReference? obj = null) where T : class
        {
            if (JavaScriptHelperModule == null)
                throw new Exception(_exceptionMessage);

            try
            {
                return JavaScriptHelperModule?.Invoke<T>("GetProperty", propertyPath, obj);
            }
            catch
            {
                return null;
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
            if (JavaScriptHelperModule == null)
                throw new Exception(_exceptionMessage);

            return JavaScriptHelperModule?.Invoke<IJSInProcessObjectReference>("InstantiateClass", constructorPath, args);
        }

        public void Dispose()
        {
            JavaScriptHelperModule?.Dispose();
        }
    }
}
