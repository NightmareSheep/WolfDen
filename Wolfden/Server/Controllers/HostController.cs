using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lobbies;
using Lobbies.LobbyBehaviour;
using LupusLobbies;
using Wolfden.Server.Other;
using Microsoft.AspNetCore.Authorization;
using Wolfden.Server.Models;
using System.IO;
using Wolfden.Shared.Models.Hosting;
using Microsoft.AspNetCore.Hosting;
using Wolfden.Shared.Models;

namespace Wolfden.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class HostController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public HostController(IWebHostEnvironment env)
        {
            _env = env;
        }


        [HttpPost]
        public async Task<IActionResult> Post(HostForm host)
        {
            var slots = new List<LupusLobbySlot>();
            var map = Shared.Statics.Maps.FirstOrDefault(map => map.Name == host.MapName);
            var AvailableColors = Statics.Colors.Skip(map.NumberOfPlayers).ToList();

            for (var i = 0; i < map.NumberOfPlayers; i++)
            {
                slots.Add(new LupusLobbySlot(i, new SlotColor(Statics.Colors, AvailableColors, Statics.Colors[i])));
            }

            var lobby = new LupusLobbyServer(host.Name, slots, host.MapName, this._env.WebRootPath);
            ConcurrencyObjects.AddObject(lobby.Id, lobby);
            return Ok(lobby.Id);
        }

        [HttpGet("GetFile")]
        public async Task<IActionResult> GetFile()
        {
            var path = _env.WebRootPath + "/game/maps/Level1/Level1.preview.png";
            Byte[] b = await System.IO.File.ReadAllBytesAsync(path);
            return File(b, "image/png");
        }

        [Route("/api/map/{mapId}")]
        [HttpGet]
        public async Task<ActionResult<string>> GetMap(string mapId)
        {
            var path = _env.WebRootPath + "/game/maps/" + mapId + "/" + mapId + ".json";
            var mapString = await System.IO.File.ReadAllTextAsync(path);

            return mapString;
        }

        [HttpGet("GetMaps")]
        public ActionResult<List<Map>> GetMaps()
        {
            return Wolfden.Shared.Statics.Maps;
        }

        [Route("/api/getLobbies")]
        [HttpGet]
        public ActionResult<List<LobbyListItem>> GetLobbies()
        {
            var lobbies = ConcurrencyObjects.GetObjectsOfType<Lobbies.Lobby>();
            var lobbyList = lobbies.Select(lobby => new LobbyListItem() { Id = lobby.Id.ToString(), Name = lobby.Name }).ToList();
            return lobbyList;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var path = _env.WebRootPath + "/game/maps/Level1/Level1.json";
            var mapString = await System.IO.File.ReadAllTextAsync(path);

            return mapString;
        }

        [Route("/api/error")]
        [HttpGet]
        public ActionResult Error()
        {
            throw new Exception("Error api was called");
        }
    }
}
