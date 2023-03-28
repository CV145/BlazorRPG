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
    internal static class QuestFactory
    {
        private const string _resourceNamespace = "RPG.Game.Engine.Data.quests.json";
        private static readonly IList<QuestTemplate> _questTemplates = JsonSerializationHelper.DeserializeResourceStream<QuestTemplate>(_resourceNamespace);

        public static Quest GetQuestById(int id)
        {
            // first find the quest template by its id.
            var template = _questTemplates.First(p => p.Id == id);

            // then create an instance of quest from that template.
            var quest = new Quest(template.Id, template.Name, template.Description,
                                  template.RewardGold, template.RewardXP);

            // next add each pre-requisite for the quest.
            foreach (var req in template.Requirements)
            {
                quest.ItemsToComplete.Add(new ItemQuantity { ItemID = req.Id, Quantity = req.Qty });
            }

            // finally add each reward item given from the quest.
            foreach (var item in template.RewardItems)
            {
                quest.RewardItems.Add(new ItemQuantity { ItemID = item.Id, Quantity = item.Qty });
            }

            return quest;
        }
    }
}
