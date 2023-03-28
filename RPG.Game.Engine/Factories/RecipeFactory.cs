using D20Tek.Common.Helpers;
using RPG.Game.Engine.Factories.DTOs;
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
        private const string _resourceNamespace = "RPG.Game.Engine.Data.recipes.json";
        private static readonly IList<RecipeTemplate> _recipeTemplates = JsonSerializationHelper.DeserializeResourceStream<RecipeTemplate>(_resourceNamespace);

        public static Recipe GetRecipeById(int id)
        {
            // first find the quest template by its id.
            var template = _recipeTemplates.First(p => p.Id == id);

            // then create an instance of quest from that template.
            var recipe = new Recipe(template.Id, template.Name);

            // next add each pre-requisite for the quest.
            foreach (var req in template.Ingredients)
            {
                recipe.AddIngredient(req.Id, req.Qty);
            }

            // finally add each reward item given from the quest.
            foreach (var item in template.OutputItems)
            {
                recipe.AddOutputItem(item.Id, item.Qty);
            }

            return recipe;
        }
    }
}
