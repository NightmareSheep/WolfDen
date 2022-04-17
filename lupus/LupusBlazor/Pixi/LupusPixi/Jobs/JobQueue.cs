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
        public event Func<Task> QueueEmptyEvent;

        public async Task EnqueueJob(IJob job)
        {
            Console.WriteLine("Enqueue job");

            Jobs.Enqueue(job);
            if (!busy)
                await ContinueQueue();
        }

        private async Task JobComplete(IJob job)
        {
            Console.WriteLine("Job complete");
            job.OnComplete -= JobComplete;
            await ContinueQueue();
        }

        private async Task ContinueQueue()
        {
            Console.WriteLine("ContinueQueue");

            if (Jobs.Count == 0)
            {
                busy = false;
                await RaiseQueueEmptyEvent();
                return;
            }

            busy = true;
            var j = Jobs.Dequeue();
            j.OnComplete += JobComplete;
            await j.Run();
        }

        private async Task RaiseQueueEmptyEvent()
        {
            var invocationList = QueueEmptyEvent?.GetInvocationList()?.Cast<Func<Task>>() ?? Enumerable.Empty<Func<Task>>();
            foreach (var subscriber in invocationList)
                await subscriber();
        }
    }
}
