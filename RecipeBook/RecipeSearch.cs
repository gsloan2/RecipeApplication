using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesApp.RecipeBook
{
    internal class RecipeSearch
    {
        public static ObservableCollection<Recipe> searchRecipes(string filterString, ObservableCollection<Category> categories)
        {
            var result = new ObservableCollection<Recipe>();


            var filterParts = filterString.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var category in categories)
            {
                foreach (var recipe in category.Recipes)
                {
                    bool matchesAll = true;
                    foreach (var part in filterParts)
                    {

                        var orIngredients = part.Trim().Split(new string[] { " or " }, StringSplitOptions.None);
                        bool matchesOr = orIngredients.Any(ingredient => recipe.Ingredients.ToLower().Contains(ingredient.Trim()));

                        if (!matchesOr)
                        {
                            matchesAll = false;
                            break; 
                        }
                    }

                    if (matchesAll)
                    {
                        result.Add(recipe);
                    }
                }
            }

            return result;
        }



    }
}
