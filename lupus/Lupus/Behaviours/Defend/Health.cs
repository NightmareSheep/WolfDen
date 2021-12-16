using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours.Defend
{
    public class Health
    {

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }

        public Unit Unit { get; }

        public virtual async Task Damage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                await Unit.Destroy();
            }


        }

        public Health(Unit unit, int health)
        {
            Unit = unit;
            MaxHealth = health;
            CurrentHealth = health;
        }
    }
}
