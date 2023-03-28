using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories.DTOs
{
    public class ItemTemplate
    {
        public int Id { get; set; }

        public GameItem.ItemCategory Category { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Price { get; set; }

        public int Damage { get; set; }

        public int Heals { get; set; }
    }
}
