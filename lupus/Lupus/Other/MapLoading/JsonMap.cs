using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Other.MapLoading
{
    public class JsonMap
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Layer[] layers { get; set; }
        public Property[] Properties { get; set; }
        public string Name { get { return Properties?.FirstOrDefault(p => p.Name == "Name")?.Value; } }
    }
}
