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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class Sprint
    {
        public int Id
        {
            get => id;
            set
            {
                if (id != 0)
                    throw new InvalidOperationException("Once assigned, the id of a sprint cannot be changed.");

                id = value;
            }
        }

        public int Number { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; private set; } = DateTime.Today;

        public DateTime EndDate { get; private set; } = DateTime.Today;

        public DateInterval DateInterval
        {
            get => new(StartDate, EndDate);
            set
            {
                if (value.StartDate == null || value.EndDate == null)
                    throw new ArgumentException("The date interval cannot be infinite.", nameof(value));

                StartDate = value.StartDate.Value;
                EndDate = value.EndDate.Value;
            }
        }

        public StoryPoints CommitmentStoryPoints { get; set; }

        public StoryPoints ActualStoryPoints
        {
            get => actualStoryPoints;
            set
            {
                actualStoryPoints = value;

                velocity = null;
            }
        }

        public List<OfficialHoliday> OfficialHolidays { get; } = new();

        public SprintState State { get; set; }

        public string Goal { get; set; }

        public string Comments { get; set; }

        private readonly List<SprintMember> allSprintMembers = new();

        //public ObservableCollection<SprintMember> SprintMembers { get; } = new();

        private IEnumerable<SprintMember> SprintMembersWithoutExcluded
        {
            get
            {
                if (ExcludedTeamMembers is null or { Count: 0 })
                    return allSprintMembers;

                return allSprintMembers
                    .Where(x => !ExcludedTeamMembers.Any(z => x.Name.Contains(z)));
            }
        }

        public IEnumerable<SprintMember> SprintMembersOrderedByEmployment => SprintMembersWithoutExcluded
            .OrderBy(x => x.TeamMember.Employments.GetLastEmploymentBatch()?.StartDate)
            .ThenBy(x => x.Name);

        private Velocity? velocity;

        /// <summary>
        /// Velocity is calculated based on ActualStoryPoints and the total work hours of the sprint members.
        /// If each of these value is changed, the velocity must be recalculated.
        /// </summary>
        public Velocity Velocity
        {
            get
            {
                velocity ??= ComputeVelocity();
                return velocity.Value;
            }
        }

        private Velocity ComputeVelocity()
        {
            return ActualStoryPoints / TotalWorkHours;
        }

        private HoursValue? totalWorkHours;

        public HoursValue TotalWorkHours
        {
            get
            {
                totalWorkHours ??= CalculateTotalWorkHours();
                return totalWorkHours.Value;
            }
        }

        private HoursValue CalculateTotalWorkHours()
        {
            return SprintMembersWithoutExcluded
                .Sum(x => x.WorkHours.Value);
        }

        private HoursValue? totalWorkHoursWithVelocityPenalties;
        private StoryPoints actualStoryPoints;
        private int id;

        public HoursValue TotalWorkHoursWithVelocityPenalties
        {
            get
            {
                totalWorkHoursWithVelocityPenalties ??= CalculateTotalWorkHoursWithVelocityPenalties();
                return totalWorkHoursWithVelocityPenalties.Value;
            }
        }

        public IReadOnlyCollection<string> ExcludedTeamMembers { get; set; }

        public Sprint()
        {
        }

        private HoursValue CalculateTotalWorkHoursWithVelocityPenalties()
        {
            return SprintMembersWithoutExcluded
                .Sum(x => x.WorkHoursWithVelocityPenalties.Value);
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

        public int CountWorkDays()
        {
            return EnumerateAllDays()
                .Where(IsWorkDay)
                .Count();
        }

        private bool IsWorkDay(SprintDay sprintDay)
        {
            if (sprintDay.IsWeekEnd)
                return false;

            if (sprintDay.IsOfficialHoliday)
            {
                List<SprintMemberDay> sprintMemberDays = SprintMembersWithoutExcluded
                    .SelectMany(x => x.Days)
                    .Where(x => x.SprintDay.Date == sprintDay.Date)
                    .Where(x =>
                    {
                        Employment employment = x.TeamMember.Employments?.GetEmploymentFor(sprintDay.Date);
                        return !sprintDay.OfficialHolidays.Select(z => z.Country).Contains(employment?.Country);
                    })
                    .ToList();

                if (sprintMemberDays.Count == 0)
                    return false;
            }

            return true;
        }

        public void AddSprintMember(TeamMember teamMember)
        {
            if (teamMember == null) throw new ArgumentNullException(nameof(teamMember));

            AddSprintMemberInternal(teamMember);
        }

        public void AddSprintMembers(IEnumerable<TeamMember> teamMembers)
        {
            if (teamMembers == null) throw new ArgumentNullException(nameof(teamMembers));

            foreach (TeamMember teamMember in teamMembers)
                AddSprintMemberInternal(teamMember);
        }

        private void AddSprintMemberInternal(TeamMember teamMember)
        {
            SprintMember sprintMember = teamMember.ToSprintMember(this);
            sprintMember.VacationsChanged += HandleSprintMemberVacationsChanged;
            allSprintMembers.Add(sprintMember);
        }

        private void HandleSprintMemberVacationsChanged(object sender, EventArgs e)
        {
            totalWorkHours = null;
        }

        public List<VelocityPenaltyInstance> GetVelocityPenalties()
        {
            return SprintMembersWithoutExcluded
                .Where(x => x.VelocityPenaltyPercentage > 0)
                .Select(x => new VelocityPenaltyInstance
                {
                    Sprint = x.Sprint,
                    TeamMember = x.TeamMember,
                    Value = x.VelocityPenaltyPercentage
                })
                .ToList();
        }

        public override string ToString()
        {
            return $"{Number}: {Title}";
        }

        public SprintMember GetSprintMember(int teamMemberId)
        {
            return allSprintMembers.FirstOrDefault(x => x.TeamMember?.Id == teamMemberId);
        }
    }
}