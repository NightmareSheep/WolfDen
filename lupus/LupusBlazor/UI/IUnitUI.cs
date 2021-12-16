using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.UI
{
    public interface IUnitUI
    {
        Task SetCharacterUI(string name);
        Task SetCharacterSkill(string name, Func<Task> OnClick);
        Task ResetCharacterUI();
        Task ClickSkill(int i);
    }
}
