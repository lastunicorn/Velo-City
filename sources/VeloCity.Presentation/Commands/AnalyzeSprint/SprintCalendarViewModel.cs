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
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class SprintCalendarViewModel
    {
        private readonly AnalyzeSprintResponse response;

        public List<CalendarItemViewModel> CalendarItems { get; }

        public bool IsVisible => response.WorkDays is { Count: > 0 };

        public bool IsPartialVacationNoteVisible => CalendarItems
            .SelectMany(x => x.VacationDetails)
            .Any(x => x.IsPartialVacation);

        public SprintCalendarViewModel(AnalyzeSprintResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));

            CalendarItems = CreateCalendarItems();
        }

        private List<CalendarItemViewModel> CreateCalendarItems()
        {
            return response.WorkDays
                .Select(CreateCalendarItem)
                .ToList();
        }

        private CalendarItemViewModel CreateCalendarItem(DateTime date)
        {
            List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(date);
            return new CalendarItemViewModel(sprintMemberDays, date);
        }

        private List<SprintMemberDay> GetAllSprintMemberDays(DateTime date)
        {
            if (response.SprintMembers == null)
                return new List<SprintMemberDay>();

            return response.SprintMembers
                .Select(x => x.Days?.FirstOrDefault(z => z.Date == date))
                .Where(x => x != null)
                .ToList();
        }
    }
}