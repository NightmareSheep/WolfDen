using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Actions
{
    public class ActionTracker
    {
        public event EventHandler ActionUsedEvent;
        public List<ActionAgent> Agents { get; set; } = new();

        public void RaiseActionUsedEvent()
        {
            ActionUsedEvent?.Invoke(this, EventArgs.Empty);
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
