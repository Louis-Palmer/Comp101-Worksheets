using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using COMP110_FactorioCalculator;
using System.IO;

namespace COMP110_FactorioCalculator_Tests
{
	[TestFixture]
	public class UnitTests
	{
		const double delta = 0.001;

		private void AssertDictionariesAreEqual(Dictionary<string, double> expected, Dictionary<string, double> actual)
		{
			List<string> problems = new List<string>();

			foreach (var kv in expected)
			{
				if (!actual.ContainsKey(kv.Key))
					problems.Add($"Dictionary does not contain expected key '{kv.Key}'");
				else if (Math.Abs(kv.Value - actual[kv.Key]) > delta)
					problems.Add($"Value for key '{kv.Key} does not match (expected {kv.Value}, got {actual[kv.Key]})");
			}

			foreach (var kv in actual)
			{
				if (!expected.ContainsKey(kv.Key))
					problems.Add($"Dictionary contains unexpected key '{kv.Key}'");
			}

			if (problems.Count > 0)
			{
				throw new AssertionException(string.Join("\n", problems));
			}
		}

		[TestCase("coal", new object[]{ "coal", 1 })]
		[TestCase("iron-plate", new object[] { "iron-plate", 1 })]
		[TestCase("battery", new object[] { "battery", 1 })]
		[TestCase("iron-gear-wheel", new object[] { "iron-plate", 2 })]
		[TestCase("copper-cable", new object[] { "copper-plate", 0.5 })]
		[TestCase("automation-science-pack", new object[] { "copper-plate", 1, "iron-plate", 2 })]
		[TestCase("logistic-science-pack", new object[] { "iron-plate", 5.5, "copper-plate", 1.5 })]
		[TestCase("military-science-pack", new object[] { "copper-plate", 2.5, "steel-plate", 0.5, "iron-plate", 4.5, "coal", 5, "stone-brick", 5 })]
		[TestCase("chemical-science-pack", new object[] { "sulfur", 0.5, "plastic-bar", 3, "copper-plate", 7.5, "iron-plate", 3, "engine-unit", 1 })]
		[TestCase("production-science-pack", new object[] { "stone", 5, "steel-plate", 8.33333333333333, "iron-plate", 10.8333333333333, "plastic-bar", 6.66666666666667, "copper-plate", 19.1666666666667, "stone-brick", 3.33333333333333 })]
		[TestCase("utility-science-pack", new object[] { "processing-unit", 0.666666666666667, "steel-plate", 2.33333333333333, "battery", 0.666666666666667, "iron-plate", 1, "copper-plate", 21.5, "electric-engine-unit", 0.333333333333333, "plastic-bar", 5 })]
		[TestCase("atomic-bomb", new object[] { "explosives", 10, "processing-unit", 10, "iron-plate", 150, "copper-plate", 325, "plastic-bar", 100, "uranium-235", 30 })]
		[TestCase("spidertron", new object[] { "raw-fish", 1, "processing-unit", 516, "iron-plate", 1090, "copper-plate", 7165, "plastic-bar", 1930, "steel-plate", 580, "electric-engine-unit", 120 })]
		public void TestGetRawMaterials(string product, object[] expectedArray)
		{
			Dictionary<string, double> expected = new Dictionary<string, double>();
			for (int i=0; i<expectedArray.Length; i+=2)
			{
				string k = (string)expectedArray[i];
				double v = Convert.ToDouble(expectedArray[i + 1]);
				expected[k] = v;
			}

			var actual = Calculator.GetRawMaterialsForProduct(product);
			AssertDictionariesAreEqual(expected, actual);
		}

		[TestCase("coal", 0)]
		[TestCase("iron-plate", 0)]
		[TestCase("battery", 0)]
		[TestCase("iron-gear-wheel", 0.5)]
		[TestCase("copper-cable", 0.25)]
		[TestCase("automation-science-pack", 5.5)]
		[TestCase("logistic-science-pack", 8.75)]
		[TestCase("military-science-pack", 11.5)]
		[TestCase("chemical-science-pack", 26.25)]
		[TestCase("production-science-pack", 51.1666666666667)]
		[TestCase("utility-science-pack", 34.9166666666667)]
		[TestCase("atomic-bomb", 1037.5)]
		[TestCase("spidertron", 10483.5)]
		public void TestGetAssemblyTime(string product, double expected)
		{
			Assert.AreEqual(expected, Calculator.GetTotalAssemblyTimeForProduct(product), delta);
		}
	}
}
