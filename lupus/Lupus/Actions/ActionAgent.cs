using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Actions
{
    public class ActionAgent
    {
        public ActionTracker Tracker { get; }
        public IAction Action { get; }

        public ActionAgent(Game game, IAction action)
        {
            Tracker = game.ActionTracker;
            Tracker.Agents.Add(this);
            Action = action;
        }

        public void ActionUsed()
        {
            Tracker.RaiseActionUsedEvent();
        }

        public int GetActions()
        {
            return Action.GetAvailableActions();
        }
        
    }
}
