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

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public struct HoursValue
    {
        public int Value { get; set; }

        public override string ToString()
        {
            return Value == 0
                ? "- h"
                : $"{Value} h";
        }

        public static implicit operator HoursValue(int value)
        {
            return new HoursValue
            {
                Value = value
            };
        }

        public static implicit operator int(HoursValue hoursValue)
        {
            return hoursValue.Value;
        }

        public static implicit operator HoursValue(int? value)
        {
            return new HoursValue
            {
                Value = value ?? 0
            };
        }

        public static implicit operator int?(HoursValue hoursValue)
        {
            return hoursValue.Value == 0
                ? null
                : hoursValue.Value;
        }
    }
}