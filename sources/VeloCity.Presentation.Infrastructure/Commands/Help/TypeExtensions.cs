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

namespace DustInTheWind.VeloCity.Presentation.Infrastructure.Commands.Help
{
    internal static class TypeExtensions
    {
        public static bool IsText(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsListOfTexts(this Type type)
        {
            return type == typeof(List<string>);
        }

        public static bool IsNumber(this Type type)
        {
            return type == typeof(int) ||
                   type == typeof(long) ||
                   type == typeof(short) ||
                   type == typeof(float) ||
                   type == typeof(double);
        }

        public static bool IsListOfNumbers(this Type type)
        {
            return type == typeof(List<int>) ||
                   type == typeof(List<long>) ||
                   type == typeof(List<short>) ||
                   type == typeof(List<float>) ||
                   type == typeof(List<double>);
        }

        public static bool IsBoolean(this Type type)
        {
            return type == typeof(bool);
        }
    }
}