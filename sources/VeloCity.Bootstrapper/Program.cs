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
using System.Threading.Tasks;
using Autofac;
using DustInTheWind.ConsoleTools;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain.DataAccess;
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

        private static async Task Main(string[] args)
        {
            bool debugVerbose = true;

            try
            {
                Config config = new();
                debugVerbose = config.DebugVerbose;

                container = BuildContainer();

                if (args.Length == 0)
                    throw new Exception("No command was provided.");

                ICliCommand command = CreateCommand(args);
                await command.Execute();

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

            switch (commandName)
            {
                case "sprint":
                    AnalyzeSprintCommand analyzeSprintCommand = container.Resolve<AnalyzeSprintCommand>();
                    analyzeSprintCommand.SprintNumber = args.Length > 1
                        ? int.Parse(args[1])
                        : null;
                    return analyzeSprintCommand;

                case "calendar":
                    return container.Resolve<PresentSprintCalendarCommand>();

                case "sprints":
                    PresentSprintsCommand presentSprintsCommand = container.Resolve<PresentSprintsCommand>();
                    presentSprintsCommand.SprintCount = args.Length > 1
                        ? int.Parse(args[1])
                        : null;
                    return presentSprintsCommand;

                case "velocity":
                    PresentVelocityCommand presentVelocityCommand = container.Resolve<PresentVelocityCommand>();
                    presentVelocityCommand.SprintCount = args.Length > 1
                        ? int.Parse(args[1])
                        : null;
                    return presentVelocityCommand;

                case "vacations":
                    VacationsCommand vacationsCommand = container.Resolve<VacationsCommand>();
                    vacationsCommand.PersonName = args.Length > 1
                        ? args[1]
                        : null;
                    return vacationsCommand;

                default:
                    throw new Exception("Invalid Command");
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