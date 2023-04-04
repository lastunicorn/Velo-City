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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberEmployments;

public class EmploymentViewModel
{
    public string TimeInterval { get; }

    public HoursValue HoursPerDay { get; }

    public EmploymentWeekViewModel EmploymentWeek { get; }

    public string Country { get; }

    public EmploymentViewModel(EmploymentInfo employmentInfo)
    {
        TimeInterval = BuildDateInterval(employmentInfo.TimeInterval);
        HoursPerDay = employmentInfo.HoursPerDay;
        EmploymentWeek = new EmploymentWeekViewModel(employmentInfo.EmploymentWeek);
        Country = employmentInfo.Country;
    }

    private static string BuildDateInterval(DateInterval dateInterval)
    {
        string startDate = dateInterval.StartDate == null
            ? ">>>"
            : dateInterval.StartDate.Value.ToString("dd MMM yyyy");

        string endDate = dateInterval.EndDate == null
            ? ">>>"
            : dateInterval.EndDate.Value.ToString("dd MMM yyyy");

        return $"{startDate} - {endDate}";
    }
}