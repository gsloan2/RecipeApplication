using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecipesApp.Dialogue
{
    /// <summary>
    /// Interaction logic for RecipeDetails.xaml
    /// </summary>
    public partial class RecipeDetails : Window
    {
        public RecipeDetails(Recipe recipe)
        {
            InitializeComponent();
            DisplayRecipeDetails(recipe);
        }

        private void DisplayRecipeDetails(Recipe recipe)
        {
            RecipeNameTextBlock.Text = recipe.Title;

            IngredientsListBox.ItemsSource = recipe.Ingredients;
            InstructionsTextBox.Text = string.Join("\n", recipe.Steps);
        }
    }
}
