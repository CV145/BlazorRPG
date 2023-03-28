using D20Tek.Common.Helpers;
using RPG.Game.Engine.Factories.DTOs;
using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
    internal static class TraderFactory
    {
        private const string _resourceNamespace = "RPG.Game.Engine.Data.traders.json";
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            IList<TraderTemplate> traderTemplates = JsonSerializationHelper.DeserializeResourceStream<TraderTemplate>(_resourceNamespace);
            foreach (var template in traderTemplates)
            {
                var trader = new Trader(template.Id, template.Name);

                foreach (var item in template.Inventory)
                {
                    for (int i = 0; i < item.Qty; i++)
                    {
                        trader.Inventory.AddItem(ItemFactory.CreateGameItem(item.Id));
                    }
                }

                _traders.Add(trader);
            }
        }

        public static Trader GetTraderById(int id) => _traders.First(t => t.Id == id);
    }
}
