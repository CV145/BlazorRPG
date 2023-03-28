using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Monster : LivingEntity
    {
        public Monster(int id, string name, string imageName, int dex, int str, int ac,
                       int maximumHitPoints, GameItem currentWeapon,
                       int rewardExperiencePoints, int gold) :
            base(id, name, dex, str, ac, maximumHitPoints, maximumHitPoints, gold)
        {
            ImageName = imageName;
            CurrentWeapon = currentWeapon;
            RewardExperiencePoints = rewardExperiencePoints;
        }

        public string ImageName { get; } = string.Empty;

        public int RewardExperiencePoints { get; }
    }
}
