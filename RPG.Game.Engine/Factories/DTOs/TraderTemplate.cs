using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories.DTOs
{
    public class TraderTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IdQuantityItem> Inventory { get; set; } = new List<IdQuantityItem>();
    }
}
