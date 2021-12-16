using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Animation
{
    public interface IAnimation
    {
        int QueueDuration { get; }

        Task Play(Func<Task> callback);

    }
}
