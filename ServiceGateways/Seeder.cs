using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pantry.Core.Models;
using Pantry.Data;
using Serilog.Core;

namespace Pantry.ServiceGateways
{
    public class Seeder
    {
        private readonly Func<DataBase> _dbFactory;
        private readonly Logger _logger;

        public Seeder(Func<DataBase> dbFactory, Logger logger)
        {
            _dbFactory = dbFactory;
            _logger = logger;
        }

        public void PopulateLocationIfNone()
        {
            using (var dbContext = _dbFactory())
            {
                if (!dbContext.Locations.Any())
                {
                    _ = dbContext.Locations.Add(new() { LocationName = "Default" });
                    _logger.Debug("Added default Location.");

                }
            }
        }

        public void SeedDatabase()
        {
            _logger.Debug("Beginning seed.");
            PopulateFoods();
            PopulateLocations();
            PopulateEquipmentTypes();
            PopulateEquipment();
            PopulateRecipes();
            PopulateItems();
            PopulateInventory();
            PopulateConsumers();
            PopulateMealOfTheDay();
            PopulateMealInstances();
            _logger.Debug("Finished seed.");
        }


        private void PopulateMealInstances()
        {
            using (var dbContext = _dbFactory())
            {
                var tableName = dbContext.MealInstances.EntityType.GetTableName();
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {tableName};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{tableName}';");
                }

                var day = (DateTime.Now - DateTime.MinValue).Days;

                foreach (var consumer in dbContext.Consumers)
                {
                    foreach (var mealOfTheDay in dbContext.MealOfTheDays)
                    {
                        for (var i = 1; i < 8; i++)
                        {
                            dbContext.MealInstances.Add(new()
                            {
                                ConsumerId = consumer.ConsumerId,
                                MealOfTheDayId = mealOfTheDay.MealOfTheDayId,
                                DaysSinceZero = day + i,
                                MealInstanceDateTime = DateTime.MinValue + TimeSpan.FromDays(day + i) + mealOfTheDay.MealOfTheDayDateTime.TimeOfDay
                            });
                        }
                    }
                }
                _ = dbContext.SaveChanges();
            }
        }

        private void PopulateMealOfTheDay()
        {
            using (var dbContext = _dbFactory())
            {
                var tableName = dbContext.MealOfTheDays.EntityType.GetTableName();
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {tableName};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{tableName}';");
                }
                dbContext.MealOfTheDays.Add(new() { MealOfTheDayName = "Ad hoc", MealOfTheDayDateTime = DateTime.MinValue + DateTime.Parse("00:00").TimeOfDay });
                dbContext.MealOfTheDays.Add(new() { MealOfTheDayName = "Breakfast", MealOfTheDayDateTime = DateTime.MinValue + DateTime.Parse("06:35").TimeOfDay });
                dbContext.MealOfTheDays.Add(new() { MealOfTheDayName = "Lunch", MealOfTheDayDateTime = DateTime.MinValue + DateTime.Parse("12:00").TimeOfDay });
                dbContext.MealOfTheDays.Add(new() { MealOfTheDayName = "Dinner", MealOfTheDayDateTime = DateTime.MinValue + DateTime.Parse("18:30").TimeOfDay });
                _ = dbContext.SaveChanges();
            }
        }

        private void PopulateConsumers()
        {
            using (var dbContext = _dbFactory())
            {
                var tableName = dbContext.Consumers.EntityType.GetTableName();
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {tableName};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{tableName}';");
                }

                dbContext.Consumers.Add(new() { ConsumerName = "Abel" });
                dbContext.Consumers.Add(new() { ConsumerName = "Betty" });
                dbContext.Consumers.Add(new() { ConsumerName = "Clyde" });
                dbContext.Consumers.Add(new() { ConsumerName = "Dorthy" });
                _ = dbContext.SaveChanges();
            }
        }

        private void PopulateItems()
        {
            using (var dbContext = _dbFactory())
            {
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {dbContext.Items.EntityType.GetTableName()};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{dbContext.Items.EntityType.GetTableName()}';");
                }
                //TODO: Investigate why there is both `Weight` and `Amount` being used.
                _ = dbContext.Items.Add(new() { Food = dbContext.Foods.First(x => x.FoodName == "Frozen Chicken"), Weight = 4540, Upc = "100Chicken" });
                _ = dbContext.Items.Add(new() { Food = dbContext.Foods.First(x => x.FoodName == "BBQ Sauce"), Weight = 454, Upc = "200BBQ" });
                _ = dbContext.Items.Add(new() { Food = dbContext.Foods.First(x => x.FoodName == "Milk"), Weight = 3900, Upc = "1GalMilk" });
                _ = dbContext.Items.Add(new() { Food = dbContext.Foods.First(x => x.FoodName == "Sliced Bread"), Weight = 454, Upc = "LotsOfBread" });
                _ = dbContext.Items.Add(new() { Food = dbContext.Foods.First(x => x.FoodName == "Bread"), Weight = 1000, Upc = "Artisan Bread" });

                _ = dbContext.SaveChanges();

            }
        }

        private void PopulateInventory()
        {
            using (var dbContext = _dbFactory())
            {
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {dbContext.LocationFoods.EntityType.GetTableName()};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{dbContext.LocationFoods.EntityType.GetTableName()}';");
                }
                var defaultLocation = dbContext.Locations.First(x => x.LocationName == "Default");
                var now = DateTime.Now;

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "1GalMilk"),
                    PurchaseDate = now,
                    ExpiryDate = now + TimeSpan.FromDays(14),
                    Quantity = 3900,
                    Location = defaultLocation,
                    Exists = true
                });

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "Artisan Bread"),
                    PurchaseDate = now,
                    ExpiryDate = now + TimeSpan.FromDays(14),
                    Quantity = 1000,
                    Location = defaultLocation,
                    Exists = true
                });

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "LotsOfBread"),
                    PurchaseDate = now + TimeSpan.FromDays(-7),
                    OpenDate = now + TimeSpan.FromDays(-7),
                    ExpiryDate = now + TimeSpan.FromDays(1),
                    Quantity = 454,
                    Location = defaultLocation,
                    Exists = true
                });

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "1GalMilk"),
                    PurchaseDate = now + TimeSpan.FromDays(-7),
                    OpenDate = now + TimeSpan.FromDays(-7),
                    ExpiryDate = now + TimeSpan.FromDays(1),
                    Quantity = 3900,
                    Location = defaultLocation,
                    Exists = true
                });

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "100Chicken"),
                    PurchaseDate = now + TimeSpan.FromDays(-30),
                    ExpiryDate = now + TimeSpan.FromDays(180),
                    Quantity = 75,
                    Location = defaultLocation,
                    Exists = true
                });

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "100Chicken"),
                    PurchaseDate = now + TimeSpan.FromDays(-180),
                    ExpiryDate = now + TimeSpan.FromDays(30),
                    Quantity = 66,
                    Location = dbContext.Locations.First(x => x.LocationName == "Deep Freezer"),
                    Exists = true
                });

                _ = dbContext.LocationFoods.Add(new()
                {
                    Item = dbContext.Items.First(x => x.Upc == "200BBQ"),
                    PurchaseDate = now + TimeSpan.FromDays(-30),
                    ExpiryDate = now + TimeSpan.FromDays(180),
                    Quantity = 220,
                    Location = defaultLocation,
                    Exists = true
                });

                _ = dbContext.SaveChanges();
            }
        }

        private void PopulateLocations()
        {
            using (var dbContext = _dbFactory())
            {
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {dbContext.Locations.EntityType.GetTableName()};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{dbContext.Locations.EntityType.GetTableName()}';");
                }
                _ = dbContext.Locations.Add(new() { LocationName = "Default" });
                _ = dbContext.Locations.Add(new() { LocationName = "Fridge" });
                _ = dbContext.Locations.Add(new() { LocationName = "Freezer" });
                _ = dbContext.Locations.Add(new() { LocationName = "Deep Freezer" });
                _ = dbContext.SaveChanges();
            }
        }

        private void PopulateEquipmentTypes()
        {
            using (var dbContext = _dbFactory())
            {
                string tableName = dbContext.EquipmentTypes.EntityType.GetTableName();
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {tableName};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{tableName}';");
                }
                _ = dbContext.EquipmentTypes.Add(new() { EquipmentTypeName = "Bread Machine" });
                _ = dbContext.EquipmentTypes.Add(new() { EquipmentTypeName = "Human" });
                _ = dbContext.EquipmentTypes.Add(new() { EquipmentTypeName = "Fridge" });
                _ = dbContext.EquipmentTypes.Add(new() { EquipmentTypeName = "Sous Vide" });

                _ = dbContext.SaveChanges();
            }
        }

        private void PopulateEquipment()
        {
            using (var dbContext = _dbFactory())
            {
                var tableName = dbContext.Equipments.EntityType.GetTableName();
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {tableName};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{tableName}';");
                }
                _ = dbContext.Equipments.Add(new() { EquipmentName = "Bread Machine", EquipmentTypeId = dbContext.EquipmentTypes.First(x => x.EquipmentTypeName == "Bread Machine").EquipmentTypeId });
                _ = dbContext.Equipments.Add(new() { EquipmentName = "Human", EquipmentTypeId = dbContext.EquipmentTypes.First(x => x.EquipmentTypeName == "Human").EquipmentTypeId });
                _ = dbContext.Equipments.Add(new() { EquipmentName = "Fridge", EquipmentTypeId = dbContext.EquipmentTypes.First(x => x.EquipmentTypeName == "Fridge").EquipmentTypeId });
                _ = dbContext.Equipments.Add(new() { EquipmentName = "Sous Vide", EquipmentTypeId = dbContext.EquipmentTypes.First(x => x.EquipmentTypeName == "Sous Vide").EquipmentTypeId });
                _ = dbContext.SaveChanges();
            }
        }

        private void ClearRecipesTable()
        {
            using (var dbContext = _dbFactory())
            {
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {dbContext.Recipes.EntityType.GetTableName()};");
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {dbContext.RecipeSteps.EntityType.GetTableName()};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{dbContext.Recipes.EntityType.GetTableName()}';");
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{dbContext.RecipeSteps.EntityType.GetTableName()}';");
                }
                _ = dbContext.SaveChanges();
            }
        }

        private void AddRecipeCookedChicken()
        {
            using (var dbContext = _dbFactory())
            {
                {
                    var recipe = dbContext.Recipes.Add(new()
                    {
                        Description = "Cooked Chicken",
                        RecipeFoods = new()
                        {
                            new() { Amount = 120, Food = dbContext.Foods.First(x => x.FoodName == "Raw Chicken") },
                            new() { Amount = 1, Food = dbContext.Foods.First(x => x.FoodName == "BBQ Sauce") },
                            new() { Amount = -120, Food = dbContext.Foods.First(x => x.FoodName == "Cooked Chicken") },
                        },
                        RecipeSteps = new()
                    });
                    _ = dbContext.SaveChanges();

                    recipe.Entity.RecipeSteps.Add(new()
                    {
                        RecipeId = recipe.Entity.RecipeId,
                        Instruction = "Put chicken in Sous Vide.",
                        Order = 1,
                        TimeCost = 1,
                        RecipeStepEquipmentType = new List<RecipeStepEquipmentType>()
                        {
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Sous Vide"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Sous Vide").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Human"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Human").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                        },
                    });

                    _ = dbContext.SaveChanges();
                    recipe.Entity.RecipeSteps.Add(new()
                    {
                        RecipeId = recipe.Entity.RecipeId,
                        Instruction = "Let it cook.",
                        Order = 2,
                        TimeCost = 120,
                        RecipeStepEquipmentType = new List<RecipeStepEquipmentType>()
                        {
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Sous Vide"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Sous Vide").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                        }
                    });

                    _ = dbContext.SaveChanges();
                    recipe.Entity.RecipeSteps.Add(new()
                    {
                        RecipeId = recipe.Entity.RecipeId,
                        Instruction = "Take chicken out.",
                        Order = 3,
                        TimeCost = 1,
                        RecipeStepEquipmentType = new List<RecipeStepEquipmentType>()
                        {
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Sous Vide"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Sous Vide").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Human"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Human").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                        }
                    });
                }
            }
        }

        private void AddRecipeRawChicken()
        {
            using (var dbContext = _dbFactory())
            {
                {
                    var recipe = dbContext.Recipes.Add(new()
                    {
                        Description = "Raw Chicken",
                        RecipeFoods = new()
                        {
                            new() { Amount = 120, Food = dbContext.Foods.First(x => x.FoodName == "Frozen Chicken") },
                            new() { Amount = -120, Food = dbContext.Foods.First(x => x.FoodName == "Raw Chicken") },
                        },
                        RecipeSteps = new()
                    });
                    _ = dbContext.SaveChanges();

                    recipe.Entity.RecipeSteps.Add(new()
                    {
                        RecipeId = recipe.Entity.RecipeId,
                        Instruction = "Put chicken in Fridge.",
                        Order = 1,
                        TimeCost = 1,
                        RecipeStepEquipmentType = new List<RecipeStepEquipmentType>()
                        {
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Fridge"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Fridge").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Human"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Human").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                        },
                    });

                    _ = dbContext.SaveChanges();
                    recipe.Entity.RecipeSteps.Add(new()
                    {
                        RecipeId = recipe.Entity.RecipeId,
                        Instruction = "Let it defrost.",
                        Order = 2,
                        TimeCost = 120,
                        RecipeStepEquipmentType = new List<RecipeStepEquipmentType>()
                        {
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Fridge"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Fridge").EquipmentTypeId,
                                RecipeStepId = 2
                            },
                        }
                    });
                }
            }

        }

        private void AddRecipeSlicedBread()
        {
            using (var dbContext = _dbFactory())
            {
                {
                    var recipe = dbContext.Recipes.Add(new()
                    {
                        Description = "Sliced Bread",
                        RecipeFoods = new()
                        {
                            new() { Amount = 454, Food = dbContext.Foods.First(x => x.FoodName == "Bread") },
                            new() { Amount = -454, Food = dbContext.Foods.First(x => x.FoodName == "Sliced Bread") },
                        },
                        RecipeSteps = new()
                    });
                    _ = dbContext.SaveChanges();

                    recipe.Entity.RecipeSteps.Add(new()
                    {
                        RecipeId = recipe.Entity.RecipeId,
                        Instruction = "Cut Bread.",
                        Order = 1,
                        TimeCost = 3,
                        RecipeStepEquipmentType = new List<RecipeStepEquipmentType>()
                        {
                            new()
                            {
                                Equipment = dbContext.Equipments.First(x => x.EquipmentName == "Human"),
                                EquipmentTypeId = dbContext.EquipmentTypes.First(x=>x.EquipmentTypeName=="Human").EquipmentTypeId,
                                RecipeStepId = 1
                            },
                        },
                    });
                }
            }
        }

        private void PopulateRecipes()
        {
            ClearRecipesTable();
            AddRecipeCookedChicken();
            AddRecipeRawChicken();
            AddRecipeSlicedBread();
        }
        private void PopulateFoods()
        {
            using (var dbContext = _dbFactory())
            {
                _ = dbContext.Database.ExecuteSqlRaw(@$"DELETE FROM {dbContext.Foods.EntityType.GetTableName()};");
                if (dbContext.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                    _ = dbContext.Database.ExecuteSqlRaw(@$"UPDATE `sqlite_sequence` SET `seq` = 0 WHERE `name` = '{dbContext.Foods.EntityType.GetTableName()}';");
                }
                _ = dbContext.Foods.Add(new() { FoodName = "Frozen Chicken" });
                _ = dbContext.Foods.Add(new() { FoodName = "Raw Chicken", });
                _ = dbContext.Foods.Add(new() { FoodName = "BBQ Sauce" });
                _ = dbContext.Foods.Add(new() { FoodName = "Cooked Chicken" });
                _ = dbContext.Foods.Add(new() { FoodName = "Sliced Chicken", IsEdible = true });
                _ = dbContext.Foods.Add(new() { FoodName = "Flour" });
                _ = dbContext.Foods.Add(new() { FoodName = "Eggs" });
                _ = dbContext.Foods.Add(new() { FoodName = "Milk", IsEdible = true });
                _ = dbContext.Foods.Add(new() { FoodName = "Bread" });
                _ = dbContext.Foods.Add(new() { FoodName = "Sliced Bread", IsEdible = true });
                _ = dbContext.Foods.Add(new() { FoodName = "Chicken Sandwich", IsEdible = true });
                _ = dbContext.SaveChanges();

            }
        }

    }
}
