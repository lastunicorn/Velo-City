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

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class Employment
{
    public DateTime? StartDate
    {
        get => TimeInterval.StartDate;
        set => TimeInterval = new DateInterval(value, TimeInterval.EndDate);
    }

    public DateTime? EndDate
    {
        get => TimeInterval.EndDate;
        set => TimeInterval = new DateInterval(TimeInterval.StartDate, value);
    }

    public DateInterval TimeInterval { get; set; }

    public HoursValue HoursPerDay { get; set; }

    public EmploymentWeek EmploymentWeek { get; set; }

    public string Country { get; set; }

    public bool ContainsDate(DateTime dateTime)
    {
        return TimeInterval.ContainsDate(dateTime);
    }

    public bool IsWorkDay(DayOfWeek dayOfWeek)
    {
        return EmploymentWeek?.IsWorkDay(dayOfWeek) ?? false;
    }

    public bool DoesContinueWith(Employment employment)
    {
        if (employment == null) throw new ArgumentNullException(nameof(employment));

        return TimeInterval.DoesContinueWith(employment.TimeInterval);
    }

    public override string ToString()
    {
        return $"{TimeInterval} | {HoursPerDay} | {EmploymentWeek}";
    }
}