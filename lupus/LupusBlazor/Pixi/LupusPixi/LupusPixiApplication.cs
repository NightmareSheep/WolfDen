using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIXI;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class LupusPixiApplication
    {
        public Application Application { get; set; }
        public ViewPort ViewPort { get; set; }

        public LupusPixiApplication(IJSRuntime iJSRuntime, int worldWidth, int worldHeight)
        {
            this.Application = new Application(iJSRuntime, "game");
            this.ViewPort = new ViewPort(Application, worldWidth, worldHeight);
            this.Application.Stage.AddChild(ViewPort);
        }
    }

    
}
