using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Trader : LivingEntity
    {
        public Trader(int id, string name)
            : base(id, name, 10, 10, 10, 999, 999, 100)
        {
        }
    }
}
