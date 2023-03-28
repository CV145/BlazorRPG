using D20Tek.Common.Helpers;
using RPG.Game.Engine.Actions;
using RPG.Game.Engine.Factories.DTOs;
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
        private const string _resourceNamespace = "RPG.Game.Engine.Data.items.json";
        private static List<GameItem> _standardGameItems = new List<GameItem>();
        
        static ItemFactory()
        {
            Load();
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

        private static void Load()
        {
            var templates = JsonSerializationHelper.DeserializeResourceStream<ItemTemplate>(_resourceNamespace);
            foreach (var tmp in templates)
            {
                switch (tmp.Category)
                {
                    case GameItem.ItemCategory.Weapon:
                        BuildWeapon(tmp.Id, tmp.Name, tmp.Price, tmp.Damage);
                        break;
                    case GameItem.ItemCategory.Consumable:
                        BuildHealingItem(tmp.Id, tmp.Name, tmp.Price, tmp.Heals);
                        break;
                    default:
                        BuildMiscellaneousItem(tmp.Id, tmp.Name, tmp.Price);
                        break;
                }
            }
        }

        private static void BuildMiscellaneousItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(id, GameItem.ItemCategory.Miscellaneous, name, price));
        }

        private static void BuildWeapon(int id, string name, int price, int damageDice)
        {
            var weapon = new GameItem(id, GameItem.ItemCategory.Weapon, name, price, true);
            weapon.SetAction(new Attack(weapon, damageDice));
            weapon.Action = new Attack(weapon, damageDice);

            _standardGameItems.Add(weapon);
        }

        private static void BuildHealingItem(int id, string name, int price, int healPoints)
        {
            GameItem item = new GameItem(id, GameItem.ItemCategory.Consumable, name, price);
            item.SetAction(new Heal(item, healPoints));
            _standardGameItems.Add(item);
        }
    }
}
