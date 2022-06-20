using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PIXI;
using BlazorJavascriptHelper;

namespace PIXI.Loading
{
    public class Loader
    {
        private IJSInProcessObjectReference _instance;
        private JavascriptHelper javascriptHelper;

        public Dictionary<string,LoaderResource?> Resources { get; } = new Dictionary<string,LoaderResource?>();

        private static Loader? shared;
        public static Loader? Shared { 
            get 
            {
                if (shared == null)
                {
                    var jsInstance = JavascriptHelper.Instance.GetJavascriptProperty<IJSInProcessObjectReference>(new string[] { "PIXI", "Loader", "shared" });
                    shared = new Loader(jsInstance);
                }
                return shared; 
            } 
        }

        public Loader(IJSInProcessObjectReference instance)
        {
            _instance = instance;
            javascriptHelper = JavascriptHelper.Instance;
        }

        public async Task Load()
        {
            await javascriptHelper.AwaitFunction(_instance, "load");
            var keys = Resources.Keys;
            foreach (var key in keys)
            {
                var loaderResourceInstance = javascriptHelper.GetJavascriptProperty<IJSInProcessObjectReference>(new string[] { "resources", key }, _instance);
                Resources[key] = new LoaderResource(loaderResourceInstance);
            }
        }

        public void Add(string name, string url)
        {
            _instance.InvokeVoid("add", name, url);
            Resources.Add(name, null);
        }
    }
}
