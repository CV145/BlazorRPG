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
        private static readonly IList<Quest> _quests = new List<Quest>();

        static QuestFactory()
        {
            // Declare the items need to complete the quest, and its reward items
            List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
            List<ItemQuantity> rewardItems = new List<ItemQuantity>();

            itemsToComplete.Add(new ItemQuantity { ItemID = 9001, Quantity = 5 });
            rewardItems.Add(new ItemQuantity { ItemID = 1002, Quantity = 1 });

            // Create the quest
            _quests.Add(new Quest
            {
                Id = 1,
                Name = "Clear the herb garden",
                Description = "Defeat the snakes in the Herbalist's garden",
                ItemsToComplete = itemsToComplete,
                RewardGold = 25,
                RewardExperiencePoints = 10,
                RewardItems = rewardItems
            });
        }

        public static Quest GetQuestById(int id)
        {
            return _quests.First(quest => quest.Id == id);
        }
    }
}
