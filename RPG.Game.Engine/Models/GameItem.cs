using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class GameItem
    {
        public GameItem(int itemTypeID, string name, int price, bool isUnique = false)
        {
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
        }

        public static GameItem Empty { get; internal set; }
        public int ItemTypeID { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Price { get; set; }
        public bool IsUnique { get; set; }

        public virtual GameItem Clone() =>
            new GameItem(ItemTypeID, Name, Price);
    }
}
