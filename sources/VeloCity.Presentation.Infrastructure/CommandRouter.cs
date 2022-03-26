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

        public ICommand CreateCommand(Arguments arguments)
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
                return arguments[parameterInfo.Name];

            if (parameterInfo.Order != null)
                return arguments.GetOrdinal(parameterInfo.Order.Value);

            return null;
        }
    }
}