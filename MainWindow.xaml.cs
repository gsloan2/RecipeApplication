using RecipesApp;
using RecipesApp.Dialogue;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.SQLite;
using RecipesApp.DataAccess;

namespace RecipesApp
{
    public partial class MainWindow : Window
    {

        private RecipesRepository DataAccess { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        public Category SelectedCategory { get; set; }  

        public Recipe SelectedRecipe { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecipeDatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            DataAccess = new RecipesRepository(connectionString);

            Categories = new ObservableCollection<Category>();

            CategoryList.ItemsSource = Categories;
            RecipeList.ItemsSource = Categories[0].Recipes; // Display first category's recipes by default
            SelectedCategory = Categories[0];
        }

        private void CategoryList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            SelectedCategory = CategoryList.SelectedItem as Category;
            RecipeList.ItemsSource = SelectedCategory?.Recipes;
        }

        private void RecipeList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedRecipe = RecipeList.SelectedItem as Recipe;
        }

        private void RecipeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RecipeList.SelectedItem is Recipe selectedRecipe)
            {
                // Open the detail window, passing in the selected recipe and the available categories
                RecipeDetails detailWindow = new RecipeDetails(selectedRecipe, Categories);
                if (detailWindow.ShowDialog() == true)
                {
                    // If saved, update the recipe details
                    var updatedRecipe = detailWindow.EditedRecipe;
                    selectedRecipe.Title = updatedRecipe.Title;
                    selectedRecipe.CategoryId = updatedRecipe.CategoryId;
                    selectedRecipe.Ingredients = updatedRecipe.Ingredients;
                    selectedRecipe.Steps = updatedRecipe.Steps;

                    DataAccess.UpdateRecipe(selectedRecipe);
                }
            }
        }

        private void CategoryList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SelectedCategory.Name.Equals("Uncategorized"))
            {
                MessageBox.Show("You cannot alter this category", "Alert", MessageBoxButton.OK);
                return;
            }

            EditCategoryDialogue dialog = new EditCategoryDialogue(SelectedCategory.Name);
            if (dialog.ShowDialog() == true)
            {
                // Update the category name
                SelectedCategory.Name = dialog.CategoryNameTextBox.Text;

                //Update category name in database
                DataAccess.UpdateCategory(SelectedCategory);

            }
        }



        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategoryDialogue dialog = new AddCategoryDialogue();

            if (dialog.ShowDialog() == true) // ShowDialog() is a blocking call that will display the dialog
            {
                string categoryName = dialog.CategoryName;

                // Add the new category to your collection
                Categories.Add(new Category { Name = categoryName });
            }
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCategory.Name.Equals("Uncategorized"))
            {
                MessageBox.Show("You cannot alter this category", "Alert", MessageBoxButton.OK);
                return;
            }

            EditCategoryDialogue dialog = new EditCategoryDialogue(SelectedCategory.Name);
            if (dialog.ShowDialog() == true)
            {
                // Update the category name
                SelectedCategory.Name = dialog.CategoryNameTextBox.Text;

                //Update category name in database
                DataAccess.UpdateCategory(SelectedCategory);

            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedCategory.Name.Equals("Uncategorized"))
            {
                MessageBox.Show("You cannot delete this category", "Alert", MessageBoxButton.OK);
                return;
            }
            if (SelectedCategory != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataAccess.DeleteCategory(SelectedCategory.Id);
                    Categories.Remove(SelectedCategory);
                    RecipeList.ItemsSource = null; 
                }
            }
            else
            {
                MessageBox.Show("Please select a category to delete");
            }
            
        }


        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            AddRecipeDialogue dialog = new AddRecipeDialogue(Categories);

            if (dialog.ShowDialog() == true)
            {
                string recipeTitle = dialog.RecipeNameTextBox.Text;
                string categoryName = dialog.CategoryComboBox.Text;
                string ingredients = string.Join(Environment.NewLine, dialog.ListItems
                                                        .Where(item => !string.IsNullOrWhiteSpace(item.Text))
                                                        .Select(item => item.Text));
                string instructions = string.Join(Environment.NewLine, dialog.InstructionsTextBox.Text
                                                        .Split(new[] { Environment.NewLine }, StringSplitOptions.None));

                // Find the Category object based on the selected category name
                Category category = Categories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

                // If the category is not found, default to the "Uncategorized" category
                if (category == null)
                {
                    category = Categories.FirstOrDefault(c => c.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase));
                }

                // If "Uncategorized" category also not found, handle the error accordingly
                if (category == null)
                {
                    // Handle error: Neither the selected category nor the "Uncategorized" category exists
                    throw new InvalidOperationException("The selected category does not exist and no 'Uncategorized' category is available.");
                }

                // Create a new Recipe object with the CategoryID from the found category
                Recipe recipe = new Recipe
                {
                    Title = recipeTitle,
                    CategoryId = category.Id, // Use CategoryId instead of Category name
                    Ingredients = ingredients,
                    Steps = instructions
                };

                // Add recipe to the category's Recipes collection
                category.Recipes.Add(recipe);

                // Insert new recipe into database
                DataAccess.InsertRecipe(recipe);

            }
        }


        private void EditRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipeList.SelectedItem is Recipe selectedRecipe)
            {
                // Open the detail window, passing in the selected recipe and the available categories
                RecipeDetails detailWindow = new RecipeDetails(selectedRecipe, Categories);
                if (detailWindow.ShowDialog() == true)
                {
                    // If saved, update the recipe details
                    var updatedRecipe = detailWindow.EditedRecipe;
                    selectedRecipe.Title = updatedRecipe.Title;
                    selectedRecipe.CategoryId = updatedRecipe.CategoryId;
                    selectedRecipe.Ingredients = updatedRecipe.Ingredients;
                    selectedRecipe.Steps = updatedRecipe.Steps;

                    DataAccess.UpdateRecipe(selectedRecipe);
                }
            }
        }

        private void DeleteRecipeButton_Click(object sender, RoutedEventArgs e)
        {

            if (SelectedRecipe != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this recipe?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataAccess.DeleteRecipe(SelectedRecipe.Id);
                    SelectedCategory.Recipes.Remove(SelectedRecipe);
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe to delete");
            }
        }
    }
}
