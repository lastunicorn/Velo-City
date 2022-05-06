// Velo City
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
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class EmploymentCollection : IEnumerable<Employment>
    {
        private readonly SortedList<DateTime, Employment> employments = new(new ReverseDuplicateKeyComparer<DateTime>());

        public EmploymentCollection()
        {
        }

        public EmploymentCollection(IEnumerable<Employment> employments)
        {
            IEnumerable<Employment> orderedEmployments = employments
                .OrderByDescending(x => x.TimeInterval.StartDate);

            foreach (Employment employment in orderedEmployments)
                AddInternal(employment);
        }

        public void Add(Employment employment)
        {
            if (employment == null) throw new ArgumentNullException(nameof(employment));

            AddInternal(employment);
        }

        private void AddInternal(Employment employment)
        {
            DateTime key = employment.TimeInterval.StartDate ?? DateTime.MinValue;
            employments.Add(key, employment);
        }
        public Employment GetFirstEmployment()
        {
            return employments.Values.LastOrDefault();
        }

        public Employment GetEmploymentFor(DateTime date)
        {
            return employments.Values.FirstOrDefault(x => x.TimeInterval.ContainsDate(date));
        }

        public IEnumerable<Employment> GetEmploymentBatchFor(DateTime date)
        {
            List<Employment> matchingBatch = GetEmploymentBatches()
                .Where(x =>
                {
                    Employment firstEmployment = x[^1];
                    DateTime? batchStartDate = firstEmployment.TimeInterval.StartDate;

                    Employment lastEmployment = x[0];
                    DateTime? batchEndDate = lastEmployment.TimeInterval.EndDate;

                    DateInterval batchTimeInterval = new(batchStartDate, batchEndDate);

                    return batchTimeInterval.ContainsDate(date);
                })
                .FirstOrDefault();

            return matchingBatch ?? Enumerable.Empty<Employment>();
        }

        public DateTime? GetStartDateForLastEmploymentBatch()
        {
            Employment lastEmployment = GetLastEmploymentBatch().LastOrDefault();

            return lastEmployment == null
                ? null
                : lastEmployment.TimeInterval.StartDate ?? DateTime.MinValue;
        }

        public IEnumerable<Employment> GetLastEmploymentBatch()
        {
            return GetEmploymentBatches().FirstOrDefault() ?? Enumerable.Empty<Employment>();
        }

        private IEnumerable<List<Employment>> GetEmploymentBatches()
        {
            List<Employment> batch = null;

            foreach (Employment employment in employments.Values)
            {
                if (batch == null)
                {
                    batch = new List<Employment> { employment };
                }
                else
                {
                    Employment lastEmployment = batch[^1];
                    if (employment.ContinuesWith(lastEmployment))
                    {
                        batch.Add(employment);
                    }
                    else
                    {
                        yield return batch;
                        batch = new List<Employment> { employment };
                    }
                }
            }

            if (batch != null)
                yield return batch;
        }

        public IEnumerator<Employment> GetEnumerator()
        {
            return employments.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}