using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Monster : LivingEntity
    {
        public string ImageName { get; set; } = string.Empty;

        public int RewardExperiencePoints { get; set; }
        public int DamageRoll { get; set; } 
    }
}
