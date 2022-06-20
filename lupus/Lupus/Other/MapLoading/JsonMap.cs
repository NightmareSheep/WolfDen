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
        public int[] Teams { get 
            {
                var teamsString = Properties?.FirstOrDefault(p => p.Name == "Teams")?.Value;
                if (teamsString == null)
                    return null;

                var teams = teamsString.Split(',').Select(t => int.Parse(t));
                return teams.ToArray(); 
            } 
        }

        public int TileHeight { get; set; }
        public int TileWidth { get; set; }
        public TileSet[] TileSets { get; set; }
    }
}
