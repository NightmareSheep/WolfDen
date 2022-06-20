using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorJavascriptHelper;

namespace PIXI
{
    public class Texture : IDisposable
    {
        public IJSInProcessObjectReference JSInstance { get; set; }

        public int Width { get { return JSInstance.Prop<int>("width"); } }
        public int Height { get { return JSInstance.Prop<int>("height"); } }

        public Texture(IJSInProcessObjectReference jSInstance)
        {
            JSInstance = jSInstance;
        }

        public Texture(Texture baseTexture, Rectangle frame)
        {
            JSInstance = JavascriptHelper.Instance.InstantiateJavascriptClass(new string[] { "PIXI", "Texture" }, new List<object> { baseTexture.JSInstance, frame.JSInstance });
        }

        public void Dispose()
        {
            JSInstance.Dispose();
        }
    }
}
