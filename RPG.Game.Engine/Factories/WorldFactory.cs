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
    internal static class WorldFactory
    {
        private const string _resourceNamespace = "RPG.Game.Engine.Data.locations.json";

        internal static World CreateWorld()
        {
            var locationTemplates = JsonSerializationHelper.DeserializeResourceStream<LocationTemplate>(_resourceNamespace);
            var newWorld = new World();

            foreach (var template in locationTemplates)
            {
                var trader = (template.TraderId is null) ? null : TraderFactory.GetTraderById(template.TraderId.Value);
                var loc = new Location(template.X, template.Y, template.Name, template.Description,
                                       template.ImageName, trader);

                foreach (var questId in template.Quests)
                {
                    loc.QuestsAvailableHere.Add(QuestFactory.GetQuestById(questId));
                }

                foreach (var enc in template.Monsters)
                {
                    loc.AddMonsterEncounter(enc.Id, enc.Perc);
                }

                newWorld.AddLocation(loc);
            }

            return newWorld;
        }
    }
}
