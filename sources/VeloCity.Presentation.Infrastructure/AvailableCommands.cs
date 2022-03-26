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

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    public class AvailableCommands
    {
        private readonly List<CommandInfo> commandInfos = new();
        private readonly List<Type> viewTypes = new();

        public void SearchInCurrentAppDomain()
        {
            IEnumerable<Type> allTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes());

            foreach (Type type in allTypes)
            {
                if (IsCommandType(type))
                {
                    CommandInfo commandInfo = new(type);
                    commandInfos.Add(commandInfo);
                }

                if (IsViewType(type))
                    viewTypes.Add(type);
            }
        }

        private static bool IsCommandType(Type type)
        {
            return type != typeof(ICommand) && typeof(ICommand).IsAssignableFrom(type);
        }

        private static bool IsViewType(Type type)
        {
            Type[] interfaceTypes = type.GetInterfaces();

            foreach (Type interfaceType in interfaceTypes)
            {
                bool isGenericType = interfaceType.IsGenericType;

                if (!isGenericType)
                    continue;

                Type genericTypeDefinition = interfaceType.GetGenericTypeDefinition();

                return genericTypeDefinition == typeof(IView<>);
            }

            return false;
        }

        public IEnumerable<Type> GetCommandTypes()
        {
            return commandInfos
                .Select(x => x.Type);
        }

        public IEnumerable<Type> GetViewTypes()
        {
            return viewTypes;
        }

        public IEnumerable<Type> GetViewTypesForCommand(Type commandType)
        {
            return viewTypes
                .Where(x =>
                {
                    IEnumerable<Type> interfaceTypes = x.GetInterfaces();

                    foreach (Type interfaceType in interfaceTypes)
                    {
                        Type[] genericArgumentTypes = interfaceType.GetGenericArguments();

                        if (genericArgumentTypes.Length != 1)
                            continue;

                        if (genericArgumentTypes[0] != commandType)
                            continue;

                        return true;
                    }

                    return false;
                });
        }

        public List<CommandInfo> GetOrderedCommandInfos()
        {
            return commandInfos
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToList();
        }

        public CommandInfo GetCommandInfo(string commandName)
        {
            return commandInfos.FirstOrDefault(x => x.Name == commandName);
        }

        public CommandInfo GetHelpCommand()
        {
            CommandInfo commandInfo = commandInfos
                .FirstOrDefault(x => x.IsHelpCommand);

            if (commandInfo != null)
                return commandInfo;

            return commandInfos
                .FirstOrDefault(x => string.Equals(x.Name, "help", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}