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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMemberCalendar;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar
{
    public class SprintMemberCalendarViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private string title;
        private List<SprintMemberCalendarDayViewModel> days;
        private SprintMember sprintMember;

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

        public SprintMemberCalendarViewModel(IMediator mediator, EventBus eventBus)
        {
            if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            eventBus.Subscribe<TeamMemberVacationChangedEvent>(HandleTeamMemberVacationChangedEvent);
        }

        private async Task HandleTeamMemberVacationChangedEvent(TeamMemberVacationChangedEvent ev, CancellationToken cancellationToken)
        {
            if (sprintMember is { Sprint: { }, TeamMember: { } })
            {
                int teamMemberId = sprintMember.TeamMember.Id;
                int sprintId = sprintMember.Sprint.Id;

                await RefreshData(teamMemberId, sprintId, cancellationToken);
            }
        }

        public void SetSprintMember(int teamMemberId, int sprintId)
        {
            _ = RefreshData(teamMemberId, sprintId);
        }

        private async Task RefreshData(int teamMemberId, int sprintId, CancellationToken cancellationToken = default)
        {
            PresentSprintMemberCalendarRequest request = new()
            {
                TeamMemberId = teamMemberId,
                SprintId = sprintId
            };

            PresentSprintMemberCalendarResponse response = await mediator.Send(request, cancellationToken);

            sprintMember = response.SprintMembers;

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