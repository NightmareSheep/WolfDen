using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Audio.Json
{
    public class AudioJson
    {
        public List<string> Src { get; set; }
        public Dictionary<string, List<float>> Sprite { get; set; }
    }
}
