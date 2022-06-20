using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorJavascriptHelper;
using Microsoft.JSInterop;

namespace PIXI
{
    public class ViewPort : Container
    {
        public Application Application { get; }
        public int WorldWidth { get; }
        public int WorldHeight { get; }

        public ViewPort(Application application, int worldWidth, int worldHeight) : base(null, false)
        {
            Application = application;
            WorldWidth = worldWidth;
            WorldHeight = worldHeight;
            var viewportModule = ViewportModule.Instance;
            Console.WriteLine("Instantiate viewport");
            this.JSInstance = viewportModule.InstantiateViewport(this.Application.PixiApp, this.Application.ElementId, WorldWidth, WorldHeight);
            this.JSInstance.InvokeVoid("drag");
            this.JSInstance.InvokeVoid("pinch");
            this.JSInstance.InvokeVoid("wheel");
        }
    }
}
