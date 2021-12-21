using LupusBlazor.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wolfden.Client.Models;

namespace Wolfden.Client.Other
{
    public static class Statics
    {
        public static readonly string[] Names = { "Apisi", "Archer", "Ares", "Arrax", "Axel", "Basil", "Bruno", "Chronos", "Colt", "Comet", "Cronus", "Echo", "Elda", "Essos", "Gunner", "Havoc", "Hera", "Hunter", "Juno", "Khal", "Lotus", "Major", "Orbit", "Pyro", "Raven", "Rhea", "Rider", "Rollo", "Rune", "Sarge", "Stark", "Storm", "Thor", "Tiva", "Tyr", "Ubba", "Valor", "Wolf", "Yara", "Zeus", "Diego", "Accalia", "Adolfa", "Akela", "Alpina", "Ama", "Amora", "Ash", "Athena", "Blanca", "Eva", "Hera", "Ivory", "Ivy", "Jenna", "Kiba", "Koda", "Kodi", "Lulu", "Luna", "Lupa", "Maia", "Maka", "Maya", "Meika", "Nala", "Raven", "Ruby", "Seraya", "Skye", "Storm", "Tallulah", "Ula", "Uma", "Una", "Vixen", "Winter", "Xena", "Yuki", "Zelda" };
        public static AudioPlayer AudioPlayer { get; set; }

        public static Link[] MainMenuLinks { get; } = new Link[]
        {
            new Link("Singleplayer", "Singleplayer"),
            new Link("Multiplayer"),
            new Link("Options"),
            new Link("About"),
            //new Link("Login","authentication/login"),
            //new Link("game/4e0f20d6-3cc2-456c-9a04-e400d7f5a634"),
            //new Link("Register","authentication/register"),
            //new Link("home"),
        };
    }
}
