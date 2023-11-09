using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using RecipesApp.RecipeBook;
using System.Windows.Controls;
using System.Windows.Input;

namespace RecipesApp.Dialogue
{
    public partial class RecipeDetails : Window
    {
        public ObservableCollection<Category> Categories { get; set; }
        private Recipe _originalRecipe;
        public ObservableCollection<ListItem> ListItems { get; set; }
        public Recipe EditedRecipe { get; private set; }

        public RecipeDetails(Recipe recipe, ObservableCollection<Category> categories)
        {
            InitializeComponent();
            _originalRecipe = recipe;
            Categories = categories;
            ListItems = new ObservableCollection<ListItem>(
                recipe.Ingredients.Select(ingredient => new ListItem { Text = ingredient })
            );
            DisplayRecipeDetails(recipe);

            ListItems.Add(new ListItem());
        }

        private void DisplayRecipeDetails(Recipe recipe)
        {
            RecipeNameTextBox.Text = recipe.Title;

            // Set the selected item by finding the category in the collection
            CategoryComboBox.ItemsSource = Categories;
            CategoryComboBox.SelectedItem = Categories.FirstOrDefault(c => c.Name == recipe.Category);

            // Set up the ingredients
            IngredientsItemsControl.ItemsSource = ListItems;

            // Fill the instructions textbox
            InstructionsTextBox.Text = string.Join(Environment.NewLine, recipe.Steps);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            Category lastCategory = Categories.FirstOrDefault(c => c.Name.Equals(_originalRecipe.Category, System.StringComparison.OrdinalIgnoreCase));
            lastCategory.Recipes.Remove(_originalRecipe);



            // Gather the edited details
            string recipeName = RecipeNameTextBox.Text;
            Category newCategory = CategoryComboBox.SelectedItem as Category;
            string categoryName = newCategory?.Name ?? string.Empty;
            List<string> ingredients = ListItems.Select(item => item.Text).ToList();
            List<string> instructions = InstructionsTextBox.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            // Create a new Recipe object with the edited details (or update the original)
            EditedRecipe = new Recipe
            {
                Title = recipeName,
                Category = categoryName,
                Ingredients = ingredients,
                Steps = instructions
            };

            newCategory.Recipes.Add(EditedRecipe);

            // Set the dialog result to true to indicate success
            this.DialogResult = true;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {



            var textBox = (TextBox)sender;
            var listItem = (ListItem)textBox.DataContext;

            // Check if the text is not null or whitespace, and if it's the last item in the collection
            if (!string.IsNullOrWhiteSpace(listItem.Text) && ListItems.IndexOf(listItem) == ListItems.Count - 1)
            {
                // Add a new empty item to the list if this is the last item and it is not empty
                ListItems.Add(new ListItem());
            }

            // Remove any empty items except for the last one
            for (int i = ListItems.Count - 2; i >= 0; i--)
            {
                if (string.IsNullOrWhiteSpace(ListItems[i].Text))
                {
                    ListItems.RemoveAt(i);
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var listItem = (ListItem)textBox.DataContext;

            // If the user deletes all text, don't add a new ListItem
            if (string.IsNullOrEmpty(textBox.Text) && ListItems.Count > 1)
            {
                // Remove the current ListItem if it's not the only one and it's empty
                ListItems.Remove(listItem);
            }
            else if (!string.IsNullOrEmpty(textBox.Text) && ListItems.Last() == listItem)
            {
                // Add a new empty item if this is the last item and it's not empty
                ListItems.Add(new ListItem());
            }
        }
    }
}
