using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories.DTOs
{
    public class LocationTemplate
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageName { get; set; } = string.Empty;

        public int? TraderId { get; set; }

        public IEnumerable<int> Quests { get; set; } = new List<int>();

        public IEnumerable<MonsterEncounterItem> Monsters { get; set; } = new List<MonsterEncounterItem>();
    }
}
