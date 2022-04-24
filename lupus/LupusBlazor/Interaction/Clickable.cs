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
        public event EventHandler ClickEvent;

        [JSInvokable]
        public void RaisClickEvent() 
        {
            ClickEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
