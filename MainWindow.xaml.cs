using RecipesApp;
using RecipesApp.Dialogue;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RecipesApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Category> Categories { get; set; }

        public Category SelectedCategory { get; set; }  

        public Recipe SelectedRecipe { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            Categories = new ObservableCollection<Category>
            {
                new Category { Name = "Desserts", Recipes = new ObservableCollection<Recipe>
                    {
                        new Recipe
                        {
                            Title = "Chocolate Cake",
                            Category = "Desserts",
                            Ingredients = new List<string>
                            {
                                "1-3/4 cups all-purpose flour",
                                "2 cups sugar",
                                "3/4 cup cocoa powder",
                                "1-1/2 teaspoons baking powder",
                                "1-1/2 teaspoons baking soda",
                                "1 teaspoon salt",
                                "2 eggs",
                                "1 cup whole milk",
                                "1/2 cup vegetable oil",
                                "2 teaspoons vanilla extract",
                                "1 cup boiling water"
                            },
                            Steps = new List<string>
                            {
                                "Preheat oven to 350 degrees F (175 degrees C).",
                                "Grease and flour two nine-inch round pans.",
                                "In a large bowl, stir together the sugar, flour, cocoa, baking powder, baking soda, and salt.",
                                "Add the eggs, milk, oil, and vanilla, and mix for 2 minutes on medium speed of mixer.",
                                "Stir in the boiling water last. Batter will be thin. Pour evenly into the prepared pans.",
                                "Bake 30 to 35 minutes in the preheated oven, until the cake tests done with a toothpick.",
                                "Cool in the pans for 10 minutes, then remove to a wire rack to cool completely."
                            }
                        }

                    }
                },
                new Category { Name = "Uncategorized", Recipes = new ObservableCollection<Recipe>
                    {

                    }
                },
                // Add more categories
            };

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
                    selectedRecipe.Category = updatedRecipe.Category;
                    selectedRecipe.Ingredients = updatedRecipe.Ingredients;
                    selectedRecipe.Steps = updatedRecipe.Steps;

                    
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
                SelectedCategory.Name = dialog.CategoryNameTextBox.Text;
                foreach (Recipe recipe in SelectedCategory.Recipes) {
                    recipe.Category = SelectedCategory.Name;
                }
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
            if(dialog.ShowDialog() == true)
            {
                SelectedCategory.Name = dialog.CategoryNameTextBox.Text;
                foreach (Recipe recipe in SelectedCategory.Recipes)
                {
                    recipe.Category = SelectedCategory.Name;
                }
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

            if(dialog.ShowDialog() == true)
            {
                string RecipeName = dialog.RecipeNameTextBox.Text;
                string categoryName = dialog.CategoryComboBox.Text;
                List<string> ingredients = dialog.ListItems
                                         .Where(item => !string.IsNullOrWhiteSpace(item.Text))
                                         .Select(item => item.Text)
                                         .ToList();
                List<string> instructions = dialog.InstructionsTextBox.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

                Recipe recipe = new Recipe
                {
                    Title = RecipeName,
                    Category = categoryName,
                    Ingredients = ingredients,
                    Steps = instructions
                };

                Category category = Categories.FirstOrDefault(c => c.Name.Equals(categoryName, System.StringComparison.OrdinalIgnoreCase));

                //add recipe to the category if category exists
                if(category != null)
                {
                    category.Recipes.Add(recipe);
                } else
                {
                    Category uncategorized = Categories.FirstOrDefault(c => c.Name.Equals("Uncategorized", System.StringComparison.OrdinalIgnoreCase));
                    uncategorized.Recipes.Add(recipe);
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
                    selectedRecipe.Category = updatedRecipe.Category;
                    selectedRecipe.Ingredients = updatedRecipe.Ingredients;
                    selectedRecipe.Steps = updatedRecipe.Steps;


                }
            }
        }

        private void DeleteRecipeButton_Click(object sender, RoutedEventArgs e)
        {

            if (SelectedRecipe != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this recipe?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
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
