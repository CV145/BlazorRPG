using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Weapon : GameItem
    {
        public Weapon(int itemTypeID, string name, int price, string damageRoll)
            : base(itemTypeID, name, price, true)
        {
            damageRoll = damageRoll;
        }

        public string DamageRoll { get; set; } = string.Empty;

        public override GameItem Clone() =>
            new Weapon(ItemTypeID, Name, Price, DamageRoll);
    }
}
