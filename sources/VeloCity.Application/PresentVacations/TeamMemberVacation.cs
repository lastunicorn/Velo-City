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
using System.Linq;

namespace DustInTheWind.VeloCity.Application.PresentVacations
{
    public class TeamMemberVacation
    {
        public string PersonName { get; set; }

        public List<VacationResponse> Vacations { get; set; }

        public SortedList<DateTime, List<VacationResponse>> VacationsMyMonth
        {
            get
            {
                Dictionary<DateTime, List<VacationResponse>> vacationByMonth = Vacations
                    .GroupBy(x => new DateTime(x.Date.Year, x.Date.Month, 1))
                    .OrderByDescending(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return new SortedList<DateTime, List<VacationResponse>>(vacationByMonth);
            }
        }
    }

    //public class VacationCollection
    //{
    //    private readonly Dictionary<DateTime, List<VacationInfo>> vacationByMonth = new();

    //    public void Add(VacationInfo item)
    //    {
    //        if (item == null) throw new ArgumentNullException(nameof(item));

    //        DateTime key = new(item.Date.Year, item.Date.Month, 1);

    //        List<VacationInfo> bucket;

    //        if (vacationByMonth.ContainsKey(key))
    //        {
    //            bucket = vacationByMonth[key];
    //        }
    //        else
    //        {
    //            bucket = new List<VacationInfo>();
    //            vacationByMonth.Add(key, bucket);
    //        }

    //        bucket.Add(item);
    //    }

    //    public IEnumerable<List<VacationInfo>> EnumerateByMonth()
    //    {
    //        return vacationByMonth
    //            .OrderByDescending(x => x.Key)
    //            .Select(x => x.Value);
    //    }
    //}
}