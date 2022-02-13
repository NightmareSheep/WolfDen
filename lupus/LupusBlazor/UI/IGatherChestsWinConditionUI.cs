using Lupus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LupusBlazor.UI
{
    public interface IGatherChestsWinConditionUI
    {
        public void SetScores(List<List<Player>> players, int[] scores);

        public void SetTurn(int currentTurn, int maxTurn);

        public void AnnounceVictor(string name);
    }
}
