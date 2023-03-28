using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories.DTOs
{
    public class RecipeTemplate
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public IEnumerable<IdQuantityItem> Ingredients { get; set; } = new List<IdQuantityItem>();

        public IEnumerable<IdQuantityItem> OutputItems { get; set; } = new List<IdQuantityItem>();
    }
}
