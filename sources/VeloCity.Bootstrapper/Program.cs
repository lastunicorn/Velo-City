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
using System.Threading.Tasks;
using Autofac;
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            ErrorMessageLevel errorMessageLevel = ErrorMessageLevel.Verbose;

            try
            {
                DisplayApplicationHeader();
                IContainer container = SetupServices.BuildContainer();
                await using ILifetimeScope lifetimeScope = container.BeginLifetimeScope();
                errorMessageLevel = GetErrorMessageLevel(lifetimeScope);
                await ProcessRequest(args, lifetimeScope);
            }
            catch (Exception ex)
            {
                if (errorMessageLevel == ErrorMessageLevel.Verbose)
                    CustomConsole.WriteLineError(ex);
                else
                    CustomConsole.WriteLineError(ex.Message);
            }
        }

        private static void DisplayApplicationHeader()
        {
            ApplicationHeader applicationHeader = new();
            applicationHeader.Display();
        }

        private static ErrorMessageLevel GetErrorMessageLevel(IComponentContext container)
        {
            IConfig config = container.Resolve<IConfig>();
            return config.ErrorMessageLevel;
        }
        
        private static async Task ProcessRequest(string[] args, IComponentContext container)
        {
            CommandRouter commandRouter = container.Resolve<CommandRouter>();

            Arguments arguments = new(args);
            await commandRouter.Execute(arguments);
        }
    }
}