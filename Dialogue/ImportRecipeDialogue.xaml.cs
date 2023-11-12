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
    
    public partial class ImportRecipeDialogue : Window
    {
        public ImportRecipeDialogue()
        {
            InitializeComponent();
        }

        private void UrlTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Text = "";
            textBox.Foreground = Brushes.Black; 

        }

        private void UrlTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter recipe URL here";
                textBox.Foreground = Brushes.LightGray;
            }
            

        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

    }


}
