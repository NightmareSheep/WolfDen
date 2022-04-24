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
        public static void CreateSprite(IJSRuntime jSRuntime, string[] resourcePath, string id, int x, int y, DotNetObjectReference<Clickable> objRef = null, bool visible = true, string tint = null)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PixiHelper.CreateSprite", resourcePath, id, x, y, objRef, null, visible, tint);
        }

        public static void SetTextSprite(IJSRuntime jSRuntime, string id, int x, int y, string text = null, bool visible = true, string containerId = null, TextStyle textStyle = TextStyle.Health)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PixiHelper.SetTextSprite", id, x, y, text, visible, containerId, textStyle);
        }

        public static void DestroySprite(IJSRuntime jSRuntime, string id) 
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PixiHelper.DestroySprite", id);
        }

        public static void SetSpritePosition(IJSRuntime jSRuntime, string id, int x, int y)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PixiHelper.SetPositionOfSprite", id, x, y);
        }

        public static void SetSpriteVisible(IJSRuntime jSRuntime, string id, bool visible)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PixiHelper.SetVisibleOfSprite", id, visible);
        }

        public static void Animation(IJSRuntime jSRuntime, object callbackObject, string id, int callbackDuration, int duration, int x, int y, bool resetAnimation = false)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PlayAnimation", callbackObject, id, callbackDuration, duration, x, y, resetAnimation);
            
        }

        public static void MoveSprite(IJSRuntime jSRuntime, object callbackObject, string id, int callbackDuration, int duration, int startX, int startY, int destinationX, int destinationY, bool resetAnimation = false)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("MoveSprite", callbackObject, id, callbackDuration, duration, startX, startY, destinationX, destinationY, resetAnimation);
        }

        public static void SetFilter(IJSRuntime jSRuntime, string id, PixiFilter pixiFilter, bool apply)
        {
            ((IJSInProcessRuntime)jSRuntime).InvokeVoid("PixiHelper.SetFilter", id, pixiFilter.ToString(), apply);
        }
    }
}
