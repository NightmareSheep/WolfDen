using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public class ActionJob : JobBase
    {
        public ActionJob(Func<Task> action)
        {
            Action = action;
        }

        public Func<Task> Action { get; }

        public override async Task Run()
        {
            await RaiseOnStartEvent();
            await Action();
            await RaiseOnCompleteEvent();
        }
    }
}
