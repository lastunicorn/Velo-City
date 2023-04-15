// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Reflection;
using Autofac;
using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.SettingsAccess;
using DustInTheWind.VeloCity.SystemAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprints;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;
using DustInTheWind.VeloCity.Wpf.Presentation.MainArea.Main;
using DustInTheWind.VeloCity.Wpf.UserAccess;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.VeloCity.Wpf.Bootstrapper;

internal class Setup
{
    public static IContainer BuildContainer()
    {
        ContainerBuilder containerBuilder = new();
        ConfigureServices(containerBuilder);

        return containerBuilder.Build();
    }

    private static void ConfigureServices(ContainerBuilder containerBuilder)
    {
        Assembly assembly = typeof(PresentSprintsRequest).Assembly;
        containerBuilder.RegisterMediatR(assembly);
        containerBuilder.RegisterGeneric(typeof(ExceptionHandlingBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        containerBuilder.RegisterType<MediatrRequestBus>().As<IRequestBus>().SingleInstance();

        containerBuilder.RegisterType<ApplicationState>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<EventBus>().AsSelf().SingleInstance();
        containerBuilder.RegisterType<SystemClock>().As<ISystemClock>();
        containerBuilder.RegisterType<Config>().As<IConfig>().SingleInstance();

        containerBuilder
            .Register(context =>
            {
                IConfig config = context.Resolve<IConfig>();

                return new JsonDatabase
                {
                    PersistenceLocation = config.DatabaseLocation
                };
            })
            .AsSelf()
            .As<IDataStorage>()
            .SingleInstance();

        containerBuilder.RegisterType<VeloCityDbContext>().AsSelf();
        containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

        containerBuilder.RegisterType<MainWindow>().AsSelf();
        containerBuilder.RegisterType<MainViewModel>().AsSelf();
        containerBuilder.RegisterType<NewSprintCommand>().AsSelf();

        containerBuilder.RegisterType<UserInterface>().As<IUserInterface>();
    }
}