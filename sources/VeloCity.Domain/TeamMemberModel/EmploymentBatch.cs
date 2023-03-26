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

using System;
using System.Collections;
using System.Collections.Generic;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

/// <summary>
/// An employment batch is a list of continuous employments.
/// </summary>
public class EmploymentBatch : IEnumerable<Employment>
{
    private readonly List<Employment> employments = new();

    public DateTime? StartDate
    {
        get
        {
            if (employments.Count == 0)
                return null;

            Employment oldestEmployment = employments[^1];
            return oldestEmployment.TimeInterval.StartDate;
        }
    }

    public EmploymentBatch()
    {
    }

    public EmploymentBatch(Employment employment)
    {
        if (employment == null) throw new ArgumentNullException(nameof(employment));

        employments.Add(employment);
    }

    public bool TryAddBeforeOldest(Employment employment)
    {
        if (employment == null) throw new ArgumentNullException(nameof(employment));

        if (employments.Count == 0)
        {
            employments.Add(employment);
            return true;
        }

        Employment lastEmployment = employments[^1];

        if (employment.DoesContinueWith(lastEmployment))
        {
            employments.Add(employment);
            return true;
        }

        return false;
    }

    public bool ContainsDate(DateTime date)
    {
        if (employments.Count == 0)
            return false;

        Employment oldestEmployment = employments[^1];
        DateTime? batchStartDate = oldestEmployment.TimeInterval.StartDate;

        Employment newestEmployment = employments[0];
        DateTime? batchEndDate = newestEmployment.TimeInterval.EndDate;

        DateInterval batchTimeInterval = new(batchStartDate, batchEndDate);

        return batchTimeInterval.ContainsDate(date);
    }

    public IEnumerator<Employment> GetEnumerator()
    {
        return employments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}