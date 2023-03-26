using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Player : LivingEntity
    {
        public string CharacterClass { get; set; } = string.Empty;

        public int ExperiencePoints { get; set; }
        public IList<QuestStatus> Quests { get; set; } = new List<QuestStatus>();
        public void AddExperience(int experiencePoints)
        {
            if (experiencePoints > 0)
            {
                ExperiencePoints += experiencePoints;
                SetLevelAndMaximumHitPoints();
            }
        }

        private void SetLevelAndMaximumHitPoints()
        {
            int originalLevel = Level;

            Level = (ExperiencePoints / 100) + 1;

            if (Level != originalLevel)
            {
                MaximumHitPoints = Level * 10;
            }
        }
    }
}
