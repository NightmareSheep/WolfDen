using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class LupusPixiApplication
    {
        public Application Application { get; set; }
        public ViewPort ViewPort { get; set; }

        public LupusPixiApplication(IJSRuntime iJSRuntime, int worldWidth, int worldHeight)
        {
            this.Application = new Application(iJSRuntime);
            this.ViewPort = new ViewPort(iJSRuntime, this.Application, worldWidth, worldHeight);
        }

        public async Task Initialize()
        {
            await Application.Initialize("game");
            await ViewPort.Initialize();
            await Application.Stage.AddChild(ViewPort);
        }
    }

    
}
