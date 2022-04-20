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
using System.Text;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.Help
{
    [HelpCommand("help", ShortDescription = "A list with all the available commands.", Order = int.MaxValue)]
    [CommandUsage("help")]
    public class HelpCommand : ICommand
    {
        private readonly AvailableCommands availableCommands;

        [CommandParameter(DisplayName = "Command Name", Order = 1, IsOptional = true)]
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
            {
                IEnumerable<CommandInfo> allCommand = availableCommands.GetOrderedCommandInfos()
                    .Where(x => x.IsEnabled);

                CommandDetails = allCommand
                    .Where(x => x.Name == CommandName)
                    .Select(CreateCommandFullInfo)
                    .FirstOrDefault();
            }
            else
            {
                IEnumerable<CommandInfo> allCommand = availableCommands.GetOrderedCommandInfos()
                    .Where(x => x.IsEnabled);

                Commands = allCommand
                    .Select(x => new CommandShortInfo
                    {
                        Name = x.Name,
                        Description = x.DescriptionLines.ToList()
                    })
                    .ToList();
            }

            return Task.CompletedTask;
        }

        private static CommandFullInfo CreateCommandFullInfo(CommandInfo commandInfo)
        {
            return new CommandFullInfo
            {
                Name = commandInfo.Name,
                Description = commandInfo.DescriptionLines.ToList(),
                Usage = CreateUsage(commandInfo)
            };
        }

        private static string CreateUsage(CommandInfo commandInfo)
        {
            StringBuilder sb = new($"velo {commandInfo.Name}");

            IEnumerable<CommandParameterViewModel> ordinalParameters = commandInfo.ParameterInfos
                .Where(x => x.Order != null)
                .OrderBy(x => x.Order)
                .Select(x => new CommandParameterViewModel(x));

            foreach (CommandParameterViewModel parameterInfo in ordinalParameters)
            {
                sb.Append(' ');
                sb.Append(parameterInfo);
            }

            IEnumerable<CommandParameterViewModel> namedParameters = commandInfo.ParameterInfos
                .Where(x => x.Name != null || x.ShortName != 0)
                .Select(x => new CommandParameterViewModel(x));

            foreach (CommandParameterViewModel parameterInfo in namedParameters)
            {
                sb.Append(' ');
                sb.Append(parameterInfo);
            }

            return sb.ToString();
        }
    }

    internal class CommandParameterViewModel
    {
        private readonly CommandParameterInfo commandParameterInfo;

        public CommandParameterViewModel(CommandParameterInfo commandParameterInfo)
        {
            this.commandParameterInfo = commandParameterInfo;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            char openChar = commandParameterInfo.IsOptional
                ? '['
                : '<';
            sb.Append(openChar);

            if (commandParameterInfo.Order != null)
            {
                sb.Append(commandParameterInfo.DisplayName.Replace(' ', '-'));
            }
            else
            {
                if (commandParameterInfo.Name != null)
                    sb.Append($"-{commandParameterInfo.Name}");

                if (commandParameterInfo.ShortName != null)
                {
                    if (commandParameterInfo.Name != null)
                        sb.Append(" | ");

                    sb.Append($"-{commandParameterInfo.ShortName}");
                }
            }

            char closingChar = commandParameterInfo.IsOptional
                ? ']'
                : '>';
            sb.Append(closingChar);

            return sb.ToString();
        }
    }
}