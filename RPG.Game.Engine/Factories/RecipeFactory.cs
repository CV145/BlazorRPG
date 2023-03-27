using RPG.Game.Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG.Game.Engine.Factories
{
    internal static class RecipeFactory
    {
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        static RecipeFactory()
        {
            Recipe granolaBar = new Recipe(1, "Granola bar recipe");
            granolaBar.AddIngredient(3001, 1);
            granolaBar.AddIngredient(3002, 1);
            granolaBar.AddIngredient(3003, 1);
            granolaBar.AddOutputItem(2001, 1);

            _recipes.Add(granolaBar);
        }

        public static Recipe GetRecipeById(int id)
        {
            Console.WriteLine("Recipe ID to grab: " + id);
            Console.WriteLine("All recipes:");
            _recipes.ForEach(Console.WriteLine);

            //First such that x's id matches given id
            return _recipes.First(x => x.Id == id) ?? throw new NullReferenceException();
        }
    }
}
