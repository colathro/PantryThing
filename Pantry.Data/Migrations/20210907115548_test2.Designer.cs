﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pantry.Data;

namespace Pantry.Data.Migrations
{
    [DbContext(typeof(DataBase))]
    [Migration("20210907115548_test2")]
    partial class test2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("Pantry.Core.Models.BetterRecipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MainOutputFoodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RecipeId");

                    b.HasIndex("MainOutputFoodId");

                    b.ToTable("BetterRecipes");
                });

            modelBuilder.Entity("Pantry.Core.Models.Equipment", b =>
                {
                    b.Property<int>("EquipmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int?>("RecipeStepId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EquipmentId");

                    b.HasIndex("RecipeStepId");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("Pantry.Core.Models.Food", b =>
                {
                    b.Property<int>("FoodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("FoodId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("Pantry.Core.Models.FoodInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int?>("BetterRecipeRecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BetterRecipeRecipeId1")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int?>("FoodTypeFoodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BetterRecipeRecipeId");

                    b.HasIndex("BetterRecipeRecipeId1");

                    b.HasIndex("FoodTypeFoodId");

                    b.ToTable("FoodInstances");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStep", b =>
                {
                    b.Property<int>("RecipeStepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BetterRecipeRecipeId")
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

                    b.HasIndex("BetterRecipeRecipeId");

                    b.ToTable("RecipeStep");
                });

            modelBuilder.Entity("Pantry.Data.BetterRecipeInput", b =>
                {
                    b.Property<int>("BetterRecipeInputId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<int>("BetterRecipeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BetterRecipeInputId");

                    b.ToTable("BetterRecipeInputs");
                });

            modelBuilder.Entity("Pantry.Core.Models.BetterRecipe", b =>
                {
                    b.HasOne("Pantry.Core.Models.Food", "MainOutput")
                        .WithMany("BetterRecipes")
                        .HasForeignKey("MainOutputFoodId");

                    b.Navigation("MainOutput");
                });

            modelBuilder.Entity("Pantry.Core.Models.Equipment", b =>
                {
                    b.HasOne("Pantry.Core.Models.RecipeStep", null)
                        .WithMany("Equipments")
                        .HasForeignKey("RecipeStepId");
                });

            modelBuilder.Entity("Pantry.Core.Models.FoodInstance", b =>
                {
                    b.HasOne("Pantry.Core.Models.BetterRecipe", null)
                        .WithMany("Inputs")
                        .HasForeignKey("BetterRecipeRecipeId");

                    b.HasOne("Pantry.Core.Models.BetterRecipe", null)
                        .WithMany("Outputs")
                        .HasForeignKey("BetterRecipeRecipeId1");

                    b.HasOne("Pantry.Core.Models.Food", "FoodType")
                        .WithMany()
                        .HasForeignKey("FoodTypeFoodId");

                    b.Navigation("FoodType");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStep", b =>
                {
                    b.HasOne("Pantry.Core.Models.BetterRecipe", null)
                        .WithMany("RecipeSteps")
                        .HasForeignKey("BetterRecipeRecipeId");
                });

            modelBuilder.Entity("Pantry.Core.Models.BetterRecipe", b =>
                {
                    b.Navigation("Inputs");

                    b.Navigation("Outputs");

                    b.Navigation("RecipeSteps");
                });

            modelBuilder.Entity("Pantry.Core.Models.Food", b =>
                {
                    b.Navigation("BetterRecipes");
                });

            modelBuilder.Entity("Pantry.Core.Models.RecipeStep", b =>
                {
                    b.Navigation("Equipments");
                });
#pragma warning restore 612, 618
        }
    }
}
