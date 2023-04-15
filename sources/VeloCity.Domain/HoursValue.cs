// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using System.Text.RegularExpressions;

namespace DustInTheWind.VeloCity.Domain;

public readonly partial struct HoursValue : IFormattable
{
    public static char DefaultZeroCharacter { get; set; } = '-';

    public int Value { get; init; }

    public static HoursValue Zero { get; } = new(0);

    public bool IsZero => Value == 0;

    public HoursValue(int value)
    {
        Value = value;
    }

    public bool Equals(HoursValue other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is HoursValue other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value;
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

    public static HoursValue Parse(string stringValue)
    {
        Regex regex = new(@"^\s*([0-9]*|-)\s*h?\s*$");
        Match match = regex.Match(stringValue);

        if (!match.Success)
            throw new ArgumentException("The string does not represent an hour value.", nameof(stringValue));

        string numberString = match.Groups[1].Value;

        if (numberString == "-")
            return Zero;

        int intValue = int.Parse(numberString);
        return new HoursValue(intValue);
    }
}