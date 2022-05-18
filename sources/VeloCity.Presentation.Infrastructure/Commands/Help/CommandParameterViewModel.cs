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

using System.Text;

namespace DustInTheWind.VeloCity.Presentation.Infrastructure.Commands.Help
{
    internal class CommandParameterViewModel
    {
        private readonly CommandParameterInfo commandParameterInfo;

        public bool DisplayAsNamedParameter { get; set; }

        public CommandParameterViewModel(CommandParameterInfo commandParameterInfo)
        {
            this.commandParameterInfo = commandParameterInfo;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            bool isNamedParameter = commandParameterInfo.Order == null || DisplayAsNamedParameter;
            bool isOptionalParameter = commandParameterInfo.IsOptional;
            bool hasMultipleNames = isNamedParameter && commandParameterInfo.Name != null && commandParameterInfo.ShortName != 0;

            if (isOptionalParameter)
                sb.Append('[');
            else if (hasMultipleNames)
                sb.Append('(');

            if (isNamedParameter)
            {
                string computedParameterName = SerializeNamedParameter();
                sb.Append(computedParameterName);
            }
            else
            {
                string computedParameterName = SerializeUnnamedParameter();
                sb.Append(computedParameterName);
            }

            if (isOptionalParameter)
                sb.Append(']');
            else if (hasMultipleNames)
                sb.Append(')');

            return sb.ToString();
        }

        private string SerializeUnnamedParameter()
        {
            StringBuilder sb = new();

            sb.Append('<');

            if (commandParameterInfo.DisplayName != null)
                sb.Append(commandParameterInfo.DisplayName.Replace(' ', '-'));
            else if (commandParameterInfo.Name != null)
                sb.Append(commandParameterInfo.Name);
            else if (commandParameterInfo.ShortName != 0)
                sb.Append(commandParameterInfo.ShortName);
            else
                sb.Append("param" + commandParameterInfo.Order);

            sb.Append('>');

            return sb.ToString();
        }

        private string SerializeNamedParameter()
        {
            StringBuilder sb = new();

            if (commandParameterInfo.Name != null)
            {
                sb.Append($"-{commandParameterInfo.Name}");

                string valueDescription = SerializeValueDescription();
                sb.Append(valueDescription);
            }

            if (commandParameterInfo.ShortName != 0)
            {
                if (commandParameterInfo.Name != null)
                    sb.Append(" | ");

                sb.Append($"-{commandParameterInfo.ShortName}");

                string valueDescription = SerializeValueDescription();
                sb.Append(valueDescription);
            }

            return sb.ToString();
        }

        private string SerializeValueDescription()
        {
            if (commandParameterInfo.ParameterType.IsText())
                return (" <text>");

            if (commandParameterInfo.ParameterType.IsNumber())
                return (" <number>");

            if (commandParameterInfo.ParameterType.IsListOfNumbers())
                return (" <list-of-numbers>");

            if (commandParameterInfo.ParameterType.IsListOfTexts())
                return (" <list-of-texts>");

            if (commandParameterInfo.ParameterType.IsBoolean())
                return string.Empty;

            return (" <value>");
        }
    }
}