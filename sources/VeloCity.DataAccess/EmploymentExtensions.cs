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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.JsonFiles.JsonFileModel;

namespace DustInTheWind.VeloCity.DataAccess;

internal static class EmploymentExtensions
{
    public static IEnumerable<JEmployment> ToJEntities(this IEnumerable<Employment> employments)
    {
        return employments?
            .Select(x => x.ToJEntity());
    }

    public static JEmployment ToJEntity(this Employment employment)
    {
        return new JEmployment
        {
            StartDate = employment.TimeInterval.StartDate,
            EndDate = employment.TimeInterval.EndDate,
            HoursPerDay = employment.HoursPerDay,
            WeekDays = ToJList(employment),
            Country = employment.Country
        };
    }

    private static List<JDayOfWeek> ToJList(Employment employment)
    {
        return employment.EmploymentWeek.IsDefault
            ? null
            : employment.EmploymentWeek
                .Select(x => x.ToJEntity())
                .ToList();
    }

    public static IEnumerable<Employment> ToEntities(this IEnumerable<JEmployment> employments)
    {
        return employments?
            .Select(x => x.ToEntity());
    }

    public static Employment ToEntity(this JEmployment employment)
    {
        return new Employment
        {
            TimeInterval = new DateInterval(employment.StartDate, employment.EndDate),
            HoursPerDay = employment.HoursPerDay,
            EmploymentWeek = employment.WeekDays == null
                ? EmploymentWeek.NewDefault
                : new EmploymentWeek(employment.WeekDays.Select(x => x.ToEntity())),
            Country = employment.Country
        };
    }
}