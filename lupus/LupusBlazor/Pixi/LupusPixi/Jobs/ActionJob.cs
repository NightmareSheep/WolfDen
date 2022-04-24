using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public class ActionJob : JobBase
    {
        public ActionJob(Action action)
        {
            Action = action;
        }

        public Action Action { get; }

        public override void Run()
        {
             RaiseOnStartEvent();
             Action();
             RaiseOnCompleteEvent();
        }
    }
}
