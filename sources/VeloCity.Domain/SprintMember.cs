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
    public class SprintMember
    {
        public PersonName Name => TeamMember.Name;

        public Sprint Sprint { get; }

        public TeamMember TeamMember { get; }

        public List<SprintMemberDay> Days => CalculateDays()
            .ToList();

        public bool IsEmployed => Days
            .Any(x => x.AbsenceReason != AbsenceReason.Unemployed);

        public int WorkHours => Days
            .Select(x => x.WorkHours)
            .Sum();

        public int AbsenceHours => Days
            .Select(x => x.AbsenceHours)
            .Sum();

        public int WorkHoursWithVelocityPenalties
        {
            get
            {
                int availabilityPercentage = 100 - VelocityPenaltyPercentage;
                return WorkHours * availabilityPercentage / 100;
            }
        }

        public List<VelocityPenalty> VelocityPenalties
        {
            get
            {
                if (TeamMember.VelocityPenalties == null)
                    return new List<VelocityPenalty>();

                return TeamMember.VelocityPenalties
                    .Where(x => Sprint.Number >= x.Sprint.Number && Sprint.Number < x.Sprint.Number + x.Duration)
                    .ToList();
            }
        }

        public int VelocityPenaltyPercentage
        {
            get
            {
                return VelocityPenalties
                    .Sum(x =>
                    {
                        int penaltyDuration = x.Duration;
                        int sprintCountFromPenaltyStart = Sprint.Number - x.Sprint.Number;
                        int sprintCountUntilNormal = penaltyDuration - sprintCountFromPenaltyStart;

                        return x.Value * sprintCountUntilNormal / penaltyDuration;
                    });
            }
        }

        public SprintMember(TeamMember teamMember, Sprint sprint)
        {
            TeamMember = teamMember ?? throw new ArgumentNullException(nameof(teamMember));
            Sprint = sprint ?? throw new ArgumentNullException(nameof(sprint));
        }

        private IEnumerable<SprintMemberDay> CalculateDays()
        {
            return Sprint?.EnumerateAllDays()
                .Select(x =>
                {
                    MemberDayAnalysis memberDayAnalysis = new()
                    {
                        SprintDay = x,
                        Employments = TeamMember?.Employments,
                        Vacations = TeamMember?.Vacations
                    };
                    memberDayAnalysis.Analyze();

                    return new SprintMemberDay
                    {
                        Date = x.Date,
                        TeamMember = TeamMember,
                        WorkHours = memberDayAnalysis.WorkHours,
                        AbsenceHours = memberDayAnalysis.AbsenceHours,
                        AbsenceReason = memberDayAnalysis.AbsenceReason,
                        AbsenceComments = memberDayAnalysis.AbsenceComments
                    };
                });
        }
        public int CalculateWorkHours()
        {
            return Sprint?.EnumerateWorkDays()
                .Select(CalculateWorkHoursFor)
                .Sum() ?? 0;
        }

        private int CalculateWorkHoursFor(SprintDay sprintDay)
        {
            Employment employment = TeamMember?.Employments?.GetEmploymentFor(sprintDay.Date);

            bool isEmployed = employment != null;
            if (!isEmployed)
                return 0;

            if (sprintDay.IsFreeDay)
                return 0;

            Vacation[] vacations = GetVacationsFor(sprintDay.Date);

            bool vacationsExist = vacations is { Length: > 0 };

            if (!vacationsExist)
                return employment.HoursPerDay;

            bool isWholeDayVacation = vacations.Any(x => x.HourCount == null);

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
            return TeamMember?.Vacations?
                .Where(x => x.Match(date))
                .ToArray();
        }
    }
}