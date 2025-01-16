using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP110_FactorioCalculator
{
	public static class Calculator
	{
		public static double GetTotalAssemblyTimeForProduct(string product)
		{
			double Time = 0;

			if (!Recipes.IsRawMaterial(product)) //checks to see if its NOT a raw material
            {
				Time = Recipes.FindAssemblyTimeForProduct(product); //gets the assembly time for the product. Not the products required to make the current one.
				Dictionary<string, double> ingredients = Recipes.FindIngredientsForProduct(product); //creates a dictionary with a string and a double called ingredients and applies the ingridents of the product to it.
				
				for (int i = 0; i < ingredients.Count; i++) //is a for and goes through for every ingridient in the dictionary.
                {
					string current = ingredients.Keys.ElementAt(i); //finds the certain inredients and applies it to the current string.

					if (!Recipes.IsRawMaterial(current)) //checks if current is a NOT a raw material.
                    {
						Time = Time + ingredients.Values.ElementAt(i) * GetTotalAssemblyTimeForProduct(current); // works out the amount of products required times it by the amount of time it takes (recursion). then adds it to Time variable. for the total amount of time required
                    }
                }
			
            }

			return Time; //returns the total amount of time.
		}

		public static Dictionary<string, double> GetRawMaterialsForProduct(string product)
		{
			Dictionary<string, double> RawMaterials = new Dictionary<string, double>();


			if (!Recipes.IsRawMaterial(product))
            {
				Dictionary<string, double> ingredients = Recipes.FindIngredientsForProduct(product);


                foreach(KeyValuePair<string, double> Ingredient in ingredients)
                {
					if (Recipes.IsRawMaterial(Ingredient.Key))
					{
						if (RawMaterials.ContainsKey(Ingredient.Key))
						{
							RawMaterials[Ingredient.Key] += Ingredient.Value;
						}
						else
						{
							RawMaterials.Add(Ingredient.Key, Ingredient.Value);
						}


					}
					else
                    {
						Dictionary<string, double> Comps = GetRawMaterialsForProduct(Ingredient.Key);
						foreach (KeyValuePair<string, double> CompIngredient in Comps)
                        {
							if (RawMaterials.ContainsKey(CompIngredient.Key))
                            {
								double CompMultiplier = CompIngredient.Value * Ingredient.Value;
								RawMaterials[CompIngredient.Key] += CompMultiplier;

                            }
                            else
                            {
								double CompMultiplier = CompIngredient.Value * Ingredient.Value;
								RawMaterials.Add(CompIngredient.Key, CompMultiplier);
							}
							
                        }
					}

                }

				return RawMaterials;
			}
            else
            {
				RawMaterials.Add(product, 1);
            }

			return RawMaterials;


			
		}

	}
}
