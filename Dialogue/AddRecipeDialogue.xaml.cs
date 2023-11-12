using RecipesApp.RecipeBook;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ListItem = RecipesApp.RecipeBook.ListItem;

namespace RecipesApp.Dialogue
{

    public partial class AddRecipeDialogue : Window
    {

        public string RecipeName { get; private set; }
        public ObservableCollection<ListItem> ListItems { get; set; }

        public AddRecipeDialogue(ObservableCollection<Category> categories)
        {
            InitializeComponent();
            ListItems = new ObservableCollection<ListItem>();
            ListItems.Add(new ListItem()); 
            DataContext = this;
            CategoryComboBox.ItemsSource = categories;
        }

        public AddRecipeDialogue(ObservableCollection<Category> categories, Recipe recipe)
        {
            InitializeComponent();
            ListItems = new ObservableCollection<ListItem>();
            DataContext = this;

            RecipeNameTextBox.Text = recipe.Title;

            CategoryComboBox.ItemsSource = categories;

            ListItems.Clear();
            if(recipe.Ingredients.Length > 0)
            {
                foreach (var ingredient in recipe.Ingredients.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ListItems.Add(new ListItem { Text = ingredient });
                }
            }
            

            
            ListItems.Add(new ListItem());

            InstructionsTextBox.Text = recipe.Instructions;

            CategoryComboBox.ItemsSource = categories;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(RecipeNameTextBox.Text) || string.Equals("Recipe Name", RecipeNameTextBox.Text))
            {
                MessageBox.Show("Please enter a recipe name.");
                return;
            }

            if(string.IsNullOrWhiteSpace(CategoryComboBox.Text))
            {

            }
            

            RecipeName = RecipeNameTextBox.Text;
            this.DialogResult = true;
        }
        private void CancelButton_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = false;
        }



        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {



            var textBox = (TextBox)sender;
            var listItem = (ListItem)textBox.DataContext;

            if (!string.IsNullOrWhiteSpace(listItem.Text) && ListItems.IndexOf(listItem) == ListItems.Count - 1)
            {
                ListItems.Add(new ListItem());
            }

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


            if (string.IsNullOrEmpty(textBox.Text) && ListItems.Count > 1)
            {

                ListItems.Remove(listItem);
            }
            else if (!string.IsNullOrEmpty(textBox.Text) && ListItems.Last() == listItem)
            {

                ListItems.Add(new ListItem());
            }
        }

        private void RecipeNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (RecipeNameTextBox.Text == "Recipe Name")
            {
                RecipeNameTextBox.Text = "";
            }
        }

        private void RecipeNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RecipeNameTextBox.Text))
            {
                RecipeNameTextBox.Text = "Recipe Name";
            }
        }


    }
}
