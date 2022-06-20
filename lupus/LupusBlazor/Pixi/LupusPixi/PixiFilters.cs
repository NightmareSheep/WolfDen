using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using LupusBlazor.Pixi;
using System.Drawing;
using LupusBlazor.Other;
using PIXI;
using BlazorJavascriptHelper;

namespace LupusBlazor.Pixi.LupusPixi
{
    public static class PixiFilters
    {
        public static Dictionary<PixiFilter, IJSInProcessObjectReference> Filters = new();
        public static Dictionary<KnownColor, IJSInProcessObjectReference> TeamFilters = new();
        public static JavascriptHelper JavascriptHelper { get; set; }

        public static void Initialize(IJSRuntime jSRuntime)
        {
            JavascriptHelper =  JavascriptHelper.Instance;

            var desaturateFilter =  JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "filters", "ColorMatrixFilter" }, new List<object>() { });
             desaturateFilter.InvokeVoid("desaturate");
            Filters.Add(PixiFilter.Desaturate, desaturateFilter);

            var glowFilter =  JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "filters", "GlowFilter" }, new List<object>() { });
             JavascriptHelper.SetJavascriptProperty(new string[] { "outerStrength" }, 1, glowFilter);
            Filters.Add(PixiFilter.GlowFilter, glowFilter);
        }

        public static IJSInProcessObjectReference GetTeamFilter(KnownColor knownColor)
        {
            if (TeamFilters.Keys.Contains(knownColor))
            {
                return TeamFilters[knownColor];
            }

            var basicColors = new List<List<float>>();
            basicColors.Add(new List<float>() { 217f / 255f, 217f / 255f, 217f / 255f });
            basicColors.Add(new List<float>() { 155f / 255f, 155f / 255f, 155f / 255f });
            basicColors.Add(new List<float>() { 130f / 255f, 130f / 255f, 130f / 255f });
            basicColors.Add(new List<float>() { 116f / 255f, 116f / 255f, 116f / 255f });
            basicColors.Add(new List<float>() { 103f / 255f, 103f / 255f, 103f / 255f });
            basicColors.Add(new List<float>() {  21f / 255f,  21f / 255f,  21f / 255f });

            ColorHelper colorHelper = new();
            var color = Color.FromKnownColor(knownColor);
            var tints = colorHelper.GenerateTints(color);

            var input = new List<List<List<float>>>();
            for (var i = 0; i < 6; i++)
            {
                var tint = tints[i];
                var rgb = new List<float>();
                rgb.Add((float)tint.R / 255f);
                rgb.Add((float)tint.G / 255f);
                rgb.Add((float)tint.B / 255f);

                var replace = new List<List<float>>();
                replace.Add(basicColors[i]);
                replace.Add(rgb);
                input.Add(replace);
            }

            var filter =  JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "filters", "MultiColorReplaceFilter" }, new List<object>() { input, 0.001f });
            TeamFilters.Add(knownColor, filter);

            return filter;
        }
    }
}
