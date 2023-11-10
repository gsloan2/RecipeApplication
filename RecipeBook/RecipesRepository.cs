using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System;

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
                    recipe.Steps
                }).Single();
            }
        }

        public List<Recipe> GetAllRecipes()
        {
            using (var db = Connection)
            {
                db.Open();
                var sql = @"
                    SELECT r.RecipeID as Id, r.Title, r.CategoryID, r.Ingredients, r.Instructions,
                           c.CategoryID, c.Name
                    FROM Recipes r
                    INNER JOIN Categories c ON r.CategoryID = c.CategoryID";

                var recipes = db.Query<Recipe, Category, Recipe>(sql, (recipe, category) =>
                {
                    recipe.CategoryId = category.Id;
                    return recipe;
                }, splitOn: "CategoryID").ToList();

                return recipes;
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
                    Instructions = recipe.Steps,
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
                        // Get the CategoryID for the "Unassigned" category
                        var unassignedCategory = db.QuerySingleOrDefault<Category>(
                            "SELECT CategoryID FROM Categories WHERE Name = 'Unassigned'",
                            transaction: transaction
                        );

                        // If there's no "Unassigned" category, throw an exception or handle accordingly
                        if (unassignedCategory == null)
                        {
                            throw new InvalidOperationException("The 'Unassigned' category does not exist.");
                        }

                        // Reassign recipes to the "Unassigned" category
                        var reassignSql = @"
                            UPDATE Recipes
                            SET CategoryID = @UnassignedCategoryId
                            WHERE CategoryID = @CategoryId";
                        db.Execute(reassignSql,
                            new { UnassignedCategoryId = unassignedCategory.Id, CategoryId = categoryId },
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
