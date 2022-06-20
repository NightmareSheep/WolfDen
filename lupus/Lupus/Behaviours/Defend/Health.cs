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

        public virtual void Damage(int damage)
        {
            if (CurrentHealth <= 0)
                return;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                
                Death();
            }


        }

        protected virtual void Death()
        {
            Unit.Destroy();
        }

        public Health(Unit unit, int health)
        {
            Unit = unit;
            MaxHealth = health;
            CurrentHealth = health;
        }
    }
}
