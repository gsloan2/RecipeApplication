using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesApp
{
    public class Recipe
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Steps { get; set; }
    }

}
