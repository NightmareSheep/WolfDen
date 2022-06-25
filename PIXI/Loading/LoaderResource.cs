using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorJavascriptHelper;

namespace PIXI.Loading
{
    public class LoaderResource : IDisposable
    {
        public string Name { get; set; }

        Texture _texture;
        public Texture Texture { 
            get 
            {
                return (_texture ??= new Texture(_jsInstance.Prop<IJSInProcessObjectReference>("texture")));
            } 
        }
        private IJSInProcessObjectReference _jsInstance;

        public LoaderResource(IJSInProcessObjectReference jsInstance)
        {
            _jsInstance = jsInstance;
        }

        public void Dispose()
        {
            _jsInstance.DisposeAsync();
        }
    }
}
