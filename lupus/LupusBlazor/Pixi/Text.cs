using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Pixi
{
    public class Text : Sprite
    {
        private string spriteText;

        public string SpriteText
        {
            get
            {
                return spriteText;
            }
            set
            {
                if (this.JavascriptHelper != null)
                    this.JavascriptHelper.SetJavascriptProperty(new string[] { "text" }, value, this.JSInstance);
                spriteText = value;
            }
        }

        private KnownColor color = KnownColor.Black; 
        public KnownColor Color { 
            get { return color;  } 
            set { 
                color = value;
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "style", "fill" }, color.ToString(), this.JSInstance);
            } 
        }

        private float strokeThickness = 0f;
        public float StrokeThickNess
        {
            get { return strokeThickness; }
            set
            {
                strokeThickness = value;
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "style", "strokeThickness" }, strokeThickness, this.JSInstance);
            }
        }

        public Text(IJSRuntime jSRuntime, string text, JavascriptHelperModule javascriptHelper = null) : base(jSRuntime, null, null, javascriptHelper)
        {
            SpriteText = text;
            JSInstance = JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Text" }, new() { SpriteText });
        }
    }
}
