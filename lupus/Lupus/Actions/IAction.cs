using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Actions
{
    public interface IAction
    {
        ActionAgent Agent { get; }

        int GetAvailableActions();

    }
}
