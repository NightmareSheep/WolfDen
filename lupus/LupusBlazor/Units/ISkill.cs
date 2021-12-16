using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.Units
{
    public interface ISkill
    {
        string Name { get; }
        Task ClickSkill();
    }
}
