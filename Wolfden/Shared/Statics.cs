using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolfden.Shared.Models.Hosting;
using System.Reflection;

namespace Wolfden.Shared
{
    public class Statics
    {
        public static List<Map> Maps { get {
                return new List<Map>() {
                    new Map()
                    {
                        Name = "Duel",
                        NumberOfPlayers = 2
                    },
                    new Map()
                    {
                        Name = "Duel 2",
                        NumberOfPlayers = 2
                    },                    
                    new Map()
                    {
                        Name = "Level2",
                        NumberOfPlayers = 2
                    },
                    new Map()
                    {
                        Name = "Level1",
                        NumberOfPlayers = 4
                    },
                    new Map()
                    {
                        Name = "Forgotten hideout",
                        NumberOfPlayers = 4
                    },
                    new Map()
                    {
                        Name = "Dirt brawl",
                        NumberOfPlayers = 4
                    },
                }; 
            } 
        }
    }
}
