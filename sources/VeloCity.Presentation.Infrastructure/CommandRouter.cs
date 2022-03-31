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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    public class CommandRouter
    {
        private readonly AvailableCommands availableCommands;
        private readonly ICommandFactory commandFactory;

        public event EventHandler<CommandCreatedEventArgs> CommandCreated;

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
            ArgumentsLens argumentsLens = new(arguments);

            ICommand command = CreateCommand(argumentsLens);

            CommandCreatedEventArgs args = new()
            {
                UnusedArguments = argumentsLens.EnumerateUnusedArguments().ToList()
            };
            OnCommandCreated(args);

            return command;
        }

        private ICommand CreateCommand(ArgumentsLens argumentsLens)
        {
            if (!argumentsLens.HasUnusedArguments)
            {
                CommandInfo helpCommandInfo = availableCommands.GetHelpCommand();
                return commandFactory.Create(helpCommandInfo.Type);
            }

            Argument commandArgument = argumentsLens.GetCommand();
            CommandInfo commandInfo = availableCommands.GetCommandInfo(commandArgument.Value);

            if (commandInfo == null)
                throw new InvalidCommandException();

            ICommand command = commandFactory.Create(commandInfo.Type);
            SetParameters(command, commandInfo, argumentsLens);

            return command;
        }

        private static void SetParameters(ICommand command, CommandInfo commandInfo, ArgumentsLens argumentsLens)
        {
            foreach (CommandParameterInfo parameterInfo in commandInfo.ParameterInfos)
            {
                Argument argument = FindArgumentFor(argumentsLens, parameterInfo);

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

        private static Argument FindArgumentFor(ArgumentsLens argumentsLens, CommandParameterInfo parameterInfo)
        {
            if (parameterInfo.Name != null)
            {
                Argument argument = argumentsLens.GetArgument(parameterInfo.Name);

                if (argument != null)
                    return argument;
            }

            if (parameterInfo.ShortName != 0)
            {
                Argument argument = argumentsLens.GetArgument(parameterInfo.ShortName.ToString());

                if (argument != null)
                    return argument;
            }

            if (parameterInfo.Order != null)
            {
                Argument argument = argumentsLens.GetArgument(parameterInfo.Order.Value);

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

        protected virtual void OnCommandCreated(CommandCreatedEventArgs e)
        {
            CommandCreated?.Invoke(this, e);
        }
    }
}