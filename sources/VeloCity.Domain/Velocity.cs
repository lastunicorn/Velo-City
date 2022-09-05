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

namespace DustInTheWind.VeloCity.Domain
{
    public readonly struct Velocity : IFormattable
    {
        private const string MeasurementUnit = "SP/h";

        public float Value { get; init; }

        public bool IsEmpty { get; private init; }

        public bool IsZero => !IsEmpty && Value == 0;

        public static Velocity Empty { get; } = new()
        {
            IsEmpty = true
        };

        public static Velocity Zero { get; } = new();

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
                    : $"{Value:0.0000} {MeasurementUnit}";
        }

        public bool Equals(Velocity other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Velocity other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Velocity velocity1, Velocity velocity2)
        {
            return Math.Abs(velocity1.Value - velocity2.Value) < 0.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001;
        }

        public static bool operator !=(Velocity velocity1, Velocity velocity2)
        {
            return Math.Abs(velocity1.Value - velocity2.Value) >= 0.0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001;
        }

        public static implicit operator float(Velocity velocity)
        {
            return velocity.Value;
        }

        public static implicit operator Velocity(float velocity)
        {
            return new Velocity
            {
                Value = velocity
            };
        }

        public static implicit operator Velocity(float? velocity)
        {
            if (velocity == null)
                return new Velocity
                {
                    IsEmpty = true
                };

            return new Velocity
            {
                Value = velocity.Value
            };
        }

        public static implicit operator Velocity?(float? velocity)
        {
            if (velocity == null)
                return null;

            return new Velocity
            {
                Value = velocity.Value
            };
        }
    }
}