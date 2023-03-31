using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories.DTOs
{
    public class MonsterTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Dex { get; set; }

        public int Str { get; set; }

        public int AC { get; set; }

        public int MaxHP { get; set; }

        public int WeaponId { get; set; }

        public int RewardXP { get; set; }

        public int Gold { get; set; }

        public string Image { get; set; } = string.Empty;

        public bool isBoss { get; }

        public IEnumerable<LootItem> LootItems { get; set; } = new List<LootItem>();
    }
}

