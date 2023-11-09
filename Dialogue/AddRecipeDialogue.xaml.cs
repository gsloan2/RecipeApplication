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
    /// <summary>
    /// Interaction logic for AddRecipeDialogue.xaml
    /// </summary>
    public partial class AddRecipeDialogue : Window
    {

        public string RecipeName { get; private set; }
        public ObservableCollection<ListItem> ListItems { get; set; }

        public AddRecipeDialogue(ObservableCollection<Category> categories)
        {
            InitializeComponent();
            ListItems = new ObservableCollection<ListItem>();
            ListItems.Add(new ListItem()); // Start with one empty item
            DataContext = this;
            CategoryComboBox.ItemsSource = categories;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(RecipeNameTextBox.Text) || string.Equals("Recipe Name", RecipeNameTextBox.Text))
            {
                MessageBox.Show("Please enter a recipe name.");
                return;
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
            if (string.IsNullOrEmpty(listItem.Text))
            {
                // Add a new empty item to the list if the current item is empty and the user starts typing
                ListItems.Add(new ListItem());
            }
        }

    }
}
