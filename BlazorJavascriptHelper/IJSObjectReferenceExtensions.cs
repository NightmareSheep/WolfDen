using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorJavascriptHelper
{
    public static class IJSObjectReferenceExtensions
    {
        public static T Prop<T>(this IJSInProcessObjectReference obj, string property)
        {
            return JavascriptHelper.Instance.GetJavascriptProperty<T>(property, obj);
        }

        public static void SetProp<T>(this IJSInProcessObjectReference obj, string property, T value)
        {
            JavascriptHelper.Instance.SetJavascriptProperty(new string[] { property }, value, obj);
        }
    }
}
