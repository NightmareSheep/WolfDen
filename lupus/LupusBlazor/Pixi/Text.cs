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
        public string SpriteText { get; }
        private KnownColor color = KnownColor.Black; 
        public KnownColor Color { 
            get { return color;  } 
            set { 
                color = value;
                this.JavascriptHelper.SetJavascriptProperty(new string[] { "style", "fill" }, color.ToString(), this.JSInstance);
            } 
        }

        public Text(Application application, IJSRuntime jSRuntime, string text, JavascriptHelper javascriptHelper = null) : base(application, jSRuntime, null, null, javascriptHelper)
        {
            SpriteText = text;
        }

        

        public override async Task Initialize()
        {
            if (this.JavascriptHelper == null)
                this.JavascriptHelper = await new JavascriptHelper(this.JSRuntime).Initialize();

            this.JSInstance = await this.JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Text" }, new() { this.SpriteText });
        }
    }
}
