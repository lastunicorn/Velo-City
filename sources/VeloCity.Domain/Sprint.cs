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
    public class Sprint
    {
        private DateTime startDate;
        private DateTime endDate;

        public int Id { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public DateTime StartDate
        {
            get => startDate;
            set
            {
                if (value > EndDate)
                    throw new ArgumentException("StartDate cannot be grater than EndDate.", nameof(value));

                startDate = value;
            }
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (value < StartDate)
                    throw new ArgumentException("EndDate cannot be less than StartDate.", nameof(value));

                endDate = value;
            }
        }

        public DateInterval DateInterval => new(StartDate, EndDate);

        public StoryPoints CommitmentStoryPoints { get; set; }

        public StoryPoints ActualStoryPoints { get; set; }

        public List<OfficialHoliday> OfficialHolidays { get; } = new();

        public SprintState State { get; set; }

        public List<SprintMember> SprintMembers { get; } = new();

        public IEnumerable<SprintMember> SprintMembersOrderedByEmployment => SprintMembers
            .OrderBy(x =>
            {
                Employment employment = x.TeamMember.Employments.GetLastEmployment();
                return employment.TimeInterval.StartDate;
            })
            .ThenBy(x => x.Name);

        public void AddSprintMember(TeamMember teamMember)
        {
            SprintMember sprintMember = teamMember.ToSprintMember(this);

            if (sprintMember.IsEmployed)
                SprintMembers.Add(sprintMember);
        }

        public IEnumerable<SprintDay> EnumerateAllDays()
        {
            int totalDaysCount = (int)(EndDate.Date - StartDate.Date).TotalDays + 1;

            return Enumerable.Range(0, totalDaysCount)
                .Select(x =>
                {
                    DateTime date = StartDate.AddDays(x);
                    return ToSprintDay(date);
                });
        }

        public Velocity CalculateVelocity()
        {
            int totalWorkHours = SprintMembers
                .Select(x => x.WorkHours)
                .Sum(x => x.Value);

            return ActualStoryPoints / totalWorkHours;
        }

        public HoursValue CalculateTotalWorkHours()
        {
            return SprintMembers
                .Select(x => x.WorkHours)
                .Sum(x => x.Value);
        }

        public int CalculateTotalWorkHoursWithVelocityPenalties()
        {
            return SprintMembers
                .Select(x => x.WorkHoursWithVelocityPenalties)
                .Sum();
        }

        public List<VelocityPenaltyInstance> GetVelocityPenalties()
        {
            return SprintMembers
                .Where(x => x.VelocityPenaltyPercentage > 0)
                .Select(x => new VelocityPenaltyInstance
                {
                    Sprint = x.Sprint,
                    TeamMember = x.TeamMember,
                    Value = x.VelocityPenaltyPercentage
                })
                .ToList();
        }

        private SprintDay ToSprintDay(DateTime date)
        {
            return new SprintDay
            {
                Date = date,
                OfficialHolidays = OfficialHolidays
                    .Where(x => x.Match(date))
                    .Select(x => x.GetInstanceFor(date.Year))
                    .ToList()
            };
        }

        public override string ToString()
        {
            return $"{Number}: {Name}";
        }
    }
}