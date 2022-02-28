using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public class WaitForQueueJob : JobBase
    {
        public WaitForQueueJob(PlayAnimationJob playAnimationJob)
        {
            PlayAnimationJob = playAnimationJob;
        }

        public PlayAnimationJob PlayAnimationJob { get; }

        public override async Task Run()
        {
            await RaiseOnStartEvent();

            if (PlayAnimationJob.JobCompleted)
            {
                await RaiseOnCompleteEvent();
                return;
            }

            PlayAnimationJob.OnComplete += JobComplete;
            PlayAnimationJob.Animation.OnQueueCompleteEvent += JobComplete;

        }

        private async Task JobComplete()
        {
            await RaiseOnCompleteEvent();
            PlayAnimationJob.OnComplete -= JobComplete;
            PlayAnimationJob.Animation.OnQueueCompleteEvent -= JobComplete;
        }

        private async Task JobComplete(IJob job)
        {
            await JobComplete(null);
        }
    }
}
