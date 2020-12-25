using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Base;

namespace AdventOfCode2020.Day21
{
    public class Solver : ISolver
    {
        private List<IngredientsLabel> Labels;

        public Solver()
        {
            Labels = new InputReader<IngredientsLabel>(21).ReadInputAsLines().ToList();
        }

        public void SolvePartOne()
        {
            var allergens = Labels.SelectMany(l => l.Allergens()).Distinct();
            var ingredients = Labels.SelectMany(l => l.Ingredients()).Distinct();
            var potentialIngredientsPerAllergen = new Dictionary<string, List<string>>();
            foreach (var allergen in allergens)
            {
                var potentialContainers = ingredients.ToList();
                foreach (var label in Labels)
                {
                    if (label.ContainsAllergen(allergen))
                    {
                        potentialContainers = potentialContainers.Intersect(label.Ingredients()).ToList();
                    }
                }
                potentialIngredientsPerAllergen.Add(allergen, potentialContainers);
            }
            var ingredientsWithoutAllergens = ingredients.Where(i => potentialIngredientsPerAllergen.Values.All(v => !v.Contains(i)));
            var count = ingredientsWithoutAllergens.Aggregate(0, (current, next) => current + Labels.Count(l => l.ContainsIngredient(next)));
            Console.WriteLine(count);
        }

        public void SolvePartTwo()
        {
            var allergens = Labels.SelectMany(l => l.Allergens()).Distinct().ToList();
            var ingredients = Labels.SelectMany(l => l.Ingredients()).Distinct();
            var potentialIngredientsPerAllergen = new Dictionary<string, List<string>>();
            foreach (var allergen in allergens)
            {
                var potentialContainers = ingredients.ToList();
                foreach (var label in Labels)
                {
                    if (label.ContainsAllergen(allergen))
                    {
                        potentialContainers = potentialContainers.Intersect(label.Ingredients()).ToList();
                    }
                }
                potentialIngredientsPerAllergen.Add(allergen, potentialContainers);
            }

            var isolatedDangerousIngredients = new Dictionary<string, string>();
            while (potentialIngredientsPerAllergen.Any())
            {
                var isolatedPairs = potentialIngredientsPerAllergen.Where(p => p.Value.Count == 1);
                foreach (var (key, value) in isolatedPairs)
                {
                    var ingredient = value[0];
                    isolatedDangerousIngredients.Add(key, ingredient);
                    potentialIngredientsPerAllergen.Remove(key);
                    foreach (var pair in potentialIngredientsPerAllergen.Where(p => p.Value.Contains(ingredient)))
                    {
                        potentialIngredientsPerAllergen[pair.Key].Remove(ingredient);
                    }
                }
            }

            allergens.Sort();
            var result = string.Join(",", allergens.Select(a => isolatedDangerousIngredients[a]));
            Console.WriteLine(result);
        }
    }

    public class IngredientsLabel
    {
        private List<string> ingredients;
        private List<string> allergens;

        public IngredientsLabel(IEnumerable<string> ingredients, IEnumerable<string> allergens)
        {
            this.ingredients = ingredients.ToList();
            this.allergens = allergens.ToList();
        }

        public List<string> Allergens()
        {
            // Distinct copy of list
            return allergens.ToList();
        }

        public List<string> Ingredients()
        {
            return ingredients.ToList();
        }

        public bool ContainsAllergen(string allergen)
        {
            return allergens.Contains(allergen);
        }

        public bool ContainsIngredient(string ingredient)
        {
            return ingredients.Contains(ingredient);
        }

        public static IngredientsLabel Parse(string line)
        {
            var parts = line.Split(" (contains ");
            var ingredients = parts[0].Split(" ");
            var allergens = parts[1].TrimEnd(')').Split(", ");
            return new IngredientsLabel(ingredients, allergens);
        }
    }
}
