using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Inventory
    {
        private readonly List<GameItem> _backingInventory = new List<GameItem>();
        private readonly List<ItemBundle> _backingGroupedInventory = new List<ItemBundle>();

        public Inventory(IEnumerable<GameItem> items)
        {
            if (items == null)
            {
                return;
            }

            foreach (GameItem item in items)
            {
                AddItem(item);
            }
        }

        public Inventory()
        {
        }

        public IReadOnlyList<GameItem> Items => _backingInventory.AsReadOnly();

        public IReadOnlyList<ItemBundle> GroupedItems => _backingGroupedInventory.AsReadOnly();

        //Properties that retrieve certain types of items using LINQ
		public IEnumerable<GameItem> Weapons => Items.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();

        public List<GameItem> Consumables =>
            Items.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();

        public void AddItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            _backingInventory.Add(item);

            if (item.IsUnique)
            {
                _backingGroupedInventory.Add(new ItemBundle { Item = item, Quantity = 1 });
            }
            else
            {
                //if none of the items match by given item id - add new item bundle
                if (_backingGroupedInventory.All(gi => gi.Item.ItemTypeID != item.ItemTypeID))
                {
                    _backingGroupedInventory.Add(new ItemBundle { Item = item, Quantity = 0 });
                }

                _backingGroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }
        }

        public void RemoveItem(GameItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item));

            _backingInventory.Remove(item);

            if (item.IsUnique == false)
            {
                ItemBundle groupedInventoryItemToRemove =
                    _backingGroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);

                if (groupedInventoryItemToRemove != null)
                {
                    if (groupedInventoryItemToRemove.Quantity == 1)
                    {
                        _backingGroupedInventory.Remove(groupedInventoryItemToRemove);
                    }
                    else
                    {
                        groupedInventoryItemToRemove.Quantity--;
                    }
                }
            }
        }

        public bool HasAllTheseItems(IEnumerable<ItemQuantity> items)
        {
            return items.All(item => Items.Count(i => i.ItemTypeID == item.ItemID) >= item.Quantity);
        }
    }
}
