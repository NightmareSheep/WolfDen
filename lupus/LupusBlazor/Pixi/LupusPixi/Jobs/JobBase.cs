using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi.Jobs
{
    public abstract class JobBase : IJob
    {
        public event EventHandler OnComplete;
        public event EventHandler OnStart;
        public bool JobStarted { get; private set; }
        public bool JobCompleted { get; private set; }

        public abstract void Run();

        protected void RaiseOnCompleteEvent()
        {
            JobCompleted = true;
            OnComplete?.Invoke(this, EventArgs.Empty);
        }

        protected void RaiseOnStartEvent()
        {
            JobStarted = true;
            OnStart?.Invoke(this, EventArgs.Empty);
        }
    }
}
