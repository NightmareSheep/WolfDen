using BlazorJavascriptHelper;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIXI
{
    public class CompositeTilemap : Container
    {
        public CompositeTilemap() : base(null, false)
        {
            this.JSInstance = JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "tilemap", "CompositeTilemap" }, null);
        }

        public void Tile(Texture texture, int x, int y)
        {
            JSInstance.InvokeVoid("tile", texture.JSInstance, x, y, new { rotate = texture.Rotate });
        }
    }
}
