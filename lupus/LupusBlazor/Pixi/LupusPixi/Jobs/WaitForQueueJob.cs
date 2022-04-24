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

        public override void Run()
        {
             RaiseOnStartEvent();

            if (PlayAnimationJob.JobCompleted)
            {
                 RaiseOnCompleteEvent();
                return;
            }

            PlayAnimationJob.OnComplete += JobComplete;
            PlayAnimationJob.Animation.OnQueueCompleteEvent += JobComplete;

        }

        private void JobComplete(object sender, EventArgs e)
        {
             RaiseOnCompleteEvent();
            PlayAnimationJob.OnComplete -= JobComplete;
            PlayAnimationJob.Animation.OnQueueCompleteEvent -= JobComplete;
        }

        private void JobComplete(IJob job)
        {
             JobComplete(null);
        }
    }
}
