using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public interface IJob
    {
        event EventHandler OnComplete;
        event EventHandler OnStart;

        void Run();
    }
}
