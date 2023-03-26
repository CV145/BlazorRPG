using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Weapon : GameItem
    {
        public Weapon(int itemTypeID, string name, int price, int damageRoll)
            : base(itemTypeID, ItemCategory.Weapon, name, price, true)
        {
            damageRoll = damageRoll;
        }

        public int DamageRoll { get; set; } 

        public override GameItem Clone() =>
            new Weapon(ItemTypeID, Name, Price, DamageRoll);
    }
}
