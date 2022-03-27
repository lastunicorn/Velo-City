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

namespace DustInTheWind.VeloCity.Domain
{
    public readonly struct Velocity
    {
        private const string MeasurementUnit = "SP/h";

        public float Value { get; init; }

        public bool IsNull { get; private init; }

        public bool IsEmpty => Value == 0;

        public static Velocity Null { get; } = new()
        {
            IsNull = true
        };

        public static Velocity Empty { get; } = new();

        public override string ToString()
        {
            return IsNull
                ? $"- {MeasurementUnit}"
                : $"{Value} {MeasurementUnit}";
        }

        public string ToString(string format)
        {
            return IsNull
                ? $"- {MeasurementUnit}"
                : $"{Value.ToString(format)} {MeasurementUnit}";
        }

        public string ToStandardDigitsString()
        {
            return IsNull
                ? $"- {MeasurementUnit}"
                : IsEmpty
                    ? $"0 {MeasurementUnit}"
                    : $"{Value:0.0000} {MeasurementUnit}";
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
    }
}