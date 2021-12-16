using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Animation
{
    public class ChangeVisibilityAnimation : IAnimation
    {
        public int QueueDuration { get => 0; }
        public IJSRuntime IJSRuntime { get; }
        public string SpriteId { get; }
        public bool Visibility { get; }

        public ChangeVisibilityAnimation(IJSRuntime iJSRuntime, string spriteId, bool visibility)
        {
            IJSRuntime = iJSRuntime;
            SpriteId = spriteId;
            Visibility = visibility;
        }

        public async Task Play(Func<Task> callback)
        {
            await PixiHelper.SetSpriteVisible(IJSRuntime, SpriteId, Visibility);
            await callback();
        }
    }
}
