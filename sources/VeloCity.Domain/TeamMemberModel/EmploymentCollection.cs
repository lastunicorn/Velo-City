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

using System.Collections;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class EmploymentCollection : IEnumerable<Employment>
{
    private readonly SortedList<DateTime, Employment> employmentsByStartDate = new(new ReverseDuplicateKeyComparer<DateTime>());

    public EmploymentCollection()
    {
    }

    public EmploymentCollection(IEnumerable<Employment> employments)
    {
        if (employments == null)
            return;

        IEnumerable<Employment> orderedEmployments = employments
            .Where(x => x != null)
            .OrderByDescending(x => x.TimeInterval.StartDate);

        foreach (Employment employment in orderedEmployments)
            AddInternal(employment);
    }

    public void Add(Employment employment)
    {
        if (employment == null) throw new ArgumentNullException(nameof(employment));

        AddInternal(employment);
    }

    public void AddRange(IEnumerable<Employment> employments)
    {
        if (employments == null)
            return;

        IEnumerable<Employment> notNullEmployments = employments.Where(x => x != null);

        foreach (Employment employment in notNullEmployments)
            AddInternal(employment);
    }

    private void AddInternal(Employment employment)
    {
        DateTime key = employment.TimeInterval.StartDate ?? DateTime.MinValue;
        employmentsByStartDate.Add(key, employment);
    }

    public Employment GetFirstEmployment()
    {
        return employmentsByStartDate.Values.LastOrDefault();
    }

    public Employment GetLastEmployment()
    {
        return employmentsByStartDate.Values.FirstOrDefault();
    }

    public Employment GetEmploymentFor(DateTime date)
    {
        return employmentsByStartDate.Values.FirstOrDefault(x => x.TimeInterval.ContainsDate(date));
    }

    public EmploymentBatch GetEmploymentBatchFor(DateTime date)
    {
        EmploymentBatch matchingBatch = GetEmploymentBatches()
            .FirstOrDefault(x => x.ContainsDate(date));

        return matchingBatch ?? new EmploymentBatch();
    }

    public EmploymentBatch GetLastEmploymentBatch()
    {
        EmploymentBatch lastEmploymentBatch = GetEmploymentBatches().FirstOrDefault();
        return lastEmploymentBatch ?? new EmploymentBatch();
    }

    private IEnumerable<EmploymentBatch> GetEmploymentBatches()
    {
        EmploymentBatch batch = null;

        foreach (Employment employment in employmentsByStartDate.Values)
        {
            if (batch == null)
            {
                batch = new EmploymentBatch(employment);
            }
            else
            {
                bool success = batch.TryAddBeforeOldest(employment);

                if (!success)
                {
                    yield return batch;
                    batch = new EmploymentBatch(employment);
                }
            }
        }

        if (batch != null)
            yield return batch;
    }

    public IEnumerator<Employment> GetEnumerator()
    {
        return employmentsByStartDate.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}