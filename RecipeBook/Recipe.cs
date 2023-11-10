using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;

namespace RecipesApp
{
    public class Recipe : INotifyPropertyChanged
    {
        private int _id;
        private string _title;
        private int _categoryId; // This is the foreign key reference to the Category table
        private string _ingredients;
        private string _steps;

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

        public string Steps
        {
            get { return _steps; }
            set
            {
                if (_steps != value)
                {
                    _steps = value;
                    OnPropertyChanged(nameof(Steps));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
