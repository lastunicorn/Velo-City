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

        public SprintMemberDayCollection Days { get; }

        public bool IsEmployed => Days
            .Any(x => x.AbsenceReason != AbsenceReason.Unemployed);

        public HoursValue WorkHours => Days
            .Select(x => x.WorkHours)
            .Sum(x => x.Value);

        public HoursValue WorkHoursWithVelocityPenalties
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
                    .Where(x => Sprint.Number >= x.Sprint?.Number && Sprint.Number < x.Sprint?.Number + x.Duration)
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

            IEnumerable<SprintMemberDay> sprintMemberDays = Sprint.EnumerateAllDays()
                .Select(x => new SprintMemberDay(TeamMember, x));

            Days = new SprintMemberDayCollection(sprintMemberDays);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}