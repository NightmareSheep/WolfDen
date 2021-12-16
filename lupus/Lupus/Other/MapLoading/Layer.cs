using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Other.MapLoading
{
    public class Layer
    {
        public Layer[] layers { get; set; }
        public string Name { get; set; }
        public uint[] Data { get; set; }
    }
}
