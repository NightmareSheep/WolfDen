using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace Wolfden.Server.Other
{
    public static class Statics
    {
        public static List<KnownColor> Colors { get; } = new List<KnownColor>() {
            KnownColor.Red,
            KnownColor.Green,
            KnownColor.Blue,
            KnownColor.Pink,
            KnownColor.Purple,
            KnownColor.Brown,
            KnownColor.Black,
            KnownColor.White
        };
    }
}
