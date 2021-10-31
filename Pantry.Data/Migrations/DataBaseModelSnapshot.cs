﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pantry.Data;

namespace Pantry.Data.Migrations
{
    [DbContext(typeof(DataBase))]
    partial class DataBaseModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("Pantry.Core.Models.Equipment", b =>
                {
                    b.Property<int>("EquipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EquipmentName")
                        .HasColumnType("TEXT");

                    b.Property<int>("EquipmentTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EquipmentId");

                    b.HasIndex("EquipmentTypeId");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("Pantry.Core.Models.EquipmentCommitment", b =>
                {
                    b.Property<int>("EquipmentCommitmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeStepId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("EquipmentCommitmentId");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("RecipeStepId");

                    b.ToTable("EquipmentCommitments");
                });

            modelBuilder.Entity("Pantry.Core.Models.EquipmentType", b =>
                {
                    b.Property<int>("EquipmentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EquipmentTypeName")
                        .HasColumnType("TEXT");

                    b.Property<int>("RecipeStepEquipmentTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EquipmentTypeId");

                    b.ToTable("EquipmentTypes");
                });

            modelBuilder.Entity("Pantry.Core.Models.Food", b =>
                {
                    b.Property<int>("FoodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FoodName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEdible")
                        .HasColumnType("INTEGER");

                    b.HasKey("FoodId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("Pantry.Core.Models.FoodFoodTag", b =>
                {
                    b.Property<int>("FoodFoodTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodTagId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TagName")
                        .HasColumnType("TEXT");

                    b.HasKey("FoodFoodTagId");

                    b.HasIndex("FoodId");

                    b.HasIndex("FoodTagId");

                    b.ToTable("FoodFoodTags");
                });

            modelBuilder.Entity("Pantry.Core.Models.FoodTag", b =>
                {
                    b.Property<int>("FoodTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FoodTagName")
                        .HasColumnType("TEXT");

                    b.HasKey("FoodTagId");

                    b.ToTable("FoodTags");
                });

            modelBuilder.Entity("Pantry.Core.Models.Inventory", b =>
                {
                    b.Property<int>("InventoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateExpired")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("TEXT");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Remaining")
                        .HasColumnType("REAL");

                    b.HasKey("InventoryId");

                    b.HasIndex("LocationId");

                    b.ToTable("Inventorys");
                });

            modelBuilder.Entity("Pantry.Core.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("InventoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UnitId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Upc")
                        .HasColumnType("TEXT");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("ItemId");

                    b.HasIndex("FoodId");

                    b.HasIndex("InventoryId");

                    b.HasIndex("UnitId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Pantry.Core.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("LocationName")
                        .HasColumnType("TEXT");

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Pantry.Core.Models.LocationFoods", b =>
                {
                    b.Property<int>("LocationFoodsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Exists")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.Property<double>("Quantity")
                        .HasColumnType("REAL");

                    b.HasKey("LocationFoodsId");

                    b.HasIndex("ItemId");

                    b.HasIndex("LocationId");

                    b.ToTable("LocationFoods");
                });

            modelBuilder.Entity("Pantry.Core.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.HasKey("RecipeId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeFood", b =>
                {
                    b.Property<int>("RecipeFoodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("FoodId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeFoodId");

                    b.HasIndex("FoodId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeFoods");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeRecipeTag", b =>
                {
                    b.Property<int>("RecipeRecipeTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeTagId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeRecipeTagId");

                    b.HasIndex("RecipeId");

                    b.HasIndex("RecipeTagId");

                    b.ToTable("RecipeRecipeTags");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStep", b =>
                {
                    b.Property<int>("RecipeStepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Instruction")
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TimeCost")
                        .HasColumnType("REAL");

                    b.HasKey("RecipeStepId");

                    b.HasIndex("RecipeId");

                    b.ToTable("RecipeSteps");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStepEquipmentType", b =>
                {
                    b.Property<int>("RecipeStepEquipmentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeStepId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeStepEquipmentTypeId");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("EquipmentTypeId");

                    b.HasIndex("RecipeStepId");

                    b.ToTable("RecipeStepEquipmentType");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeTag", b =>
                {
                    b.Property<int>("RecipeTagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("RecipeTagName")
                        .HasColumnType("TEXT");

                    b.HasKey("RecipeTagId");

                    b.ToTable("RecipeTags");
                });

            modelBuilder.Entity("Pantry.Core.Models.Unit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("UnitType")
                        .HasColumnType("TEXT");

                    b.Property<double>("UnitWeight")
                        .HasColumnType("REAL");

                    b.HasKey("UnitId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Pantry.Core.Models.Equipment", b =>
                {
                    b.HasOne("Pantry.Core.Models.EquipmentType", "EquipmentType")
                        .WithMany("Equipments")
                        .HasForeignKey("EquipmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EquipmentType");
                });

            modelBuilder.Entity("Pantry.Core.Models.EquipmentCommitment", b =>
                {
                    b.HasOne("Pantry.Core.Models.Equipment", "Equipment")
                        .WithMany("EquipmentCommitments")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.Recipe", "Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.RecipeStep", "RecipeStep")
                        .WithMany()
                        .HasForeignKey("RecipeStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Recipe");

                    b.Navigation("RecipeStep");
                });

            modelBuilder.Entity("Pantry.Core.Models.FoodFoodTag", b =>
                {
                    b.HasOne("Pantry.Core.Models.Food", "Food")
                        .WithMany("FoodFoodTags")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.FoodTag", "FoodTag")
                        .WithMany("FoodFoodTags")
                        .HasForeignKey("FoodTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");

                    b.Navigation("FoodTag");
                });

            modelBuilder.Entity("Pantry.Core.Models.Inventory", b =>
                {
                    b.HasOne("Pantry.Core.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Pantry.Core.Models.Item", b =>
                {
                    b.HasOne("Pantry.Core.Models.Food", "Food")
                        .WithMany("Items")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.Inventory", null)
                        .WithMany("Items")
                        .HasForeignKey("InventoryId");

                    b.HasOne("Pantry.Core.Models.Unit", "Unit")
                        .WithMany()
                        .HasForeignKey("UnitId");

                    b.Navigation("Food");

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Pantry.Core.Models.LocationFoods", b =>
                {
                    b.HasOne("Pantry.Core.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeFood", b =>
                {
                    b.HasOne("Pantry.Core.Models.Food", "Food")
                        .WithMany("RecipeFoods")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.Recipe", "Recipe")
                        .WithMany("RecipeFoods")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeRecipeTag", b =>
                {
                    b.HasOne("Pantry.Core.Models.Recipe", "Recipe")
                        .WithMany("RecipeRecipeTags")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.RecipeTag", "RecipeTag")
                        .WithMany("RecipeRecipeTags")
                        .HasForeignKey("RecipeTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");

                    b.Navigation("RecipeTag");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStep", b =>
                {
                    b.HasOne("Pantry.Core.Models.Recipe", "Recipe")
                        .WithMany("RecipeSteps")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStepEquipmentType", b =>
                {
                    b.HasOne("Pantry.Core.Models.Equipment", "Equipment")
                        .WithMany("RecipeStepEquipment")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.EquipmentType", "EquipmentType")
                        .WithMany("RecipeStepEquipmentType")
                        .HasForeignKey("EquipmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Pantry.Core.Models.RecipeStep", "RecipeStep")
                        .WithMany("RecipeStepEquipmentType")
                        .HasForeignKey("RecipeStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("EquipmentType");

                    b.Navigation("RecipeStep");
                });

            modelBuilder.Entity("Pantry.Core.Models.Equipment", b =>
                {
                    b.Navigation("EquipmentCommitments");

                    b.Navigation("RecipeStepEquipment");
                });

            modelBuilder.Entity("Pantry.Core.Models.EquipmentType", b =>
                {
                    b.Navigation("Equipments");

                    b.Navigation("RecipeStepEquipmentType");
                });

            modelBuilder.Entity("Pantry.Core.Models.Food", b =>
                {
                    b.Navigation("FoodFoodTags");

                    b.Navigation("Items");

                    b.Navigation("RecipeFoods");
                });

            modelBuilder.Entity("Pantry.Core.Models.FoodTag", b =>
                {
                    b.Navigation("FoodFoodTags");
                });

            modelBuilder.Entity("Pantry.Core.Models.Inventory", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Pantry.Core.Models.Recipe", b =>
                {
                    b.Navigation("RecipeFoods");

                    b.Navigation("RecipeRecipeTags");

                    b.Navigation("RecipeSteps");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStep", b =>
                {
                    b.Navigation("RecipeStepEquipmentType");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeTag", b =>
                {
                    b.Navigation("RecipeRecipeTags");
                });
#pragma warning restore 612, 618
        }
    }
}
