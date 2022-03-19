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

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    public class CommandInfo
    {
        private readonly Type commandType;
        private CommandAttribute commandAttribute;
        private List<CommandUsageAttribute> commandUsageAttributes;
        private readonly List<string> descriptionLines;

        public string Name { get; }

        public IReadOnlyList<string> DescriptionLines => descriptionLines;

        public Type Type { get; }

        public int Order => commandAttribute.Order;

        public bool IsEnabled => commandAttribute.Enabled;

        public IEnumerable<CommandParameterInfo> ParameterInfos
        {
            get
            {
                return commandType.GetProperties()
                    .Select(x =>
                    {
                        CommandParameterAttribute customAttribute = x.GetCustomAttributes<CommandParameterAttribute>()
                            .SingleOrDefault();
                        
                        return customAttribute == null
                            ? null
                            : new CommandParameterInfo(x, customAttribute);
                    })
                    .Where(x => x != null)
                    .ToArray();
            }
        }

        public CommandInfo(Type commandType)
        {
            this.commandType = commandType ?? throw new ArgumentNullException(nameof(commandType));

            RetrieveAttributes();

            Name = CalculateCommandName();
            descriptionLines = CalculateDescription();
            Type = commandType;
        }

        private void RetrieveAttributes()
        {
            commandAttribute = commandType.GetCustomAttributes(typeof(CommandAttribute), false)
                .Cast<CommandAttribute>()
                .SingleOrDefault();

            commandUsageAttributes = commandType.GetCustomAttributes(typeof(CommandUsageAttribute), false)
                .Cast<CommandUsageAttribute>()
                .ToList();
        }

        private string CalculateCommandName()
        {
            if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.CommandName))
                return commandAttribute.CommandName;

            if (commandType.Name.EndsWith("Command"))
                return commandType.Name[..^"Command".Length].ToLower();

            return commandType.Name.ToLower();
        }

        private List<string> CalculateDescription()
        {
            List<string> lines = new();

            if (commandAttribute != null && !string.IsNullOrEmpty(commandAttribute.ShortDescription))
                lines.Add(commandAttribute.ShortDescription);

            if (commandUsageAttributes.Count > 0)
            {
                lines.Add("usage:");

                IEnumerable<string> usageExamples = commandUsageAttributes
                    .Select(x => "  " + x.UsageExample);

                lines.AddRange(usageExamples);
            }

            return lines;
        }
    }
}