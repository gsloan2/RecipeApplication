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
    /// Interaction logic for EditCategoryDialogue.xaml
    /// </summary>
    public partial class EditCategoryDialogue : Window
    {

        public string NewCategoryName { get; private set; }
        public EditCategoryDialogue(String categoryName)
        {
            InitializeComponent();
            CategoryNameTextBox.Text = categoryName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            
            // Basic validation
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Please enter a category name.");
                return;
            }

            NewCategoryName = CategoryNameTextBox.Text;
            this.DialogResult = true;
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
