using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
     internal static class ItemFactory
    {
        private static List<GameItem> _standardGameItems = new List<GameItem>
        {
            new Weapon(1001, "Pointy Stick", 1, 1, 2),
            new Weapon (1002, "Rusty Sword", 5, 1, 3)
        };

        //Look through item database and clone a new copy of it
        public static GameItem? CreateGameItem(int itemTypeID)
        {
            var standardItem = _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID);
            return standardItem?.Clone();
        }
    }
}
