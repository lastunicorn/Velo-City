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

using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.Presentation.AnalyzeSprint;
using DustInTheWind.VeloCity.Presentation.PresentSprintCalendar;
using DustInTheWind.VeloCity.Presentation.PresentSprints;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.VeloCity
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IContainer container = BuildContainer();

            AnalyzeSprintCommand command = container.Resolve<AnalyzeSprintCommand>();
            //PresentSprintCalendarCommand command = container.Resolve<PresentSprintCalendarCommand>();
            //PresentSprintsCommand command = container.Resolve<PresentSprintsCommand>();

            await command.Execute();
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
        }
    }
}