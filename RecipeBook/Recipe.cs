using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;

namespace RecipesApp
{
    public class Recipe : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private int _categoryId; // This is the foreign key reference to the Category table
        private string _ingredients;
        private string _instructions;


        public Recipe()
        {
            this.Ingredients = "";

            this.Instructions = "";
            this.Title = "";
        }

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set
            {
                if (_categoryId != value)
                {
                    _categoryId = value;
                    OnPropertyChanged(nameof(CategoryId));
                }
            }
        }

        public string Ingredients
        {
            get { return _ingredients; }
            set
            {
                if (_ingredients != value)
                {
                    _ingredients = value;
                    OnPropertyChanged(nameof(Ingredients));
                }
            }
        }

        public string Instructions
        {
            get { return _instructions; }
            set
            {
                if (_instructions != value)
                {
                    _instructions = value;
                    OnPropertyChanged(nameof(Instructions));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Id: {_id}");
            sb.AppendLine($"Title: {_title}");
            sb.AppendLine($"Category ID: {_categoryId}");
            sb.AppendLine($"Ingredients: {_ingredients}");
            sb.AppendLine($"Instructions: {_instructions}");

            return sb.ToString();
        }
    }
}
