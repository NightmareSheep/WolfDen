using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public class TriggerJob : JobBase
    {
        public override async Task Run()
        {
            await RaiseOnStartEvent();
            await RaiseOnCompleteEvent();
        }
    }
}
