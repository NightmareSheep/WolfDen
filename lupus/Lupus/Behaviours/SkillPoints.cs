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
        public event Func<int, object, Task> SkillPointsUsedEvent;

        public SkillPoints(Game game, Unit unit, int points)
        {
            MaxPoints = CurrentPoints = points;
            Game = game;
            Unit = unit;
            game.TurnResolver.StartTurnEvent += StartTurn;
        }

        public virtual async Task UseSkillPoints(int amount, object spender)
        {
            CurrentPoints -= amount;
            if (SkillPointsUsedEvent != null)
                await SkillPointsUsedEvent.Invoke(amount, spender);
        }

        public virtual Task StartTurn(List<Player> activePlayers)
        {
            if (activePlayers.Contains(Unit.Owner))
            {
                CurrentPoints = MaxPoints;
            }
            else
            {
                CurrentPoints = 0;
            }
            return Task.CompletedTask;
        }

        public Task Destroy()
        {
            Game.TurnResolver.StartTurnEvent -= StartTurn;
            return Task.CompletedTask;
        }
    }
}
