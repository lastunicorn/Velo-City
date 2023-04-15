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

using System.Runtime.Serialization;

namespace DustInTheWind.VeloCity.JsonFiles;

public enum JDayOfWeek
{
    [EnumMember(Value = "sunday")]
    Sunday,

    [EnumMember(Value = "monday")]
    Monday,

    [EnumMember(Value = "tuesday")]
    Tuesday,

    [EnumMember(Value = "wednesday")]
    Wednesday,

    [EnumMember(Value = "thursday")]
    Thursday,

    [EnumMember(Value = "friday")]
    Friday,

    [EnumMember(Value = "saturday")]
    Saturday
}