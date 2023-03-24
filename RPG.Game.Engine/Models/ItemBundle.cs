using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class ItemBundle
    {
        public GameItem Item { get; set; } = GameItem.Empty;
        public int Quantity { get; set; }
    }
}
