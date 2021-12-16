using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Other
{
    public class ColorHelper
    {
        public void GenerateColorFilters(IJSRuntime jSRuntime,  List<Color> colors)
        {
            var colorFilters = new List<ColorFilter>();

            for (int i = 0; i < colors.Count; i++)
            {
                var color = colors[i];
                var tints = GenerateTints(color);
                var tintsString = string.Join(",", tints.Select(tint => ToHexString(tint)));
                var playerColor = new ColorFilter
                {
                    Id = i + 1,
                    Name = ColorTranslator.ToHtml(color),
                    Colors = tintsString
                };
                colorFilters.Add(playerColor);
            }

            jSRuntime.InvokeVoidAsync("setFilters", JsonConvert.SerializeObject(colorFilters));
        }

        private Color[] GenerateTints(Color source)
        {
            var colors = new Color[6];
            colors[0] = Tint(source, Color.White, 0.8m);
            colors[1] = Tint(source, Color.White, 0.4m);
            colors[2] = source;
            colors[3] = Tint(source, Color.Black, 0.1m);
            colors[4] = Tint(source, Color.Black, 0.2m);
            colors[5] = Tint(source, Color.Black, 0.8m);
            return colors;
        }

        private Color Tint(Color source, Color tint, decimal alpha)
        {
            //(tint -source)*alpha + source
            var red = Convert.ToInt32(((tint.R - source.R) * alpha + source.R));
            var blue = Convert.ToInt32(((tint.B - source.B) * alpha + source.B));
            var green = Convert.ToInt32(((tint.G - source.G) * alpha + source.G));
            return Color.FromArgb(255, red, green, blue);
        }

        private string ToHexString(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";
    }
}
