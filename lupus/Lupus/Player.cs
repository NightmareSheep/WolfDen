using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Newtonsoft.Json;

namespace Lupus
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public Dictionary<string, object> GameObjects = new Dictionary<string, object>();
        public int Team { get; set; }
        public KnownColor Color { get; set; }

    }
}
