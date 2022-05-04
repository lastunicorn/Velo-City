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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.Help
{
    [HelpCommand("help", ShortDescription = "Obtain more details and explanation about the available commands.", Order = int.MaxValue)]
    public class HelpCommand : ICommand
    {
        private readonly AvailableCommands availableCommands;

        [CommandParameter(DisplayName = "command name", Order = 1, IsOptional = true)]
        public string CommandName { get; set; }

        public List<CommandShortInfo> Commands { get; private set; }

        public CommandFullInfo CommandDetails { get; private set; }

        public HelpCommand(AvailableCommands availableCommands)
        {
            this.availableCommands = availableCommands ?? throw new ArgumentNullException(nameof(availableCommands));
        }

        public Task Execute()
        {
            if (CommandName != null)
                CommandDetails = GetCommandDetails(CommandName);
            else
                Commands = GetAllCommandDetails();

            return Task.CompletedTask;
        }

        private CommandFullInfo GetCommandDetails(string commandName)
        {
            CommandInfo commandInfo = availableCommands.GetByName(commandName);

            if (commandInfo == null)
                throw new CommandNotFoundException(commandName);

            return new CommandFullInfo(commandInfo);
        }

        private List<CommandShortInfo> GetAllCommandDetails()
        {
            return availableCommands.GetAllEnabled()
                .Select(x => new CommandShortInfo
                {
                    Name = x.Name,
                    Description = x.DescriptionLines.ToList()
                })
                .ToList();
        }
    }
}