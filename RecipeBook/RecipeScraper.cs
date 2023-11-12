using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace RecipesApp.RecipeBook
{
    internal class RecipeScraper
    {
        public async Task<Recipe> ScrapeRecipeAsync(string url)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
                var htmlDocument = new HtmlDocument();

                var html = await httpClient.GetStringAsync(url);
                htmlDocument.LoadHtml(html);
                Recipe recipe = new Recipe();

                if (url.Contains("tasteofhome"))
                {
                    recipe = TasteOfHomeScrape(htmlDocument);
                }
                else if (url.Contains("allrecipes"))
                {
                    recipe = AllRecipesScrape(htmlDocument);
                }
                else if (url.Contains("foodnetwork"))
                {
                    recipe = FoodNetworkScrape(htmlDocument);
                }
                else if (url.Contains("delish.com"))
                {
                    recipe = DelishScrape(htmlDocument);
                }
                else if (url.Contains("nytimes"))
                {
                    recipe = NYTimesScrape(htmlDocument);
                }
                else if (url.Contains("bonappetit"))
                {
                    recipe = BonAppetitScrape(htmlDocument);
                }
                else if (url.Contains("thepioneerwoman"))
                {
                    recipe = PioneerWomanScrape(htmlDocument);
                }
                else
                {
                    recipe = GenericScrape(htmlDocument);
                }

                return recipe;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error fetching HTML content from the URL: " + ex.Message);
                return new Recipe();
            }
            catch (HtmlWebException ex)
            {
                Console.WriteLine("Error parsing HTML content: " + ex.Message);
                return new Recipe();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Recipe();
            }
        }

        public Recipe PioneerWomanScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();


            var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'ingredients-body')]//li[contains(@class, 'css-1emtwfa')]");
            if (ingredientsNodes != null)
            {
                var ingredientsBuilder = new StringBuilder();
                foreach (var node in ingredientsNodes)
                {
                    var ingredient = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(ingredient))
                    {
                        ingredientsBuilder.AppendLine(ingredient);
                    }
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }

            var directionsNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'css-l74ltq')]//ol[@class='css-x4ihvu et3p2gv0']/li");
            if (directionsNodes != null)
            {
                var directionsBuilder = new StringBuilder();
                foreach (var node in directionsNodes)
                {
                    var direction = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(direction))
                    {
                        directionsBuilder.AppendLine(direction + "\n");
                    }
                }
                recipe.Instructions = directionsBuilder.ToString().TrimEnd(new char[] { '\n', '\r' });
            }

            return recipe;
        }

        public Recipe BonAppetitScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();


            var ingredientQuantityNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'List-iSNGTT')]//p[contains(@class, 'Amount-hYcAMN')]");
            var ingredientDescriptionNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'List-iSNGTT')]//div[contains(@class, 'Description-cSrMCf')]");

            if (ingredientQuantityNodes != null && ingredientDescriptionNodes != null && ingredientQuantityNodes.Count == ingredientDescriptionNodes.Count)
            {
                var ingredientsBuilder = new StringBuilder();
                for (int i = 0; i < ingredientQuantityNodes.Count; i++)
                {
                    ingredientsBuilder.AppendLine(ingredientQuantityNodes[i].InnerText.Trim() + " " + ingredientDescriptionNodes[i].InnerText.Trim());
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }

            var directionsNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'InstructionsWrapper-hZXqPx')]");
            if (directionsNode != null)
            {
                var directionsText = directionsNode.InnerText;

                var steps = Regex.Split(directionsText, @"(Step \d+)").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                var directionsBuilder = new StringBuilder();
                foreach (var step in steps)
                {
                    var trimmedStep = step.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedStep))
                    {
                        directionsBuilder.AppendLine(trimmedStep);
                        directionsBuilder.AppendLine(); 
                    }
                }
                recipe.Instructions = directionsBuilder.ToString().Trim();
            }

            return recipe;
        }

        public Recipe NYTimesScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();

            
            var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//h2[contains(text(), 'Ingredients')]/following-sibling::ul[1]//li");
            if (ingredientsNodes != null)
            {
                var ingredientsBuilder = new StringBuilder();
                foreach (var node in ingredientsNodes)
                {
                    ingredientsBuilder.AppendLine(node.InnerText.Trim());
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }

            
            var directionsNodes = htmlDocument.DocumentNode.SelectNodes("//h2[contains(text(), 'Preparation')]/following-sibling::ol[1]//li");
            if (directionsNodes != null)
            {
                var directionsBuilder = new StringBuilder();
                foreach (var node in directionsNodes)
                {
                    directionsBuilder.AppendLine(node.InnerText.Trim());
                    directionsBuilder.AppendLine(); 
                }
                recipe.Instructions = directionsBuilder.ToString().Trim();
            }

            return recipe;
        }

        public Recipe TasteOfHomeScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();


            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();

            var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//h2[contains(text(), 'Ingredients')]/following-sibling::ul[1]/li");
            if (ingredientsNodes != null)
            {
                var ingredientsBuilder = new StringBuilder();
                foreach (var node in ingredientsNodes)
                {
                    ingredientsBuilder.AppendLine(node.InnerText.Trim());
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }

            var directionsNodes = htmlDocument.DocumentNode.SelectNodes("//h2[contains(text(), 'Directions')]/following-sibling::ol[1]/li");
            if (directionsNodes != null)
            {
                var directionsBuilder = new StringBuilder();
                foreach (var node in directionsNodes)
                {
                    directionsBuilder.AppendLine(node.InnerText.Trim());
                }
                recipe.Instructions = directionsBuilder.ToString().Trim();
            }

            return recipe;
        }

        public Recipe AllRecipesScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            
            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();

            
            var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//ul[@class='mntl-structured-ingredients__list']/li");
            if (ingredientsNodes != null)
            {
                var ingredientsBuilder = new StringBuilder();
                foreach (var node in ingredientsNodes)
                {
                    ingredientsBuilder.AppendLine(node.InnerText.Trim());
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }

            
            var directionsNodes = htmlDocument.DocumentNode.SelectNodes("//ol[@class='comp mntl-sc-block-group--OL mntl-sc-block mntl-sc-block-startgroup']/li/p");
            if (directionsNodes != null)
            {
                var directionsBuilder = new StringBuilder();
                foreach (var node in directionsNodes)
                {
                    var direction = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(direction))
                    {
                        directionsBuilder.AppendLine(direction);
                    }
                }
                recipe.Instructions = directionsBuilder.ToString().Trim();
            }

            return recipe;
        }


        public Recipe FoodNetworkScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();


            var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//span[contains(text(), 'Ingredients')]/following::div[1]//p[contains(@class, 'o-Ingredients__a-Ingredient')]");
            if (ingredientsNodes != null)
            {
                var ingredientsBuilder = new StringBuilder();
                foreach (var node in ingredientsNodes)
                {
                    var ingredient = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(ingredient) && !ingredient.StartsWith("Deselect All", StringComparison.OrdinalIgnoreCase))
                    {
                        ingredientsBuilder.AppendLine(ingredient);
                    }
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }


            var directionsNodes = htmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'o-Method__m-Body')]/ol/li");
            if (directionsNodes != null)
            {
                var directionsBuilder = new StringBuilder();
                foreach (var node in directionsNodes)
                {
                    var direction = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(direction))
                    {
                        directionsBuilder.AppendLine(direction);
                    }
                }
                recipe.Instructions = directionsBuilder.ToString().Trim();
            }

            return recipe;
        }


        public Recipe DelishScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();

            
            var ingredientsNodes = htmlDocument.DocumentNode.SelectNodes("//li[contains(@class, 'css-1oxvhc1')]");
            if (ingredientsNodes != null)
            {
                var ingredientsBuilder = new StringBuilder();
                foreach (var node in ingredientsNodes)
                {
                    var ingredient = node.InnerText.Trim();
                    if (!string.IsNullOrEmpty(ingredient))
                    {
                        ingredientsBuilder.AppendLine(ingredient);
                    }
                }
                recipe.Ingredients = ingredientsBuilder.ToString().Trim();
            }

            
            var directionsNode = htmlDocument.DocumentNode.SelectSingleNode("//div[contains(@class, 'css-l74ltq')]");
            if (directionsNode != null)
            {
                var directionsBuilder = new StringBuilder();
                foreach (var liNode in directionsNode.SelectNodes(".//li"))
                {
                    var directionText = HtmlEntity.DeEntitize(liNode.InnerText.Trim());
                    if (!string.IsNullOrEmpty(directionText))
                    {
                        directionsBuilder.AppendLine(directionText);
                        
                    }
                }
                
                recipe.Instructions = directionsBuilder.ToString().TrimEnd(new char[] { '\r', '\n' });
            }

            return recipe;
        }

        public Recipe GenericScrape(HtmlDocument htmlDocument)
        {
            Recipe recipe = new Recipe();

            var recipeTitle = htmlDocument.DocumentNode.SelectSingleNode("//h1");
            recipe.Title = recipeTitle?.InnerText.Trim();


            var ingredientsNodes = ExtractNodes(htmlDocument, "ingredients");
            if (ingredientsNodes == null || !ingredientsNodes.Any())
            {

                ingredientsNodes = BackupExtractNodes(htmlDocument, "ingredients");
            }
            ProcessNodes(ingredientsNodes, recipe, isIngredients: true);


            var directionsNodes = ExtractNodes(htmlDocument, "preparation", "directions", "steps");
            if (directionsNodes == null || !directionsNodes.Any())
            {

                directionsNodes = BackupExtractNodes(htmlDocument, "directions", "steps");
            }
            ProcessNodes(directionsNodes, recipe, isIngredients: false);

            return recipe;
        }

        private List<HtmlNode> ExtractNodes(HtmlDocument doc, params string[] keywords)
        {
            var headerNodes = doc.DocumentNode.SelectNodes("//h1|//h2|//h3|//h4|//h5|//h6|//p");
            if (headerNodes == null) return new List<HtmlNode>(); 

            var relevantNodes = headerNodes
                .Where(node => keywords.Any(keyword => node.InnerText.ToLower().Contains(keyword)))
                .ToList();

            var resultNodes = new List<HtmlNode>();
            foreach (var node in relevantNodes)
            {
                var followingNodes = node.SelectNodes("following-sibling::ul[1]//li|following-sibling::ol[1]//li|following-sibling::p");
                if (followingNodes != null)
                {
                    resultNodes.AddRange(followingNodes);
                }
            }

            return resultNodes;
        }           
        

        private List<HtmlNode> BackupExtractNodes(HtmlDocument doc, params string[] keywords)
        {
            return doc.DocumentNode.SelectNodes("//ul//li|//ol//li|//p")
                        .Where(node => keywords.Any(keyword => node.InnerText.ToLower().Contains(keyword)))
                        .ToList();
        }

        private void ProcessNodes(List<HtmlNode> nodes, Recipe recipe, bool isIngredients)
        {
            if (nodes != null && nodes.Any())
            {
                var builder = new StringBuilder();
                foreach (var node in nodes)
                {
                    builder.AppendLine(node.InnerText.Trim());
                    builder.AppendLine(); 
                }
                if (isIngredients)
                    recipe.Ingredients = builder.ToString().Trim();
                else
                    recipe.Instructions = builder.ToString().Trim();
            }
        }



    }


}
