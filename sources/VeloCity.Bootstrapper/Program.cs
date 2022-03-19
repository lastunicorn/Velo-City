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
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Presentation;
using DustInTheWind.VeloCity.Presentation.Commands.Help;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class Program
    {
        private static IContainer container;

        private static async Task Main(string[] args)
        {
            ErrorMessageLevel debugVerbose = ErrorMessageLevel.Verbose;

            try
            {
                ApplicationHeader applicationHeader = new();
                applicationHeader.Display();

                container = BuildContainer();

                IConfig config = container.Resolve<IConfig>();
                debugVerbose = config.ErrorMessageLevel;

                Arguments arguments = new(args);
                ICommand command = CreateCommand(arguments);

                await command.Execute(arguments);

                ExecuteViewsFor(command);
            }
            catch (Exception ex)
            {
                if (debugVerbose == ErrorMessageLevel.Verbose)
                    CustomConsole.WriteLineError(ex);
                else
                    CustomConsole.WriteLineError(ex.Message);
            }
        }

        private static IContainer BuildContainer()
        {
            ContainerBuilder containerBuilder = new();
            ConfigureServices(containerBuilder);

            return containerBuilder.Build();
        }

        private static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            Assembly assembly = typeof(AnalyzeSprintRequest).Assembly;
            containerBuilder.RegisterMediatR(assembly);

            containerBuilder.RegisterType<Config>().As<IConfig>().SingleInstance();
            
            AvailableCommands availableCommands = new();
            availableCommands.SearchInCurrentAppDomain();

            containerBuilder.RegisterInstance(availableCommands).AsSelf();

            containerBuilder
                .Register((c, p) =>
                {
                    try
                    {
                        IConfig config = c.Resolve<IConfig>();
                        string databaseFilePath = config.DatabaseLocation;
                        return new DatabaseFile(databaseFilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new DatabaseNotFoundException(ex);
                    }
                })
                .AsSelf();

            containerBuilder.RegisterType<Database>().AsSelf();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            foreach (Type type in availableCommands.GetCommandTypes())
                containerBuilder.RegisterType(type).AsSelf();

            foreach (Type type in availableCommands.GetViewTypes())
                containerBuilder.RegisterType(type).AsSelf();

            containerBuilder.RegisterType<DataGridFactory>().AsSelf();
        }

        private static ICommand CreateCommand(Arguments arguments)
        {
            if (arguments.Count == 0)
                return container.Resolve<HelpCommand>();

            string commandName = arguments[0].Value;

            AvailableCommands availableCommands = container.Resolve<AvailableCommands>();
            CommandInfo commandInfo = availableCommands.GetCommandInfo(commandName);

            if (commandInfo == null)
                throw new Exception("Invalid Command");

            return (ICommand)container.Resolve(commandInfo.Type);
        }

        private static void ExecuteViewsFor(ICommand command)
        {
            Type commandType = command.GetType();

            AvailableCommands availableCommands = container.Resolve<AvailableCommands>();
            IEnumerable<Type> viewTypes = availableCommands.GetViewTypesForCommand(commandType);

            foreach (Type viewType in viewTypes)
            {
                object view = container.Resolve(viewType);

                MethodInfo displayMethodInfo = viewType.GetMethod(nameof(IView<ICommand>.Display));
                displayMethodInfo?.Invoke(view, new object[] { command });
            }
        }
    }
}