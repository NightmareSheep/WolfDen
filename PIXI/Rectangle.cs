using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorJavascriptHelper;

namespace PIXI
{
    public class Rectangle : IDisposable
    {
        public IJSInProcessObjectReference JSInstance { get; set; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            JSInstance = JavascriptHelper.Instance.InstantiateJavascriptClass(new string[] { "PIXI","Rectangle" }, new List<object> { x, y, width, height });
        }

        public void Dispose()
        {
            JSInstance.Dispose();
        }
    }
}
