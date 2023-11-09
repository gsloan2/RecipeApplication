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
