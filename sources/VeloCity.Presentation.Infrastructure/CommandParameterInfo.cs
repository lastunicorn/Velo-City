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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DustInTheWind.VeloCity.Presentation.Infrastructure
{
    public class CommandParameterInfo
    {
        private readonly PropertyInfo propertyInfo;
        private readonly CommandParameterAttribute customAttribute;

        public string Name => customAttribute.Name;

        public char ShortName => customAttribute.ShortName;

        public string DisplayName => customAttribute.DisplayName;

        public int? Order => customAttribute.Order == 0
            ? null
            : customAttribute.Order;

        public bool IsOptional => customAttribute.IsOptional;

        public CommandParameterInfo(PropertyInfo propertyInfo, CommandParameterAttribute customAttribute)
        {
            this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            this.customAttribute = customAttribute ?? throw new ArgumentNullException(nameof(customAttribute));
        }

        public void SetValue(ICommand command, string value)
        {
            object valueAsObject = propertyInfo.PropertyType == typeof(bool) && value == null
                ? true
                : ParseValue(value);

            propertyInfo.SetValue(command, valueAsObject);
        }

        private object ParseValue(string value)
        {
            bool isListOfNumbers = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType == typeof(List<int>);
            if (isListOfNumbers)
            {
                return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse)
                    .ToList();
            }

            bool isListOfStrings = propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType == typeof(List<string>);
            if (isListOfStrings)
            {
                return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();
            }

            TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
            return typeConverter.ConvertFromString(value);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            if (Name != null)
                sb.Append($"Name = {Name}");

            if (Order != null)
            {
                if (sb.Length > 0)
                    sb.Append("; ");

                sb.Append($"Order = {Order}");
            }

            if (sb.Length > 0)
                sb.Append("; ");

            sb.Append($"Optional = {IsOptional}");

            return sb.ToString();
        }
    }
}