using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LupusBlazor
{
    public class Clickable
    {
        
        public event Func<Task> ClickEvent;

        [JSInvokable]
        public async Task RaisClickEvent() 
        {
            if (ClickEvent != null)
            {
                var invocationList = ClickEvent.GetInvocationList().Cast<Func<Task>>();
                foreach (var subscriber in invocationList)
                    await subscriber();
            }
        }
    }
}
