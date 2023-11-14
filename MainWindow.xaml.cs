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
using RecipesApp.RecipeBook;

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

            Console.WriteLine("loading data");
            Categories = DataAccess.Load();

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
                    selectedRecipe.Instructions = updatedRecipe.Instructions;

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

                Category category = new Category { Name = categoryName };
                Categories.Add(category);
                DataAccess.InsertCategory(category);
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
                    ObservableCollection<Recipe> recipes = SelectedCategory.Recipes;
                    foreach (Recipe recipe in recipes)
                    {
                        Categories[0].Recipes.Add(recipe);
                    }
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
                    Instructions = instructions
                };

                // Add recipe to the category's Recipes collection
                category.Recipes.Add(recipe);

                // Insert new recipe into database
                DataAccess.InsertRecipe(recipe);

            }
        }

        private async void ImportRecipeButton_Click(object sender, EventArgs e)
        {
            RecipeScraper scraper = new RecipeScraper();
            ImportRecipeDialogue dialogue = new ImportRecipeDialogue();

            if(dialogue.ShowDialog() == true)
            {
                string url = dialogue.UrlTextBox.Text;
                Console.WriteLine("Got this url: " + url);
                Recipe recipe = await scraper.ScrapeRecipeAsync(url);
                Console.WriteLine(recipe.ToString());

                AddRecipeDialogue dialogue2 = new AddRecipeDialogue(Categories, recipe);
                if (dialogue2.ShowDialog() == true)
                {
                    string recipeTitle = dialogue2.RecipeNameTextBox.Text;
                    string categoryName = dialogue2.CategoryComboBox.Text;
                    string ingredients = string.Join(Environment.NewLine, dialogue2.ListItems
                                                            .Where(item => !string.IsNullOrWhiteSpace(item.Text))
                                                            .Select(item => item.Text));
                    string instructions = string.Join(Environment.NewLine, dialogue2.InstructionsTextBox.Text
                                                            .Split(new[] { Environment.NewLine }, StringSplitOptions.None));


                    Category category = Categories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

                    if (category == null)
                    {
                        category = Categories.FirstOrDefault(c => c.Name.Equals("Uncategorized", StringComparison.OrdinalIgnoreCase));
                    }

            
                    if (category == null)
                    {
                   
                        throw new InvalidOperationException("The selected category does not exist and no 'Uncategorized' category is available.");
                    }

                    
                    Recipe recipeToAdd = new Recipe
                    {
                        Title = recipeTitle,
                        CategoryId = category.Id, 
                        Ingredients = ingredients,
                        Instructions = instructions
                    };

                    
                    category.Recipes.Add(recipeToAdd);

                    
                    DataAccess.InsertRecipe(recipeToAdd);

                }

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
                    selectedRecipe.Instructions = updatedRecipe.Instructions;

                    DataAccess.UpdateRecipe(selectedRecipe);
                }
            }
        }

        private void ExportRecipeButton_Click(Object sender, RoutedEventArgs e)
        {
            if(SelectedRecipe == null)
            {
                MessageBox.Show("Select a recipe to export!");
            }
            else
            {
                CreateDocument.CreateRecipeDocument(SelectedRecipe);
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


        private void HomePage_Click(object sender, RoutedEventArgs e)
        {
            RecipeList.Visibility = Visibility.Visible;
            CategoryList.Visibility = Visibility.Visible;
            HomePagePanel.Visibility = Visibility.Visible;

            SearchPanel.Visibility = Visibility.Collapsed;
            SearchResultsList.Visibility = Visibility.Collapsed;

            CategoryList.ItemsSource = Categories;
            RecipeList.ItemsSource = Categories[0].Recipes; // Display first category's recipes by default
            SelectedCategory = Categories[0];
        }

        private void SearchPage_Click(Object sender, RoutedEventArgs e)
        {
            RecipeList.Visibility = Visibility.Collapsed;
            CategoryList.Visibility = Visibility.Collapsed;
            HomePagePanel.Visibility = Visibility.Collapsed;

            SearchPanel.Visibility = Visibility.Visible;
            SearchResultsList.Visibility = Visibility.Visible;
        }

        private void SearchButton_Click(Object sender, RoutedEventArgs e)
        {
            SearchResultsList.ItemsSource = RecipeSearch.searchRecipes(SearchTextBox.Text, Categories);
            
        }


        private void Search_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Enter ingredients to search(ex: potato, butter, pepper)")
            {
                SearchTextBox.Text = "";

            }
        }

        private void Search_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Enter ingredients to search(ex: potato, butter, pepper)";
            }
        }


        private void SearchResultList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedRecipe = SearchResultsList.SelectedItem as Recipe;
        }

        private void SearchResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SearchResultsList.SelectedItem is Recipe selectedRecipe)
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
                    selectedRecipe.Instructions = updatedRecipe.Instructions;

                    DataAccess.UpdateRecipe(selectedRecipe);
                }
            }
        }
    }
}
