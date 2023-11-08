using RecipesApp;
using RecipesApp.Dialogue;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;

namespace RecipesApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Category> Categories { get; set; }

        public Category SelectedCategory { get; set; }  
        public MainWindow()
        {
            InitializeComponent();
            Categories = new ObservableCollection<Category>
            {
                new Category { Name = "Desserts", Recipes = new List<Recipe>
                    {
                        new Recipe { Title = "Chocolate Cake", Category = "Desserts", Ingredients = new List<string>(), Steps = new List<string>() }
                        // Add more recipes
                    }
                },
                // Add more categories
            };

            CategoryList.ItemsSource = Categories;
            RecipeList.ItemsSource = Categories[0].Recipes; // Display first category's recipes by default
        }

        private void CategoryList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            SelectedCategory = CategoryList.SelectedItem as Category;
            RecipeList.ItemsSource = SelectedCategory?.Recipes;
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
            EditCategoryDialogue dialog = new EditCategoryDialogue(SelectedCategory.Name);
            if(dialog.ShowDialog() == true)
            {
                SelectedCategory.Name = dialog.CategoryNameTextBox.Text;
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
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
            // Implement logic to add a new recipe
        }

        private void EditRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to edit the selected recipe
        }

        private void DeleteRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to delete the selected recipe
        }
    }
}
