using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;

namespace RecipesApp.DataAccess
{
    public class RecipesRepository
    {
        private readonly string _connectionString;

        public RecipesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection => new SQLiteConnection(_connectionString);

        public ObservableCollection<Category> Load()
        {
            var categories = GetAllCategories();
            var recipes = GetAllRecipes();

            // Log categories and recipes count for debugging
            Console.WriteLine($"Loaded {categories.Count} categories and {recipes.Count} recipes");

            var categoriesCollection = new ObservableCollection<Category>(categories);

            foreach (var recipe in recipes)
            {
                var category = categoriesCollection.FirstOrDefault(c => c.Id == recipe.CategoryId);

                if (category != null)
                {
                    category.Recipes.Add(recipe);
                }
                else
                {
                    // Log an error if the category is not found
                    Console.WriteLine($"Category not found for recipe ID {recipe.Id} with CategoryID {recipe.CategoryId}");
                }
            }

            return categoriesCollection;
        }


        public void InsertCategory(Category category)
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = "INSERT INTO Categories (Name) VALUES (@Name); SELECT last_insert_rowid();";
                category.Id = db.Query<int>(sql, new { Name = category.Name }).Single();
            }
        }

        public void InsertRecipe(Recipe recipe)
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = @"
                    INSERT INTO Recipes (CategoryID, Title, Ingredients, Instructions) 
                    VALUES (@CategoryId, @Title, @Ingredients, @Instructions);
                    SELECT last_insert_rowid();";

                recipe.Id = db.Query<int>(sql, new
                {
                    recipe.CategoryId,
                    recipe.Title,
                    recipe.Ingredients,
                    recipe.Instructions
                }).Single();
            }
        }

        public List<Recipe> GetAllRecipes()
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = "SELECT RecipeID as Id, CategoryID, Title, Ingredients, Instructions FROM Recipes";
                return db.Query<Recipe>(sql).ToList();

            }
        }




        private List<Category> GetAllCategories()
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = "SELECT CategoryID as Id, Name FROM Categories";
                return db.Query<Category>(sql).ToList();
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = "UPDATE Categories SET Name = @Name WHERE CategoryID = @Id";
                db.Execute(sql, new { Name = category.Name, Id = category.Id });
            }
        }

        public void UpdateRecipe(Recipe recipe)
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = @"
                    UPDATE Recipes 
                    SET Title = @Title, 
                        Ingredients = @Ingredients, 
                        Instructions = @Instructions,
                        CategoryID = @CategoryId
                    WHERE RecipeID = @Id";
                db.Execute(sql, new
                {
                    Title = recipe.Title,
                    Ingredients = recipe.Ingredients,
                    Instructions = recipe.Instructions,
                    CategoryId = recipe.CategoryId,
                    Id = recipe.Id
                });
            }
        }

        public void DeleteRecipe(int recipeId)
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = "DELETE FROM Recipes WHERE RecipeID = @Id";
                db.Execute(sql, new { Id = recipeId });
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var db = Connection)
            {
                db.Open();
                // Start a transaction
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {

                        int unassignedCategoryId = 1;

                        // Reassign recipes to the "Unassigned" category
                        var reassignSql = @"
                    UPDATE Recipes
                    SET CategoryID = @UnassignedCategoryId
                    WHERE CategoryID = @CategoryId";
                        db.Execute(reassignSql,
                            new { UnassignedCategoryId = unassignedCategoryId, CategoryId = categoryId },
                            transaction: transaction
                        );

                        // Delete the category
                        var deleteSql = "DELETE FROM Categories WHERE CategoryID = @Id";
                        db.Execute(deleteSql, new { Id = categoryId }, transaction: transaction);

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch
                    {
                        // If something goes wrong, rollback the transaction
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


    }
}
