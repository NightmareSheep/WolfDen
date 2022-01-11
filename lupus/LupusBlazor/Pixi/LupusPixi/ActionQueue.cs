using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi.LupusPixi
{
    public class ActionQueue
    {
        private bool busy;
        private Queue<Func<Task>> Actions { get; set; } = new Queue<Func<Task>>();

        public async Task AddAction(Func<Task> action)
        {
            if (!busy)
            {
                busy = true;
                await action();
            }
            else
                Actions.Enqueue(action);
        }

        public async Task ContinueQueue()
        {
            if (Actions.Count == 0)
            {
                busy |= false;
                return;
            }

            var action = Actions.Dequeue();
            await action();
        }
    }
}
