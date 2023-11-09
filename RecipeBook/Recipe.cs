using System.Collections.Generic;
using System.ComponentModel;

namespace RecipesApp
{
    public class Recipe : INotifyPropertyChanged
    {
        private string title;
        private string category;
        private List<string> ingredients;
        private List<string> steps;

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        public List<string> Ingredients
        {
            get { return ingredients; }
            set
            {
                if (ingredients != value)
                {
                    ingredients = value;
                    OnPropertyChanged(nameof(Ingredients));
                }
            }
        }

        public List<string> Steps
        {
            get { return steps; }
            set
            {
                if (steps != value)
                {
                    steps = value;
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
