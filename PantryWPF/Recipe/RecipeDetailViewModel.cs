﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Pantry.Core.Models;
using Pantry.ServiceGateways.Recipe;
using Pantry.WPF.Food;
using Pantry.WPF.Main;
using Pantry.WPF.Shared;
using Stylet;

namespace Pantry.WPF.Recipe
{
    public class RecipeDetailViewModel : Screen
    {
        private readonly RecipeServiceGateway _recipeServiceGateway;
        private readonly FoodListViewModel _foodListViewModel;
        private Pantry.Core.Models.Recipe _selectedRecipe;

        public List<Core.Models.Food> Foods { get; set; }

        public DelegateCommand SaveStepCommand { get; set; }
        public DelegateCommand SaveFoodCommand { get; set; }
        public DelegateCommand DeleteStepCommand { get; set; }
        public DelegateCommand DeleteFoodCommand { get; set; }
        public DelegateCommand DeleteThisRecipeCommand { get; set; }
        public DelegateCommand CookCommand { get; set; }
        public DelegateCommand GoToFoodCommand { get; set; }
        public NavigationCommand NavigateToFood { get; set; }
        public BindableCollection<RecipeStep> RecipeStepsList { get; set; } = new();
        public BindableCollection<RecipeFood> RecipeFoodsList { get; set; } = new();

        public BindableCollection<EquipmentTypeProjection> EquipmentTypeProjections { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetAndNotify(ref _description, value, nameof(Description));
        }
        private string _newDescription;
        public string NewDescription
        {
            get => _newDescription;
            set => SetAndNotify(ref _newDescription, value, nameof(NewDescription));
        }
        private string _newDuration;
        public string NewDuration
        {
            get => _newDuration;
            set => SetAndNotify(ref _newDuration, value, nameof(NewDuration));
        }
        private Core.Models.Food _newFood;
        public Core.Models.Food NewFood
        {
            get => _newFood;
            set => SetAndNotify(ref _newFood, value, nameof(NewFood));
        }
        private string _newFoodAmount;
        public string NewFoodAmount
        {
            get => _newFoodAmount;
            set => SetAndNotify(ref _newFoodAmount, value, nameof(NewFoodAmount));
        }
        private RecipeStep _selectedRecipeStep;
        public RecipeStep SelectedRecipeStep
        {
            get => _selectedRecipeStep;
            set => SetAndNotify(ref _selectedRecipeStep, value, nameof(SelectedRecipeStep));
        }
        private RecipeFood _selectedRecipeFood;
        public RecipeFood SelectedRecipeFood
        {
            get => _selectedRecipeFood;
            set => SetAndNotify(ref _selectedRecipeFood, value, nameof(SelectedRecipeFood));
        }
        private string _itemsUsed;
        public string ItemsUsed
        {
            get => _itemsUsed;
            set => SetAndNotify(ref _itemsUsed, value, nameof(ItemsUsed));
        }
        private bool _canCook;
        public bool CanCook
        {
            get => _canCook;
            set => SetAndNotify(ref _canCook, value, nameof(CanCook));
        }

        public RecipeDetailViewModel(RecipeServiceGateway recipeServiceGateway, FoodListViewModel foodListViewModel)
        {
            _recipeServiceGateway = recipeServiceGateway;
            _foodListViewModel = foodListViewModel;
            SaveStepCommand = new(SaveNewStep);
            SaveFoodCommand = new(SaveNewFood);
            DeleteStepCommand = new(DeleteSelectedStep);
            DeleteFoodCommand = new(DeleteSelectedFood);
            DeleteThisRecipeCommand = new(DeleteThisRecipe);
            CookCommand = new(CookIt);
        }

        public async Task Load(int recipeId, string description)
        {
            Description = description;
            var recipes = await _recipeServiceGateway.GetRecipe(recipeId);
            _selectedRecipe = recipes.FirstOrDefault(x => x.RecipeId == recipeId);
            Foods = await _recipeServiceGateway.GetFoods();
            EquipmentTypeProjections = new(_recipeServiceGateway.GetEquipmentTypeProjections());
            await LoadRecipeDetailData();
            CanCook = await CalculateCanCook();
        }

        public async Task CookIt()
        {
            // ReSharper disable once RedundantJumpStatement
            return;
        }

        public async Task<bool> CalculateCanCook()
        {
            return false;
        }


        private async Task LoadRecipeDetailData()
        {
            await LoadSteps();
            await LoadFoodInstances();
        }

        private async Task DeleteThisRecipe()
        {
            var recipes = await _recipeServiceGateway.GetRecipe(_selectedRecipe.RecipeId);
            var thisRecipe =recipes.FirstOrDefault();
            if (thisRecipe is null)
            {
                return;
            }

            await _recipeServiceGateway.DeleteRecipe(thisRecipe);
            await LoadRecipeDetailData();
        }

        private async Task DeleteSelectedFood()
        {
            if (SelectedRecipeFood is null || _selectedRecipe is null) return;
            await _recipeServiceGateway.DeleteFood(SelectedRecipeFood);
            await LoadRecipeDetailData();
        }

        private async Task DeleteSelectedStep()
        {
            if (SelectedRecipeStep is null || _selectedRecipe is null) return;
            await _recipeServiceGateway.DeleteRecipeStep(SelectedRecipeStep);
            await LoadRecipeDetailData();
        }

        private async Task SaveNewFood()
        {
            if (_selectedRecipe is null) { return; }
            if (NewFood is not null && double.TryParse(NewFoodAmount, out var foodAmount) && foodAmount != 0)
            {
                var saveSucceeded = await _recipeServiceGateway.AddRecipeFood(_selectedRecipe.RecipeId, NewFood.FoodId, foodAmount);
                if (saveSucceeded)
                {
                    NewFoodAmount = "";
                    await LoadRecipeDetailData();
                }
                else
                {
                    MessageBox.Show("It failed for some reason.");
                }
            }
        }

        private async Task LoadFoodInstances()
        {
            RecipeFoodsList.Clear();
            RecipeFoodsList.AddRange(_recipeServiceGateway.GetRecipeFoods(_selectedRecipe));
        }

        private async Task LoadSteps()
        {
            RecipeStepsList.Clear();
            RecipeStepsList.AddRange(await _recipeServiceGateway.GetRecipeSteps(_selectedRecipe.RecipeId));
            NotifyOfPropertyChange(nameof(RecipeStepsList));
        }

        private async Task SaveNewStep()
        {
            if (_selectedRecipe is null) return;
            //ToDo: Why do I not yet have centralized exception handling?

            var goodNumber = int.TryParse(NewDuration, out var tempDuration);

            if (!goodNumber || string.IsNullOrWhiteSpace(NewDescription)) return;
            var equipmentTypeIds = EquipmentTypeProjections.Where(x => x.IsSelected).Select(x => x.EquipmentTypeId);
            _recipeServiceGateway.AddRecipeStep(NewDescription, _selectedRecipe.RecipeId, tempDuration, equipmentTypeIds);

            NewDescription = "";
            NewDuration = "";
            await LoadRecipeDetailData ();
        }
    }
}