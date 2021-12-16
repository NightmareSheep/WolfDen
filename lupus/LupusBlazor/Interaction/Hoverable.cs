using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;

namespace LupusBlazor
{
    public class Hoverable
    {
        public event Action PointerOverEvent;

        [JSInvokable]
        public void RaisPointerOverEvent() => PointerOverEvent?.Invoke();

        public event Action PointerOutEvent;

        [JSInvokable]
        public void RaisPointerOutEvent() => PointerOutEvent?.Invoke();
    }
}
