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

namespace DustInTheWind.VeloCity.Domain;

internal class DateIntervalIntersection
{
    private readonly DateInterval dateInterval1;
    private readonly DateInterval dateInterval2;

    private DateTime startDate2;
    private DateTime endDate2;
    private DateTime startDate1;
    private DateTime endDate1;

    private bool isCalculated;
    private DateInterval? result;

    public DateInterval? Result
    {
        get
        {
            if (!isCalculated)
            {
                result = CalculateResult();
                isCalculated = true;
            }

            return result;
        }
    }

    public DateIntervalIntersection(DateInterval dateInterval1, DateInterval dateInterval2)
    {
        this.dateInterval1 = dateInterval1;
        this.dateInterval2 = dateInterval2;
    }

    private DateInterval? CalculateResult()
    {
        startDate1 = dateInterval1.StartDate ?? DateTime.MinValue;
        endDate1 = dateInterval1.EndDate ?? DateTime.MaxValue;

        startDate2 = dateInterval2.StartDate ?? DateTime.MinValue;
        endDate2 = dateInterval2.EndDate ?? DateTime.MaxValue;

        if (startDate2 < startDate1)
            return IntersectWhenSecondStartsBeforeFirst();

        if (startDate2 == startDate1)
            return IntersectWhenBothStartTogether();

        if (startDate2 <= endDate1)
            return IntersectWhenSecondStartsDuringFirst();
        
        return null;
    }

    private DateInterval? IntersectWhenSecondStartsBeforeFirst()
    {
        if (endDate2 < startDate1)
            return null;

        if (endDate2 < endDate1)
            return new DateInterval(dateInterval1.StartDate, dateInterval2.EndDate);

        return dateInterval1;
    }

    private DateInterval? IntersectWhenBothStartTogether()
    {
        if (endDate2 <= endDate1)
            return dateInterval2;

        return dateInterval1;
    }

    private DateInterval? IntersectWhenSecondStartsDuringFirst()
    {
        if (endDate2 <= endDate1)
            return dateInterval2;

        return new DateInterval(dateInterval2.StartDate, dateInterval1.EndDate);
    }

    public static implicit operator DateInterval?(DateIntervalIntersection intersection)
    {
        return intersection.Result;
    }
}