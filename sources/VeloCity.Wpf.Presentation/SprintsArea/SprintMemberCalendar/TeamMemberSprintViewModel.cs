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
using System.Linq;
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar
{
    public class TeamMemberSprintViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private string title;
        private List<SprintMemberCalendarDayViewModel> days;

        public string Title
        {
            get => title;
            private set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public List<SprintMemberCalendarDayViewModel> Days
        {
            get => days;
            private set
            {
                days = value;
                OnPropertyChanged();
            }
        }

        public TeamMemberSprintViewModel(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void SetSprintMember(SprintMember sprintMember)
        {
            Title = sprintMember.TeamMember.Name;
            Days = sprintMember.Days
                .Select(x => new SprintMemberCalendarDayViewModel(mediator, x))
                .ToList();

            CreateChartBars(Days);
        }

        private static void CreateChartBars(IEnumerable<SprintMemberCalendarDayViewModel> calendarItems)
        {
            SprintMemberWorkChart chart = new(calendarItems);

            foreach (ChartBarValue<SprintMemberCalendarDayViewModel> chartBarValue in chart)
            {
                if (chartBarValue.Item?.IsWorkDay == true)
                    chartBarValue.Item.ChartBarValue = chartBarValue;
            }
        }
    }
}