using Lupus.Other;
using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours
{
    public class SkillPoints : IDestroy
    {
        public int MaxPoints { get; }
        public int CurrentPoints { get; private set; }
        public Game Game { get; }
        public Unit Unit { get; }
        public event EventHandler<int> SkillPointsUsedEvent;

        public SkillPoints(Game game, Unit unit, int points)
        {
            MaxPoints = points;
            CurrentPoints = 0;
            Game = game;
            Unit = unit;
            game.TurnResolver.StartTurnEvent += StartTurn;
        }

        public virtual void UseSkillPoints(int amount, object sender)
        {
            CurrentPoints -= amount;
            SkillPointsUsedEvent?.Invoke(sender, amount);
        }

        public virtual void StartTurn(object sender, List<Player> activePlayers)
        {
            if (activePlayers.Contains(Unit.Owner))
            {
                CurrentPoints = MaxPoints;
            }
            else
            {
                CurrentPoints = 0;
            }
        }

        public void Destroy()
        {
            Game.TurnResolver.StartTurnEvent -= StartTurn;
        }
    }
}
