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
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Presentation;
using DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint;
using DustInTheWind.VeloCity.Presentation.Commands.PresentSprintCalendar;
using DustInTheWind.VeloCity.Presentation.Commands.PresentSprints;
using DustInTheWind.VeloCity.Presentation.Commands.PresentVelocity;
using DustInTheWind.VeloCity.Presentation.Commands.Vacations;
using MediatR.Extensions.Autofac.DependencyInjection;


namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class Program
    {
        private static IContainer container;

        private static readonly Dictionary<string, Type> CommandTypes = new()
        {
            { "sprint", typeof(AnalyzeSprintCommand) },
            { "calendar", typeof(PresentSprintCalendarCommand) },
            { "sprints", typeof(PresentSprintsCommand) },
            { "velocity", typeof(PresentVelocityCommand) },
            { "vacations", typeof(VacationsCommand) }
        };

        private static async Task Main(string[] args)
        {
            bool debugVerbose = true;

            try
            {
                ApplicationHeader applicationHeader = new();
                applicationHeader.Display();

                Config config = new();
                debugVerbose = config.DebugVerbose;

                container = BuildContainer();

                if (args.Length == 0)
                    throw new Exception("No command was provided.");

                ICliCommand command = CreateCommand(args);
                await command.Execute(args);

            }
            catch (Exception ex)
            {
                if (debugVerbose)
                    CustomConsole.WriteLineError(ex);
                else
                    CustomConsole.WriteLineError(ex.Message);
            }
        }

        private static ICliCommand CreateCommand(string[] args)
        {
            string commandName = args[0];

            if (!CommandTypes.ContainsKey(commandName))
                throw new Exception("Invalid Command");

            Type commandType = CommandTypes[commandName];
            return (ICliCommand)container.Resolve(commandType);
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

            containerBuilder.Register((c, p) =>
            {
                Config config = new();
                string databaseFilePath = config.DatabaseLocation;
                return new DatabaseFile(databaseFilePath);
            }).AsSelf();

            containerBuilder.RegisterType<Database>().AsSelf();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            containerBuilder.RegisterType<AnalyzeSprintCommand>().AsSelf();
            containerBuilder.RegisterType<AnalyzeSprintView>().AsSelf();

            containerBuilder.RegisterType<PresentSprintCalendarCommand>().AsSelf();
            containerBuilder.RegisterType<PresentSprintCalendarView>().AsSelf();

            containerBuilder.RegisterType<PresentSprintsCommand>().AsSelf();
            containerBuilder.RegisterType<PresentSprintsView>().AsSelf();

            containerBuilder.RegisterType<PresentVelocityCommand>().AsSelf();
            containerBuilder.RegisterType<PresentVelocityView>().AsSelf();

            containerBuilder.RegisterType<VacationsCommand>().AsSelf();
            containerBuilder.RegisterType<VacationsView>().AsSelf();
        }
    }
}