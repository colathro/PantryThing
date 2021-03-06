using System;
using System.Collections.Generic;
using System.Linq;
using Pantry.Core.Models;

namespace Pantry.Core.Extensions
{
    public static class ExtensionMethods
    {

        public static string GetDagString(RecipeDag dag)
        {
            if (dag.SubordinateBetterRecipes.Count == 0) { return dag.MainRecipe.RecipeFoods.First(x => x.Amount < 0).Food.FoodName.ToString(); }
            return string.Join(Environment.NewLine, dag.SubordinateBetterRecipes.Select(x => dag.MainRecipe.RecipeFoods.First(y => y.Amount < 0).Food.FoodName.ToString() + "->" + GetDagString(x)));
        }

        public static void ConsoleResult(this CookPlan canCook)
        {
            if (canCook.CanMake)
            {
                if (canCook.TotalInput is not null) Console.WriteLine("Ingredients Used: " + Environment.NewLine + string.Join(Environment.NewLine, canCook.TotalInput.Select(x => x.Food.FoodName + "- " + x.Amount)));
                if (canCook.TotalOutput is not null) Console.WriteLine($"{Environment.NewLine}New Products: " + Environment.NewLine + string.Join(Environment.NewLine, canCook.TotalOutput.Where(x => x.Amount > 0).Select(x => x.Food.FoodName + "- " + x.Amount)));
                if (canCook.RecipesTouched is not null) Console.WriteLine($"{Environment.NewLine}Recipes: {Environment.NewLine}" + string.Join(Environment.NewLine, canCook.RecipesTouched.Select(x => x.Description)));
                if (canCook.RawCost is not null) Console.WriteLine($"{Environment.NewLine}Total Cost:{Environment.NewLine}"
                                   + string.Join(Environment.NewLine, canCook.RawCost.Select(x => x.Food.FoodName + ": " + x.Amount)));
                if (canCook.RecipeSteps is not null)
                {
                    Console.WriteLine($"{Environment.NewLine}Steps: ");
                    foreach (var x in canCook.RecipeSteps)
                    {
                        Console.WriteLine(string.Join(Environment.NewLine, x.Select(y => y.Instruction + "- " + y.TimeCost)));
                    }
                }

                if (canCook.RecipeDag is not null) { Console.WriteLine($"Dags{Environment.NewLine}" + GetDagString(canCook.RecipeDag)); }
                if (canCook.Steps is not null)
                {
                    Console.WriteLine(canCook.Steps);
                }
                Console.WriteLine("-----");
            }
        }
        public static void OutputRemaining(this List<RecipeFood> pantry)
        {
            if (pantry is null)
            {
                return;
            }
            Console.WriteLine($"Remaining: {Environment.NewLine}" + string.Join(Environment.NewLine, pantry.Where(pi => pi.Amount is > 0 and < 10_000_000).Select(pi => pi.Food.FoodName + ": " + pi.Amount)));
        }
    }
}