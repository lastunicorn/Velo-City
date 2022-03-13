﻿// Velo City
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

namespace DustInTheWind.VeloCity.Domain
{
    internal class MemberDayAnalysis
    {
        public SprintDay SprintDay { get; set; }

        public List<Employment> Employments { get; set; }

        public List<Vacation> Vacations { get; set; }

        public int WorkHours { get; private set; }

        public int AbsenceHours { get; private set; }

        public AbsenceReason AbsenceReason { get; private set; }

        public string AbsenceComments { get; private set; }

        public void Analyze()
        {
            WorkHours = 0;
            AbsenceHours = 0;
            AbsenceReason = AbsenceReason.None;
            AbsenceComments = null;

            if (SprintDay == null)
                return;

            Employment employment = GetEmploymentFor(SprintDay.Date);

            bool isEmployed = employment != null;
            if (!isEmployed)
            {
                AbsenceReason = AbsenceReason.Unemployed;

                return;
            }

            if (SprintDay.IsWeekEnd)
            {
                AbsenceHours = employment.HoursPerDay;
                AbsenceReason = AbsenceReason.WeekEnd;

                return;
            }

            if (SprintDay.IsOfficialHoliday)
            {
                AbsenceHours = employment.HoursPerDay;
                AbsenceReason = AbsenceReason.OfficialHoliday;

                return;
            }

            Vacation[] vacations = GetVacationsFor(SprintDay.Date);

            bool vacationsExist = vacations is { Length: > 0 };

            if (vacationsExist)
            {
                Vacation[] wholeDayVacations = vacations
                    .Where(x => x.HourCount == null)
                    .ToArray();

                if (wholeDayVacations.Length > 0)
                {
                    AbsenceHours = employment.HoursPerDay;
                    AbsenceReason = AbsenceReason.Vacation;

                    AbsenceComments = CalculateAbsenceComments(wholeDayVacations);

                    return;
                }

                int vacationHours = vacations
                    .Where(x => x.HourCount != null)
                    .Sum(x => x.HourCount.Value);

                if (vacationHours > employment.HoursPerDay)
                {
                    AbsenceHours = employment.HoursPerDay;
                    AbsenceReason = AbsenceReason.Vacation;
                    AbsenceComments = CalculateAbsenceComments(vacations);

                    return;
                }

                WorkHours = employment.HoursPerDay - vacationHours;
                AbsenceHours = vacationHours;
                AbsenceReason = AbsenceReason.Vacation;
                AbsenceComments = CalculateAbsenceComments(vacations);

                return;
            }

            WorkHours = employment.HoursPerDay;
        }

        private static string CalculateAbsenceComments(IEnumerable<Vacation> vacations)
        {
            string[] vacationComments = vacations
                .Select(x => x.Comments)
                .Where(x => x != null)
                .ToArray();

            return vacationComments.Length > 0
                ? string.Join("; ", vacationComments)
                : null;
        }

        private Employment GetEmploymentFor(DateTime date)
        {
            return Employments?
                .FirstOrDefault(x => x.TimeInterval.ContainsDate(date));
        }

        private Vacation[] GetVacationsFor(DateTime date)
        {
            return Vacations?
                .Where(x => x.Match(date))
                .ToArray();
        }
    }
}