using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class ItemBundle
    {
        public ItemBundle(GameItem item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
        public GameItem Item { get; }
        public int Quantity { get; set; }
    }
}
