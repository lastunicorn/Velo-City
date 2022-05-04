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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class EmploymentCollection : Collection<Employment>
    {
        public EmploymentCollection(IEnumerable<Employment> employments)
        {
            foreach (Employment employment in employments)
                Items.Add(employment);
        }

        public Employment GetEmploymentFor(DateTime date)
        {
            return Items.FirstOrDefault(x => x.TimeInterval.ContainsDate(date));
        }

        public Employment GetEmploymentFor(DateInterval dateInterval)
        {
            return Items.FirstOrDefault(x => x.TimeInterval.IsIntersecting(dateInterval));
        }

        public Employment GetLastEmployment()
        {
            //return Items
            //    .OrderByDescending(x => x.TimeInterval.StartDate)
            //    .FirstOrDefault();

            IEnumerable<Employment> employments = Items
                .OrderByDescending(x => x.TimeInterval.StartDate);

            Employment lastEmployment = null;

            foreach (Employment employment in employments)
            {
                bool isCandidate = lastEmployment == null ||
                                   employment.TimeInterval.StartDate == null ||
                                   employment.TimeInterval.EndDate == null ||
                                   employment.TimeInterval.EndDate.Value.AddDays(1) >= lastEmployment.TimeInterval.StartDate.Value;

                if (!isCandidate)
                    break;

                lastEmployment = employment;

                if (lastEmployment.TimeInterval.StartDate == null)
                    break;
            }

            return lastEmployment;
        }

        public DateTime? GetLastEmploymentDate()
        {
            Employment lastEmployment = GetLastEmployment();

            return lastEmployment == null
                ? null
                : lastEmployment.TimeInterval.StartDate ?? DateTime.MinValue;
        }
    }
}