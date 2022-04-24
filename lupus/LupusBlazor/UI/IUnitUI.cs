using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.UI
{
    public interface IUnitUI
    {
        void SetCharacterUI(string name);
        void SetCharacterSkill(string name, Action OnClick);
        void ResetCharacterUI();
        void ClickSkill(int i);
    }
}
