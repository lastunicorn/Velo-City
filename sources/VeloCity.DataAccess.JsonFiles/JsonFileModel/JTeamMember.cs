﻿// VeloCity
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

namespace DustInTheWind.VeloCity.JsonFiles.JsonFileModel;

public class JTeamMember
{
    public int Id { get; set; }

    [Obsolete("Use FirstName, MiddleName, LastName and Nickname instead.")]
    public string Name { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Nickname { get; set; }

    public List<JEmployment> Employments { get; set; }

    public string Comments { get; set; }

    public List<JVacationDay> VacationDays { get; set; }

    public List<JVelocityPenalty> VelocityPenalties { get; set; }
}