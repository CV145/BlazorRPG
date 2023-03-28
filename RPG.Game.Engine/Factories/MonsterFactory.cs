using D20Tek.Common.Helpers;
using RPG.Game.Engine.Factories.DTOs;
using RPG.Game.Engine.Models;
using RPG.Game.Engine.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
    internal static class MonsterFactory
    {
        private const string _resourceNamespace = "RPG.Game.Engine.Data.monsters.json";
        private static readonly IList<MonsterTemplate> _monsterTemplates = JsonSerializationHelper.DeserializeResourceStream<MonsterTemplate>(_resourceNamespace);

        public static Monster GetMonster(int monsterId)
        {
            // first find the monster template by its id.
            var template = _monsterTemplates.First(p => p.Id == monsterId);

            // then create an instance of monster from that template.
            var weapon = ItemFactory.CreateGameItem(template.WeaponId);
            var monster = new Monster(template.Id, template.Name, template.Image, template.Dex, template.Str,
                                      template.AC, template.MaxHP, weapon, template.RewardXP, template.Gold);

            // finally add random loot for this monster instance.
            foreach (var loot in template.LootItems)
            {
                AddLootItem(monster, loot.Id, loot.Perc);
            }

            return monster;
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (DiceService.RollD(100) <= percentage)
            {
                monster.Inventory.AddItem(item: ItemFactory.CreateGameItem(itemID));
            }
        }
    }
}
