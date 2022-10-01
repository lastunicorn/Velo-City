// VeloCity
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

namespace DustInTheWind.VeloCity.Domain
{
    public readonly struct HoursValue : IFormattable
    {
        public static char DefaultZeroCharacter { get; set; } = '-';

        public int Value { get; init; }

        public static HoursValue Zero { get; } = new(0);

        public HoursValue(int value)
        {
            Value = value;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value == 0
                ? format == null
                    ? $"{DefaultZeroCharacter} h"
                    : $"{format} h"
                : $"{Value} h";
        }

        public override string ToString()
        {
            return Value == 0
                ? $"{DefaultZeroCharacter} h"
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

        public static HoursValue operator +(HoursValue hoursValue1, HoursValue hoursValue2)
        {
            return new HoursValue
            {
                Value = hoursValue1.Value + hoursValue2.Value
            };
        }

        public static HoursValue operator +(HoursValue hoursValue1, int hoursValue2)
        {
            return new HoursValue
            {
                Value = hoursValue1.Value + hoursValue2
            };
        }

        public static int operator +(int hoursValue1, HoursValue hoursValue2)
        {
            return hoursValue1 + hoursValue2.Value;
        }

        public static HoursValue operator -(HoursValue hoursValue1, HoursValue hoursValue2)
        {
            return new HoursValue
            {
                Value = hoursValue1.Value - hoursValue2.Value
            };
        }

        public static HoursValue operator -(HoursValue hoursValue1, int hoursValue2)
        {
            return new HoursValue
            {
                Value = hoursValue1.Value - hoursValue2
            };
        }

        public static int operator -(int hoursValue1, HoursValue hoursValue2)
        {
            return hoursValue1 - hoursValue2.Value;
        }

        public static HoursValue Parse(string stringValue)
        {
            int intValue = int.Parse(stringValue);
            return new HoursValue(intValue);
        }
    }
}