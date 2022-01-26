using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Actions
{
    public class ActionTracker
    {
        public event Func<Task> ActionUsedEvent;
        public List<ActionAgent> Agents { get; set; } = new();

        public async Task RaiseActionUsedEvent()
        {
            if (ActionUsedEvent != null)
            {
                var invocationList = ActionUsedEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }

        public int GetAvailableActions()
        {
            var result = 0;
            foreach (var agent in Agents)
                result += agent.GetActions();

            return result;
        }
    }
}
