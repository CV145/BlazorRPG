using RPG.Game.Engine.Actions;
using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
    //All game items created here
     internal static class ItemFactory
    {
        private static List<GameItem> _standardGameItems = new List<GameItem>();
        
        static ItemFactory()
        {
            BuildWeapon(1001, "Pointy Stick", 1, 2);
            BuildWeapon(1002, "Rusty Sword", 5, 3);

            BuildWeapon(1501, "Snake fangs", 0, 2);
            BuildWeapon(1502, "Rat claws", 0, 2);
            BuildWeapon(1503, "Spider fangs", 0, 4);

            BuildHealingItem(2001, "Granola bar", 5, 2);
            BuildMiscellaneousItem(3001, "Oats", 1);
            BuildMiscellaneousItem(3002, "Honey", 2);
            BuildMiscellaneousItem(3003, "Raisins", 2);

            BuildMiscellaneousItem(9001, "Snake fang", 1);
            BuildMiscellaneousItem(9002, "Snakeskin", 2);
            BuildMiscellaneousItem(9003, "Rat tail", 1);
            BuildMiscellaneousItem(9004, "Rat fur", 2);
            BuildMiscellaneousItem(9005, "Spider fang", 1);
            BuildMiscellaneousItem(9006, "Spider silk", 2);
        }

        //Look through item database and clone a new copy of it
        public static GameItem? CreateGameItem(int itemTypeID)
        {
            var standardItem = _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID);
            return standardItem?.Clone();
        }

        //Get the first item that matches given ID or "" if null
        public static string GetItemName(int itemTypeId)
        {
            return _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeId)?.Name ?? "";
        }

        private static void BuildMiscellaneousItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(id, GameItem.ItemCategory.Miscellaneous, name, price));
        }

        private static void BuildWeapon(int id, string name, int price, int damageDice)
        {
            var weapon = new GameItem(id, GameItem.ItemCategory.Weapon, name, price, true);
            weapon.Action = new Attack(weapon, damageDice);

            _standardGameItems.Add(weapon);
        }

        private static void BuildHealingItem(int id, string name, int price, int hitPointsToHeal)
        {
            GameItem item = new GameItem(id, GameItem.ItemCategory.Consumable, name, price);
            item.Action = new Heal(item, hitPointsToHeal);
            _standardGameItems.Add(item);
        }
    }
}
