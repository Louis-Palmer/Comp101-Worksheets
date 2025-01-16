using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace COMP110_FactorioCalculator
{
	// Stores a database of Factorio recipes
	// This parses a JSON file written by the "Data Exporter to JSON" mod, https://mods.factorio.com/mod/recipelister
	static class Recipes
	{
		// Stores the actual recipes
		private static Dictionary<string, Recipe> s_recipes;

		// Find the recipe.json file path
		private static string FindRecipeJsonPath()
		{
			DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
			while (directory != null)
			{
				string jsonPath = Path.Combine(directory.FullName, "recipe.json");
				if (File.Exists(jsonPath))
					return jsonPath;

				directory = directory.Parent;
			}

			throw new FileNotFoundException("Failed to find recipe.json");
		}

		// Static constructor, which initialises the database
		static Recipes()
		{
			// Load the text from file
			string jsonText = File.ReadAllText(FindRecipeJsonPath());

			// Parse the loaded file content
			JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() } };
			s_recipes = JsonConvert.DeserializeObject<Dictionary<string, Recipe>>(jsonText, settings);
		}

		// Search for all recipes that produce the specified product
		private static IEnumerable<Recipe> FindRecipesForProduct(string product)
		{
			return s_recipes.Values.Where(r => r.Products.Any(p => p.Name == product));
		}

		// Get the ingredients required for a particular product
		// Returns a dictionary whose keys are products and values are quantities
		public static Dictionary<string, double> FindIngredientsForProduct(string product)
		{
			// Fail on raw materials
			if (IsRawMaterial(product))
				throw new InvalidOperationException("Cannot get ingredients for a raw material");

			// Create a dictionary to store the result
			Dictionary<string, double> result = new Dictionary<string, double>();

			// Find the recipe -- this will throw an exception if there isn't exactly 1 recipe
			Recipe recipe = FindRecipesForProduct(product).Single();

			// Find out how many products are crafted by this recipe
			// (usually 1, but some recipes e.g. copper wire give more than 1 product)
			double productAmount = recipe.Products.Single(p => p.Name == product).Amount;

			// Go through the recipe, populating the dictionary
			foreach (Product ingredient in recipe.Ingredients)
			{
				result.Add(ingredient.Name, ingredient.Amount / productAmount);
			}

			return result;
		}

		// Get the time in seconds taken to craft a particular product
		public static double FindAssemblyTimeForProduct(string product)
		{
			// Fail on raw materials
			if (IsRawMaterial(product))
				throw new InvalidOperationException("Cannot get assembly time for a raw material");

			// Find the recipe -- this will throw an exception if there isn't exactly 1 recipe
			Recipe recipe = FindRecipesForProduct(product).Single();

			// Find out how many products are crafted by this recipe
			// (usually 1, but some recipes e.g. copper wire give more than 1 product)
			double productAmount = recipe.Products.Single(p => p.Name == product).Amount;

			// Return the time divided by product amount
			// E.g. copper wire takes 0.5 seconds and produces 2 products, so for our purposes it takes 0.5 / 2 = 0.25s to assemble 1 copper wire
			return recipe.Energy / productAmount;
		}

		// Determine whether the given product is classed as a "raw material"
		// This is a product which either has multiple recipes crafting it, or has no recipe, or cannot be crafted in the player's inventory
		public static bool IsRawMaterial(string product)
		{
			List<Recipe> recipes = FindRecipesForProduct(product).ToList();
			return recipes.Count != 1 || recipes[0].Category != "crafting";
		}

		// Product class, which is deserialised from JSON
		class Product
		{
			// The name of the product (this is the internal name in Factorio)
			public string Name { get; set; }

			// The quantity of the product that is consumed or produced by the recipe
			public double Amount { get; set; }
		}

		// Recipe class, which is deserialised from JSON
		class Recipe
		{
			// The name of the recipe 
			public string Name { get; set; }

			// The category, which specifies which machines can craft this recipe
			public string Category { get; set; }

			// The game files call this "energy", but it's actually the crafting time in seconds
			public double Energy { get; set; }

			// An array of ingredients consumed by the recipe
			public Product[] Ingredients { get; set; }

			// An array of products produced by the recipe
			// Most recipes have a single product, but a few (e.g. those relating to oil processing) produce multiple products
			public Product[] Products { get; set; }
		}
	}
}
