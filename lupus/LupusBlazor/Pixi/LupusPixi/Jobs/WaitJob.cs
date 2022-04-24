using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    internal class WaitJob : JobBase
    {
        public WaitJob(TriggerJob triggerJob)
        {
            TriggerJob = triggerJob;
        }

        public TriggerJob TriggerJob { get; }

        public override void Run()
        {
             RaiseOnStartEvent();

            if (TriggerJob.JobCompleted)
            {
                RaiseOnCompleteEvent();
                return;
            }

            TriggerJob.OnComplete += TriggerJobCompleted;
        }

        public void TriggerJobCompleted(object sender, EventArgs e)
        {
            var job = sender as IJob;
            TriggerJob.OnComplete -= TriggerJobCompleted;
            RaiseOnCompleteEvent();
        }
    }
}
