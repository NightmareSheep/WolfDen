using Lupus.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Behaviours.Defend
{
    public class Invulnurable : Health
    {
        public Invulnurable(Unit unit) : base(unit, 9999)
        {
        }

        public override Task Damage(int damage)
        {
            return Task.CompletedTask;
        }
    }
}
