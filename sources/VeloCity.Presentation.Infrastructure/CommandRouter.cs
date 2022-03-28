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

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    public class CommandRouter
    {
        private readonly AvailableCommands availableCommands;
        private readonly ICommandFactory commandFactory;

        public CommandRouter(AvailableCommands availableCommands, ICommandFactory commandFactory)
        {
            this.availableCommands = availableCommands ?? throw new ArgumentNullException(nameof(availableCommands));
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        public async Task Execute(Arguments arguments)
        {
            ICommand command = CreateCommand(arguments);

            await command.Execute();

            ExecuteViewsFor(command);
        }

        private ICommand CreateCommand(Arguments arguments)
        {
            if (arguments.Count == 0)
            {
                CommandInfo helpCommandInfo = availableCommands.GetHelpCommand();
                return commandFactory.Create(helpCommandInfo.Type);
            }

            string commandName = arguments[0].Value;
            CommandInfo commandInfo = availableCommands.GetCommandInfo(commandName);

            if (commandInfo == null)
                throw new InvalidCommandException();

            ICommand command = commandFactory.Create(commandInfo.Type);
            SetParameters(command, commandInfo, arguments);

            return command;
        }

        private static void SetParameters(ICommand command, CommandInfo commandInfo, Arguments arguments)
        {
            foreach (CommandParameterInfo parameterInfo in commandInfo.ParameterInfos)
            {
                Argument argument = FindArgumentFor(arguments, parameterInfo);

                if (argument == null)
                {
                    if (!parameterInfo.IsOptional)
                    {
                        string parameterName = parameterInfo.DisplayName ?? parameterInfo.Name ?? parameterInfo.Order?.ToString();
                        throw new ParameterMissingException(parameterName);
                    }
                }
                else
                {
                    parameterInfo.SetValue(command, argument.Value);
                }
            }
        }

        private static Argument FindArgumentFor(Arguments arguments, CommandParameterInfo parameterInfo)
        {
            if (parameterInfo.Name != null)
            {
                Argument argument = arguments[parameterInfo.Name];

                if (argument != null)
                    return argument;
            }

            if (parameterInfo.ShortName != 0)
            {
                Argument argument = arguments[parameterInfo.ShortName.ToString()];

                if (argument != null)
                    return argument;
            }

            if (parameterInfo.Order != null)
            {
                Argument argument = arguments.GetOrdinal(parameterInfo.Order.Value);

                if (argument != null)
                    return argument;
            }

            return null;
        }

        private void ExecuteViewsFor(ICommand command)
        {
            Type commandType = command.GetType();

            IEnumerable<Type> viewTypes = availableCommands.GetViewTypesForCommand(commandType);

            foreach (Type viewType in viewTypes)
            {
                object view = commandFactory.CreateView(viewType);

                MethodInfo displayMethodInfo = viewType.GetMethod(nameof(IView<ICommand>.Display));
                displayMethodInfo?.Invoke(view, new object[] { command });
            }
        }
    }
}