// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberEmployments;

public class EmploymentsViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;
    private List<EmploymentViewModel> employments;

    public List<EmploymentViewModel> Employments
    {
        get => employments;
        private set
        {
            employments = value;
            OnPropertyChanged();
        }
    }

    public EmploymentsViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
        eventBus.Subscribe<TeamMemberChangedEvent>(HandleSprintChangedEvent);
    }

    private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
    {
        await ReloadEmployments();
    }

    private async Task HandleSprintChangedEvent(TeamMemberChangedEvent ev, CancellationToken cancellationToken)
    {
        await ReloadEmployments();
    }

    private async Task ReloadEmployments()
    {
        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await requestBus.Send<PresentTeamMemberEmploymentsRequest, PresentTeamMemberEmploymentsResponse>(request);

        Employments = response.Employments
            .Select(x => new EmploymentViewModel(x))
            .ToList();
    }
}