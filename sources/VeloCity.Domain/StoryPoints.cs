﻿// Velo City
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
    public readonly struct StoryPoints
    {
        public float Value { get; init; }

        public bool IsNull { get; private init; }

        public bool IsEmpty => Value == 0;

        public static StoryPoints Null { get; } = new()
        {
            IsNull = true
        };

        public static StoryPoints Empty { get; } = new();

        public override string ToString()
        {
            return IsNull
                ? "- SP"
                : $"{Value} SP";
        }

        public string ToString(string format)
        {
            return IsNull
                ? "- SP"
                : Value.ToString(format) + " SP";
        }

        public string ToStandardDigitsString()
        {
            return IsNull
                ? "- SP"
                : IsEmpty
                    ? "0 SP"
                    : Value.ToString("0.0000") + " SP";
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
            return storyPoints.IsNull
                ? null
                : storyPoints.Value;
        }

        public static implicit operator StoryPoints(float? storyPoints)
        {
            if (storyPoints == null)
                return new StoryPoints
                {
                    Value = 0,
                    IsNull = true
                };

            return new StoryPoints
            {
                Value = storyPoints.Value
            };
        }
    }
}