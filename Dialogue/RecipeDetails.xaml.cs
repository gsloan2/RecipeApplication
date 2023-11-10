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

            // Assuming Ingredients is a string where ingredients are separated by newlines.
            var ingredientLines = recipe.Ingredients.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            ListItems = new ObservableCollection<ListItem>(
                ingredientLines.Select(ingredient => new ListItem { Text = ingredient })
            );
            DisplayRecipeDetails(recipe);

            ListItems.Add(new ListItem());
        }


        private void DisplayRecipeDetails(Recipe recipe)
        {
            RecipeNameTextBox.Text = recipe.Title;


            CategoryComboBox.ItemsSource = Categories;
            CategoryComboBox.SelectedItem = Categories.FirstOrDefault(c => c.Id == recipe.CategoryId);

            // Assuming Ingredients is a string and each ingredient is separated by Environment.NewLine
            IngredientsItemsControl.ItemsSource = recipe.Ingredients
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(ingredient => new ListItem { Text = ingredient });

            // Assuming Steps is a string where each step is separated by Environment.NewLine
            InstructionsTextBox.Text = recipe.Steps;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Assuming 'Categories' is an ObservableCollection<Category> that includes all categories
            // and '_originalRecipe' is the recipe being edited

            // Find the category that the original recipe belongs to by CategoryId
            Category lastCategory = Categories.FirstOrDefault(c => c.Id == _originalRecipe.CategoryId);
            if (lastCategory != null)
            {
                // Remove the original recipe from the last category's Recipes collection
                lastCategory.Recipes.Remove(_originalRecipe);
            }

            // Gather the edited details
            string recipeName = RecipeNameTextBox.Text;
            Category newCategory = CategoryComboBox.SelectedItem as Category;

            // Join the ingredients and instructions into strings separated by newline
            string ingredients = string.Join(Environment.NewLine, ListItems.Select(item => item.Text));
            string instructions = string.Join(Environment.NewLine, InstructionsTextBox.Text
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            // Update the original Recipe object with the edited details
            _originalRecipe.Title = recipeName;
            _originalRecipe.CategoryId = newCategory?.Id ?? 0; // Assuming there is no Category with Id 0
            _originalRecipe.Ingredients = ingredients;
            _originalRecipe.Steps = instructions;

            EditedRecipe = new Recipe
            {
                Title = recipeName,
                CategoryId = newCategory?.Id ?? 0,
                Ingredients = ingredients,
                Steps = instructions
            };

            // If the new category is different, add the recipe to the new category's Recipes collection
            if (newCategory != null && newCategory != lastCategory)
            {
                newCategory.Recipes.Add(_originalRecipe);
            }


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
