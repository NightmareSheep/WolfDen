using System;
using System.Collections.Generic;
using System.Text;

namespace LupusBlazor
{
    struct PixiResource
    {
        public string Name;
        public string[] Path;

        public PixiResource(string[] path, string name) : this()
        {
            Path = path;
            Name = name;
        }
    }
}
