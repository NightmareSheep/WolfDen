using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class Sprite : Container
    {
        public IJSObjectReference Texture { get; }

        private KnownColor tint;
        public KnownColor Tint { 
            get { return tint; }
            set {
                var color = Color.FromKnownColor(value);
                var hexValue = ColorTranslator.ToHtml(Color.FromArgb(color.ToArgb())).Remove(0,1);
                var colorNr = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);               
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "tint" }, colorNr, this.JSInstance); 
                tint = value; 
            }
        }

        private int width;
        public int Width
        {
            get { return width; }
            set { this.JavascriptHelper.SetJavascriptProperty(new string[] { "width" }, value, this.JSInstance); width = value; }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set { this.JavascriptHelper.SetJavascriptProperty(new string[] { "height" }, value, this.JSInstance); height = value; }
        }

        public Sprite(IJSRuntime jSRuntime, IJSInProcessObjectReference texture, IJSInProcessObjectReference instance = null, JavascriptHelperModule javascriptHelper = null, bool instantiateJSInstance = true) : base(jSRuntime, instance, javascriptHelper, false)
        {
            this.Texture = texture;
            if (JSInstance == null && instance == null)
                this.JSInstance = this.JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Sprite" }, new() { this.Texture });
        }

        public void SetAnchor(float x, float? y = null)
        {
            if (y == null)
                y = x;

             this.JSInstance.InvokeVoid("anchor.set", x, y);
        }
    }
}
