using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using LupusBlazor.Pixi;
using System.Drawing;
using LupusBlazor.Other;

namespace LupusBlazor.Pixi.LupusPixi
{
    public static class PixiFilters
    {
        public static Dictionary<PixiFilter, IJSObjectReference> Filters = new();
        public static Dictionary<KnownColor, IJSObjectReference> TeamFilters = new();
        public static JavascriptHelperModule JavascriptHelper { get; set; }

        public static async Task Initialize(IJSRuntime jSRuntime)
        {
            JavascriptHelper = await JavascriptHelperModule.GetInstance(jSRuntime);

            var desaturateFilter = await JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "filters", "ColorMatrixFilter" }, new List<object>() { });
            await desaturateFilter.InvokeVoidAsync("desaturate");
            Filters.Add(PixiFilter.Desaturate, desaturateFilter);

            var glowFilter = await JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "filters", "GlowFilter" }, new List<object>() { });
            await JavascriptHelper.SetJavascriptProperty(new string[] { "outerStrength" }, 1, glowFilter);
            Filters.Add(PixiFilter.GlowFilter, glowFilter);
        }

        public static async Task<IJSObjectReference> GetTeamFilter(KnownColor knownColor)
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

            var filter = await JavascriptHelper.InstantiateJavascriptClass(new string[] { "PIXI", "filters", "MultiColorReplaceFilter" }, new List<object>() { input, 0.001f });
            TeamFilters.Add(knownColor, filter);

            return filter;
        }
    }
}
