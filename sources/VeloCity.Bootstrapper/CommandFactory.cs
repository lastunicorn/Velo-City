﻿// Velo City
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
using Autofac;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class CommandFactory : ICommandFactory
    {
        private static readonly Type CommandInterfaceType = typeof(ICommand);
        private readonly IComponentContext context;

        public CommandFactory(IComponentContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TCommand Create<TCommand>() where TCommand : ICommand
        {
            return context.Resolve<TCommand>();
        }

        public ICommand Create(Type commandType)
        {
            if (commandType == null) throw new ArgumentNullException(nameof(commandType));

            bool isCommandType = CommandInterfaceType.IsAssignableFrom(commandType);
            if (!isCommandType)
                throw new Exception($"The {commandType.FullName} does not implement the {CommandInterfaceType.FullName}");

            return (ICommand)context.Resolve(commandType);
        }
    }
}