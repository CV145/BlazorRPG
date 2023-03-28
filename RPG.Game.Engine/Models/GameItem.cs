using RPG.Game.Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class GameItem
    {
        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable
        }

        public static readonly GameItem Empty = new GameItem();

        public GameItem(int itemTypeID, ItemCategory category, string name, int price, bool isUnique = false, IAction? action = null)
        {
            ItemTypeID = itemTypeID;
            Category = category;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
        }

        public int ItemTypeID { get; }

        public ItemCategory Category { get; }

        public string Name { get; }

        public int Price { get; }

        public bool IsUnique { get; }

        public IAction? Action { get; set; }

        public GameItem()
        {
        }

        public virtual GameItem Clone() =>
            new GameItem(ItemTypeID, Category, Name, Price, IsUnique, Action);

        internal void SetAction(IAction? action)
        {
            this.Action = action;
        }

        public MessageBox PerformAction(LivingEntity actor, LivingEntity target)
        {
            if (Action is null)
            {
                throw new InvalidOperationException("CurrentWeapon.Action cannot be null");
            }

            return Action.Execute(actor, target);
        }
    }
}
