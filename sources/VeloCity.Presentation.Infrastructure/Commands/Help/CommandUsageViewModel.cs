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

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DustInTheWind.VeloCity.Presentation.Infrastructure.Commands.Help
{
    public class CommandUsageViewModel
    {
        private readonly CommandInfo commandInfo;

        public CommandUsageViewModel(CommandInfo commandInfo)
        {
            this.commandInfo = commandInfo;
        }

        public override string ToString()
        {
            if (commandInfo == null)
                return string.Empty;

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
                .Select(x => new CommandParameterViewModel(x)
                {
                    DisplayAsNamedParameter = true
                });

            foreach (CommandParameterViewModel parameterInfo in namedParameters)
            {
                sb.Append(' ');
                sb.Append(parameterInfo);
            }

            return sb.ToString();
        }
    }
}