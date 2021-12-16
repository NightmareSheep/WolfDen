using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Wolfden.Server.Hubs
{
    public interface IlupusLobbyClient : ILobbyClient
    {
        Task ChangeColor(string userId, KnownColor color);
    }
}
