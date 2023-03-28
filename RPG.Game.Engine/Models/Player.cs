using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Models
{
    public class Player : LivingEntity
    {
        public static readonly Player Empty = new Player(string.Empty, string.Empty, 10, 10, 10, 10, 0);

        public Player(string name, string charClass, int dex, int str, int ac,
                      int maximumHitPoints, int gold)
            : base(1, name, dex, str, ac, maximumHitPoints, maximumHitPoints, gold)
        {
            CharacterClass = charClass;
        }

        public string CharacterClass { get; } = string.Empty;

        public int ExperiencePoints { get; private set; }

        public IList<QuestStatus> Quests { get; } = new List<QuestStatus>();

        public IList<Recipe> Recipes { get; } = new List<Recipe>();
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

        public void LearnRecipe(Recipe recipe)
        {
            if (!Recipes.Any(r => r.Id == recipe.Id))
            {
                Recipes.Add(recipe);
            }
        }
    }
}
