using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Recipe
    {
        public Recipe(int id, string name)
        {
           Id = id;
           Name = name;
        }

        public int Id { get; }

        public string Name { get; } = string.Empty;

        public IList<ItemQuantity> Ingredients { get; } = new List<ItemQuantity>();

        public IList<ItemQuantity> OutputItems { get; } = new List<ItemQuantity>();

        public void AddIngredient(int itemId, int quantity)
        {
            if (!Ingredients.Any(x => x.ItemID == itemId))
            {
                Ingredients.Add(new ItemQuantity { ItemID = itemId, Quantity = quantity });
            }
        }

        public void AddOutputItem(int itemId, int quantity)
        {
            if (!OutputItems.Any(x => x.ItemID == itemId))
            {
                OutputItems.Add(new ItemQuantity { ItemID = itemId, Quantity = quantity });
            }
        }

        public MessageBox ToDisplayMessage()
        {
            var messageLines = new List<string>
            {
                "Ingredients:"
            };

            foreach (ItemQuantity q in Ingredients)
            {
                messageLines.Add(q.QuantityItemDescription);
            }

            messageLines.Add("Creates:");
            foreach (ItemQuantity itemQuantity in OutputItems)
            {
                messageLines.Add(itemQuantity.QuantityItemDescription);
            }

            return new MessageBox($"Recipe Added - {Name}", messageLines);
        }
    }
}
