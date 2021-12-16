using LupusBlazor.Animation;
using LupusBlazor.Pixi;
using LupusBlazor.UI;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor
{
    public static class PixiHelper
    {
        public static async Task CreateSprite(IJSRuntime jSRuntime, string[] resourcePath, string id, int x, int y, DotNetObjectReference<Clickable> objRef = null, bool visible = true, string tint = null)
        {
            await jSRuntime.InvokeVoidAsync("PixiHelper.CreateSprite", resourcePath, id, x, y, objRef, null, visible, tint);
        }

        public static async Task SetTextSprite(IJSRuntime jSRuntime, string id, int x, int y, string text = null, bool visible = true, string containerId = null, TextStyle textStyle = TextStyle.Health)
        {
            await jSRuntime.InvokeVoidAsync("PixiHelper.SetTextSprite", id, x, y, text, visible, containerId, textStyle);
        }

        public static async Task DestroySprite(IJSRuntime jSRuntime, string id) 
        {
            await jSRuntime.InvokeVoidAsync("PixiHelper.DestroySprite", id);
        }

        public static async Task SetSpritePosition(IJSRuntime jSRuntime, string id, int x, int y)
        {
            await jSRuntime.InvokeVoidAsync("PixiHelper.SetPositionOfSprite", id, x, y);
        }

        public static async Task SetSpriteVisible(IJSRuntime jSRuntime, string id, bool visible)
        {
            await jSRuntime.InvokeVoidAsync("PixiHelper.SetVisibleOfSprite", id, visible);
        }

        public static async Task Animation(IJSRuntime jSRuntime, object callbackObject, string id, int callbackDuration, int duration, int x, int y, bool resetAnimation = false)
        {
            await jSRuntime.InvokeVoidAsync("PlayAnimation", callbackObject, id, callbackDuration, duration, x, y, resetAnimation);
        }

        public static async Task MoveSprite(IJSRuntime jSRuntime, object callbackObject, string id, int callbackDuration, int duration, int startX, int startY, int destinationX, int destinationY, bool resetAnimation = false)
        {
            await jSRuntime.InvokeVoidAsync("MoveSprite", callbackObject, id, callbackDuration, duration, startX, startY, destinationX, destinationY, resetAnimation);
        }

        public static async Task SetFilter(IJSRuntime jSRuntime, string id, PixiFilter pixiFilter, bool apply)
        {
            await jSRuntime.InvokeVoidAsync("PixiHelper.SetFilter", id, pixiFilter.ToString(), apply);
        }
    }
}
