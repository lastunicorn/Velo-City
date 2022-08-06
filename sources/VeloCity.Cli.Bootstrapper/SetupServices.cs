// Velo City
// Copyright (C) 2022 Dust in the Wind
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

using System;
using System.Reflection;
using Autofac;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentSprint;
using DustInTheWind.VeloCity.Cli.Presentation;
using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.SettingsAccess;
using DustInTheWind.VeloCity.SystemAccess;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.VeloCity.Cli.Bootstrapper
{
    internal class SetupServices
    {
        public static IContainer BuildContainer()
        {
            ContainerBuilder containerBuilder = new();
            ConfigureServices(containerBuilder);

            return containerBuilder.Build();
        }

        private static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            Assembly assembly = typeof(PresentSprintRequest).Assembly;
            containerBuilder.RegisterMediatR(assembly);

            containerBuilder.RegisterType<SystemClock>().As<ISystemClock>();
            containerBuilder.RegisterType<Config>().As<IConfig>().SingleInstance();
            containerBuilder.RegisterType<CommandRouter>().AsSelf();
            containerBuilder.RegisterType<CommandFactory>().As<ICommandFactory>();

            AvailableCommands availableCommands = new();
            availableCommands.LoadFromCurrentAppDomain();

            containerBuilder.RegisterInstance(availableCommands).AsSelf();

            containerBuilder.RegisterType<VeloCityDbContext>().AsSelf();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            foreach (Type type in availableCommands.GetCommandTypes())
                containerBuilder.RegisterType(type).AsSelf();

            foreach (Type type in availableCommands.GetViewTypes())
                containerBuilder.RegisterType(type).AsSelf();

            containerBuilder.RegisterType<DataGridFactory>().AsSelf();
        }
    }
}