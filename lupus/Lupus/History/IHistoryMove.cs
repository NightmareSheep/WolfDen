using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus
{
    public interface IHistoryMove
    {
        Task Execute(Game game);
    }
}
