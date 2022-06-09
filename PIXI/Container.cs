﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LupusBlazor.Pixi
{
    public class Container : DisplayObject
    {

        public Container(IJSRuntime jSRuntime, IJSInProcessObjectReference instance = null, JavascriptHelperModule javascriptHelper = null, bool instantiateJSInstance = true) : base(jSRuntime, instance, javascriptHelper)
        {
            if (instance == null && instantiateJSInstance)
                this.JSInstance = JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "Container" }, null);
        }

        public void AddChild(Container child)
        {
             this.JSInstance.InvokeVoid("addChild", child.JSInstance);
        }

        public void RemoveChild(Container child)
        {
            if (JSInstance != null && child?.JSInstance != null)
                 this.JSInstance.InvokeVoid("removeChild", child.JSInstance);
        }

        public void RemoveChildren()
        {
            if (JSInstance != null)
                 this.JSInstance.InvokeVoid("removeChildren");
        }


    }
}
