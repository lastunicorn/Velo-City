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

namespace DustInTheWind.VeloCity.Domain
{
    public class TeamMember
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Employment> Employments { get; set; }

        public string Comments { get; set; }

        public List<Vacation> Vacations { get; set; }

        public int CalculateWorkHoursFor(Sprint sprint)
        {
            return sprint.EnumerateWorkDays()
                .Select(CalculateWorkHoursFor)
                .Sum();
        }

        public SprintMember ToSprintMember(Sprint sprint)
        {
            return new SprintMember
            {
                Name = Name,
                Days = CalculateDays(sprint).ToList()
            };
        }

        private IEnumerable<SprintMemberDay> CalculateDays(Sprint sprint)
        {
            return sprint.EnumerateAllDays()
                .Select(x =>
                {
                    MemberDayAnalysis memberDayAnalysis = new()
                    {
                        SprintDay = x,
                        Employments = Employments,
                        Vacations = Vacations
                    };
                    memberDayAnalysis.Analyze();

                    return new SprintMemberDay
                    {
                        Date = x.Date,
                        WorkHours = memberDayAnalysis.WorkHours,
                        AbsenceHours = memberDayAnalysis.AbsenceHours,
                        AbsenceReason = memberDayAnalysis.AbsenceReason,
                        AbsenceComments = memberDayAnalysis.AbsenceComments
                    };

                    //return new SprintMemberDay
                    //{
                    //    Date = x.Date,
                    //    WorkHours = CalculateWorkHoursFor(x),
                    //    AbsenceHours = CalculateVacationHoursFor(x),
                    //    AbsenceReason = CalculateVacationReason(x),
                    //    AbsenceComments = GetVacationComments(x)
                    //};
                });
        }

        public int CalculateWorkHoursFor(SprintDay sprintDay)
        {
            Employment employment = GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
            if (!isEmployed)
                return 0;

            if (sprintDay.IsFreeDay)
                return 0;

            Vacation[] vacations = GetVacationsFor(sprintDay.Date);

            bool vacationsExist = vacations is { Length: > 0 };
            
            if (!vacationsExist)
                return employment.HoursPerDay;

            bool isWholeDayVacation = vacations.Any(x=>x.HourCount == null);

            if (isWholeDayVacation)
                return 0;

            int vacationHours = vacations
                .Where(x => x.HourCount != null)
                .Sum(x => x.HourCount.Value);

            if (vacationHours > employment.HoursPerDay)
                return 0;

            return employment.HoursPerDay - vacationHours;
        }

        private Vacation[] GetVacationsFor(DateTime date)
        {
            return Vacations?
                .Where(x => x.Match(date))
                .ToArray();
        }

        private int CalculateVacationHoursFor(SprintDay sprintDay)
        {
            Employment employment = GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
            if (!isEmployed)
                return 0;

            if (sprintDay.IsFreeDay)
                return employment.HoursPerDay;

            Vacation vacation = Vacations?.FirstOrDefault(x => x.Match(sprintDay.Date));

            if (vacation == null)
                return 0;

            return vacation.HourCount ?? employment.HoursPerDay;
        }

        private bool IsEmployed(DateTime date)
        {
            Employment employment = GetEmploymentFor(date);
            return employment != null;
        }

        private Employment GetEmploymentFor(DateTime date)
        {
            return Employments?
                .FirstOrDefault(x => x.TimeInterval.ContainsDate(date));
        }

        private AbsenceReason CalculateVacationReason(SprintDay sprintDay)
        {
            bool isEmployed = IsEmployed(sprintDay.Date);
            if (!isEmployed)
                return AbsenceReason.Unemployed;

            if (sprintDay.IsWeekEnd)
                return AbsenceReason.WeekEnd;

            if (sprintDay.IsOfficialHoliday)
                return AbsenceReason.OfficialHoliday;

            Vacation vacation = Vacations?.FirstOrDefault(x => x.Match(sprintDay.Date));

            return vacation != null
                ? AbsenceReason.Vacation
                : AbsenceReason.None;
        }

        private string GetVacationComments(SprintDay sprintDay)
        {
            Vacation vacation = Vacations?.FirstOrDefault(x => x.Match(sprintDay.Date));

            return vacation?.Comments;
        }
    }
}