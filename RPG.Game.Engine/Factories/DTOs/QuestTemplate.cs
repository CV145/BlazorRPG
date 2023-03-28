using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories.DTOs
{
    //Maps to JSON file elements
    public class QuestTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public IEnumerable<IdQuantityItem> Requirements { get; set; } = new List<IdQuantityItem>();

        public int RewardGold { get; set; }

        public int RewardXP { get; set; }

        public IEnumerable<IdQuantityItem> RewardItems { get; set; } = new List<IdQuantityItem>();
    }
}
