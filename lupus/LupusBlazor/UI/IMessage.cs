using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.UI
{
    public interface IMessage
    {
        bool Enabled { get; set; }
        void ShowMessage();
    }
}
