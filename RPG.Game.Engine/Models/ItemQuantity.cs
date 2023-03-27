using RPG.Game.Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class ItemQuantity
    {
        public int ItemID { get; set; }
        public int Quantity { get; set; }
        public string QuantityItemDescription => $"{ItemFactory.GetItemName(ItemID)} (x{Quantity})";
    }
}
