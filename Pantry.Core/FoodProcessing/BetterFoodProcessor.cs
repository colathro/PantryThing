﻿using System;
using System.Collections.Generic;

namespace Pantry.Core.FoodProcessing
{
    public class BetterFoodProcessor : IFoodProcessor
    {
        public CookPlan CanCookSomething(IList<FoodInstance> foodInventory, Recipe recipe, IList<Recipe> recipes = null)
        {
            throw new NotImplementedException();
        }
    }
}