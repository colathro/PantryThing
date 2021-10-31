﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pantry.Core.Models;
using Pantry.Data;

namespace Pantry.ServiceGateways.Recipe
{

    public class FoodServiceGateWay
    {
        private readonly Func<DataBase> _dbFactory;

        public FoodServiceGateWay(Func<DataBase> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void DeleteFood(int foodId)
        {
            using (var db = _dbFactory())
            {
                var x = db.Foods.First(x => x.FoodId == foodId);
                db.Foods.Remove(x);
                db.SaveChanges();
            }
        }

        public List<Food> GetAllFoods()
        {
            using (var db = _dbFactory())
            {
                if (db.Foods is null || !db.Foods.Any()) return null;
                return db.Foods
                    .Include(x => x.RecipeFoods)
                    .ThenInclude(x => x.Recipe).ToList();
            }
        }
        public void AddFood(string newFoodName)
        {
            using (var db = _dbFactory())
            {
                db.Foods.Add(new() { FoodName = newFoodName });
                db.SaveChanges();
            }
        }

        public void KeepOnlyUniqueFoodNames()
        { //ToDo: This method should be a part of another Service Gateway that finds issues.
            using (var db = _dbFactory())
            {
                var names = new List<string>();
                foreach (var x in db.Foods)
                {
                    if (!names.Contains(x.FoodName) && !string.IsNullOrWhiteSpace(x.FoodName))
                    {
                        names.Add(x.FoodName);
                    }
                    else
                    {
                        db.Foods.Remove(x);
                    }
                }
                db.SaveChanges();
            }
        }

        public List<Core.Models.Recipe> GetRecipes(Core.Models.Food selectedFood)
        {
            using (var db = _dbFactory())
            {
                if (selectedFood is null || db.RecipeFoods is null) return null;

                var newList = db.RecipeFoods
                    .Where(x => x.FoodId == selectedFood.FoodId)
                    .Select(x => x.Recipe)
                    .ToList();

                return newList;
            }
        }

    }

    public class RecipeServiceGateway
    {
        private readonly Func<DataBase> _dbFactory;

        public RecipeServiceGateway(Func<DataBase> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public List<Core.Models.Recipe> GetRecipes()
        {
            using (var db = _dbFactory())
            {
                return db.Recipes
                    .Include(x => x.RecipeFoods)
                    .ThenInclude(x => x.Food)
                    .ToList();
            }
        }

        public void AddEmptyRecipe(string newRecipeName)
        {
            using (var db = _dbFactory())
            {
                db.Recipes.Add(new() { Description = newRecipeName });
                db.SaveChanges();
            }
        }
        public List<Core.Models.Recipe> GetRecipe(int recipeId)
        {
            using (var db = _dbFactory())
            {
                return db.Recipes
                    .Where(x => x.RecipeId == recipeId)
                    .Include(x => x.RecipeFoods)
                    .ThenInclude(x => x.Food)
                    .ToList();
            }
        }

        public List<Food> GetFoods()
        {
            using (var db = _dbFactory())
            {
                return db.Foods.ToList();
            }
        }

        public void DeleteRecipe(Core.Models.Recipe recipe)
        {
            using (var db = _dbFactory())
            {
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
        }

        public void DeleteFood(RecipeFood selectedRecipeFood)
        {
            using (var db = _dbFactory())
            {
                db.RecipeFoods.Remove(selectedRecipeFood);
                db.SaveChanges();
            }
        }

        public void DeleteRecipeStep(RecipeStep selectedRecipeStep)
        {
            using (var db = _dbFactory())
            {
                db.RecipeSteps.Remove(selectedRecipeStep);
                db.SaveChanges();
            }
        }
        public List<RecipeStep> GetRecipeSteps(Core.Models.Recipe selectedRecipe)
        {
            List<RecipeStep> newList;
            using (var db = _dbFactory())
            {
                if (selectedRecipe is null && (!db.RecipeSteps.Any() ||
                                               !db.RecipeSteps.Any(x =>
                                                   x.RecipeId == selectedRecipe.RecipeId)))
                {
                    newList = new();
                }
                else
                {
                    newList = db.RecipeSteps.Where(
                            x => x.RecipeId == selectedRecipe.RecipeId)
                        .Include(y => y.RecipeStepEquipmentType)
                        .Include(y => y.RecipeStepEquipmentType)
                        .ThenInclude(y => y.Equipment).ToList();
                }

                return newList;
            }
        }

        public List<RecipeFood> GetRecipeFoods(Core.Models.Recipe selectedRecipe)
        {
            using (var db = _dbFactory())
            {
                if (selectedRecipe is null || db.RecipeFoods is null) return null;
                var newList = db.RecipeFoods.Include(x => x.Food).Where(x => x.RecipeId == selectedRecipe.RecipeId)
                    .ToList();

                return newList;
            }
        }

        public bool AddRecipeFood(int recipeId, int foodId, double amount)
        {
            using (var db = _dbFactory())
            {
                var rx =
                    db.Recipes
                        .Where(x => x.RecipeId == recipeId)
                        .Include(x => x.RecipeFoods)
                        .FirstOrDefault();
                if (rx?.RecipeFoods is null)
                {
                    return false;
                }
                var x = new RecipeFood() { Amount = amount, FoodId = foodId, RecipeId = recipeId };
                rx.RecipeFoods.Add(x);
                db.SaveChanges();
                return true;
            }
        }

        public void AddRecipeStep(string newDescription, int recipeId, double tempDuration)
        {

            using (var db = _dbFactory())
            {
                var entity = db.RecipeSteps.Add(new()
                {
                    Instruction = newDescription,
                    RecipeId = recipeId,
                    TimeCost = tempDuration
                }).Entity;
                var stepId = entity.RecipeStepId;

                var equipments = this.GetEquipmentProjections();
                entity.RecipeStepEquipmentType = new List<RecipeStepEquipmentType>(equipments
                    .Where(x => x.IsSelected)
                    .Select(x => new RecipeStepEquipmentType()
                    {
                        EquipmentId = x.EquipmentId,
                        RecipeStepId = stepId
                    }));
                db.SaveChanges();
            }
        }

        public List<EquipmentProjection> GetEquipmentProjections()
        {
            using (var db = _dbFactory())
            {
                return db.Equipments.Select(x =>
                    new EquipmentProjection()
                    {
                        IsSelected = false,
                        EquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName
                    }).ToList();
            }
        }
        public List<LocationFoods> GetLocationFoods()
        {
            using (var db = _dbFactory())
            {
                return db.LocationFoods
                    .Where(x => x.Quantity > 0)
                    .Include(x => x.Item)
                    .ThenInclude(x => x.Food)
                    .Include(x => x.Location)
                    .ToList();
            }
        }

    }
}