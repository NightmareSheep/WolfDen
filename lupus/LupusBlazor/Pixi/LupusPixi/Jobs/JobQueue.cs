using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public class JobQueue
    {
        private bool busy;
        private Queue<IJob> Jobs { get; set; } = new();
        public event EventHandler QueueEmptyEvent;

        public void EnqueueJob(IJob job)
        {
            Console.WriteLine("Enqueue job");

            Jobs.Enqueue(job);
            if (!busy)
                 ContinueQueue();
        }

        private void JobComplete(object sender, EventArgs e)
        {
            var job = sender as IJob;
            Console.WriteLine("Job complete");
            job.OnComplete -= JobComplete;
             ContinueQueue();
        }

        private void ContinueQueue()
        {
            Console.WriteLine("ContinueQueue");

            if (Jobs.Count == 0)
            {
                busy = false;
                 RaiseQueueEmptyEvent();
                return;
            }

            busy = true;
            var j = Jobs.Dequeue();
            j.OnComplete += JobComplete;
             j.Run();
        }

        private void RaiseQueueEmptyEvent()
        {
            QueueEmptyEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
