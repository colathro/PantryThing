﻿using Pantry.Data;
using Pantry.ServiceGateways;
using Pantry.ServiceGateways.Equipment;
using Pantry.ServiceGateways.Recipe;
using Pantry.WPF.Main;
using Serilog;
using Serilog.Core;
using Stylet;
using StyletIoC;
// ReSharper disable MemberCanBeMadeStatic.Local

namespace Pantry.WPF
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            ConfigureDatabase(builder);
            ConfigureService(builder);
            ConfigureLogger(builder);
        }

        private void ConfigureDatabase(IStyletIoCBuilder builder)
        {
            builder
                .Bind<DataBase>()
                .ToFactory(_ => new DataBase());
        }

        private void ConfigureLogger(IStyletIoCBuilder builder)
        {
            builder
                .Bind<Logger>()
                .ToInstance(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.File(@"..\..\..\..\PantryLogs.log")
                    .CreateLogger()
                );
        }

        private void ConfigureService(IStyletIoCBuilder builder)
        {
            builder.Bind<ItemService>().ToSelf();
            builder.Bind<EquipmentServiceGateway>().ToSelf();
            builder.Bind<RecipeServiceGateway>().ToSelf();
            builder.Bind<FoodServiceGateWay>().ToSelf();
            builder.Bind<Seeder>().ToSelf();
        }

        protected override void Configure()
        {
            var ctx = Container.Get<DataBase>();
            ctx.Database.EnsureCreated();
        }
    }
}
