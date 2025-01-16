using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP110_FactorioCalculator
{
	class Program
	{
		static void Main(string[] args)
		{
			string productName = "automation-science-pack";
			// Other names to try: "logistic-science-pack", "military-science-pack", "chemical-science-pack", "production-science-pack", "utility-science-pack", "atomic-bomb", "spidertron"

			Console.WriteLine($"Time: {Calculator.GetTotalAssemblyTimeForProduct(productName)} seconds");

			Dictionary<string, double> ingredients = Calculator.GetRawMaterialsForProduct(productName);

			foreach (var keyValue in ingredients)
			{
				Console.WriteLine($"{keyValue.Key} : {keyValue.Value}");
			}

			Console.WriteLine();
			Console.WriteLine("Press any key to exit");
			Console.ReadKey();
		}
	}
}
