﻿// VeloCity
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

using Autofac;
using DustInTheWind.ConsoleTools.Commando;

namespace DustInTheWind.VeloCity.Cli.Bootstrapper;

internal class CommandFactory : ICommandFactory
{
    private readonly IComponentContext context;

    public CommandFactory(IComponentContext context)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public TCommand Create<TCommand>()
        where TCommand : ICommand
    {
        return context.Resolve<TCommand>();
    }

    public ICommand Create(Type commandType)
    {
        if (commandType == null) throw new ArgumentNullException(nameof(commandType));

        bool isCommandType = typeof(ICommand).IsAssignableFrom(commandType);
        if (!isCommandType)
            throw new TypeIsNotCommandException(commandType);

        return (ICommand)context.Resolve(commandType);
    }

    public object CreateView(Type viewType)
    {
        return context.Resolve(viewType);
    }
}