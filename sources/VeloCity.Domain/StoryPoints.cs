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
    public readonly struct StoryPoints : IFormattable
    {
        private const string MeasurementUnit = "SP";

        public float Value { get; init; }

        public bool IsEmpty { get; private init; }

        public bool IsNotEmpty => !IsEmpty;

        public bool IsZero => !IsEmpty && Value == 0;

        public static StoryPoints Empty { get; } = new()
        {
            IsEmpty = true
        };

        public static StoryPoints Zero { get; } = new();

        public override string ToString()
        {
            return IsEmpty
                ? $"- {MeasurementUnit}"
                : $"{Value} {MeasurementUnit}";
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(format);
        }

        public string ToString(string format)
        {
            if (format == "standard")
                return ToStandardDigitsString();

            return IsEmpty
                ? $"- {MeasurementUnit}"
                : $"{Value.ToString(format)} {MeasurementUnit}";
        }

        public string ToStandardDigitsString()
        {
            return IsEmpty
                ? $"- {MeasurementUnit}"
                : IsZero
                    ? $"0 {MeasurementUnit}"
                    : $"{Value:0.####} {MeasurementUnit}";
        }

        public static implicit operator float(StoryPoints storyPoints)
        {
            return storyPoints.Value;
        }

        public static implicit operator StoryPoints(float storyPoints)
        {
            return new StoryPoints
            {
                Value = storyPoints
            };
        }

        public static implicit operator float?(StoryPoints storyPoints)
        {
            return storyPoints.IsEmpty
                ? null
                : storyPoints.Value;
        }

        public static implicit operator StoryPoints(float? storyPoints)
        {
            if (storyPoints == null)
            {
                return new StoryPoints
                {
                    IsEmpty = true
                };
            }

            return new StoryPoints
            {
                Value = storyPoints.Value
            };
        }

        public static implicit operator StoryPoints?(float? storyPoints)
        {
            if (storyPoints == null)
                return null;

            return new StoryPoints
            {
                Value = storyPoints.Value
            };
        }

        public bool Equals(StoryPoints other)
        {
            return Value.Equals(other.Value) && IsEmpty == other.IsEmpty;
        }

        public override bool Equals(object obj)
        {
            return obj is StoryPoints other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, IsEmpty);
        }

        public static bool operator ==(StoryPoints storyPoints1, StoryPoints storyPoints2)
        {
            return Math.Abs(storyPoints1.Value - storyPoints2.Value) < 0.0000000000000000000000000000000000000000000000000000000000000000000001;
        }

        public static bool operator !=(StoryPoints storyPoints1, StoryPoints storyPoints2)
        {
            return Math.Abs(storyPoints1.Value - storyPoints2.Value) >= 0.0000000000000000000000000000000000000000000000000000000000000000000001;
        }
    }
}