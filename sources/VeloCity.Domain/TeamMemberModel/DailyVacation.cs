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

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class DailyVacation : Vacation
{
    private DateInterval dateInterval;

    public DateTime? StartDate => dateInterval.StartDate;

    public DateTime? EndDate => dateInterval.EndDate;

    public DateInterval DateInterval
    {
        get => dateInterval;
        set
        {
            dateInterval = value;
            OnChanged();
        }
    }

    public override bool Match(DateTime date)
    {
        return DateInterval.ContainsDate(date);
    }

    public void ChangeStartDate(DateTime? startDate)
    {
        DateInterval = DateInterval.ChangeStartDate(startDate);
    }

    public void ChangeEndDate(DateTime? endDate)
    {
        DateInterval = DateInterval.ChangeEndDate(endDate);
    }

    public void ExtendRight(uint dayCount)
    {
        DateInterval = DateInterval.InflateRight(dayCount);
    }

    public void ExtendLeft(uint dayCount)
    {
        DateInterval = DateInterval.InflateLeft(dayCount);
    }

    public uint CountDaysBefore(DateTime date)
    {
        if (date == DateTime.MinValue)
            return 0;

        DateInterval chopInterval = new(null, date.AddDays(-1));

        DateInterval? intersection = DateInterval.Intersect(DateInterval, chopInterval);
        return intersection?.TotalDays ?? 0;
    }

    public uint CountDaysAfter(DateTime date)
    {
        if (date == DateTime.MaxValue)
            return 0;

        DateInterval chopInterval = new(date.AddDays(1));

        DateInterval? intersection = DateInterval.Intersect(DateInterval, chopInterval);
        return intersection?.TotalDays ?? 0;
    }
}