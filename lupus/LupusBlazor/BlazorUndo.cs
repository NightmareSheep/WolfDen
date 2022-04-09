using Lupus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor
{
    public class BlazorUndo : Undo
    {
        public BlazorUndo(BlazorGame game) : base(game)
        {
        }
    }
}
