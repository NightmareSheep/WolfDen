using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public abstract class JobBase : IJob
    {
        public event Func<IJob, Task> OnComplete;
        public event Func<IJob, Task> OnStart;
        public bool JobStarted { get; private set; }
        public bool JobCompleted { get; private set; }

        public abstract Task Run();

        protected async Task RaiseOnCompleteEvent()
        {
            JobCompleted = true;
            var invocationList = OnComplete?.GetInvocationList()?.Cast<Func<IJob, Task>>();
            foreach (var subscriber in invocationList ?? Enumerable.Empty<Func<IJob, Task>>())
                await subscriber(this);
        }

        protected async Task RaiseOnStartEvent()
        {
            JobStarted = true;
            var invocationList = OnStart?.GetInvocationList()?.Cast<Func<IJob, Task>>();
            foreach (var subscriber in invocationList ?? Enumerable.Empty<Func<IJob, Task>>())
                await subscriber(this);
        }
    }
}
