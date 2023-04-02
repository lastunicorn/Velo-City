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

using System.Collections.ObjectModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberVacations;
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentTeamMember;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.TeamMemberVacations;

public class VacationsViewModel : ViewModelBase
{
    private readonly IRequestBus requestBus;

    public ObservableCollection<VacationGroupViewModel> VacationGroups { get; } = new();

    public VacationsViewModel(IRequestBus requestBus, EventBus eventBus)
    {
        if (eventBus == null) throw new ArgumentNullException(nameof(eventBus));
        this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

        eventBus.Subscribe<ReloadEvent>(HandleReloadEvent);
        eventBus.Subscribe<TeamMemberChangedEvent>(HandleSprintChangedEvent);
        eventBus.Subscribe<TeamMemberVacationChangedEvent>(HandleTeamMemberVacationChangedEvent);
    }

    private async Task HandleReloadEvent(ReloadEvent ev, CancellationToken cancellationToken)
    {
        await ReloadVacations();
    }

    private async Task HandleSprintChangedEvent(TeamMemberChangedEvent ev, CancellationToken cancellationToken)
    {
        await ReloadVacations();
    }

    private async Task HandleTeamMemberVacationChangedEvent(TeamMemberVacationChangedEvent ev, CancellationToken cancellationToken)
    {
        await ReloadVacations();
    }

    private async Task ReloadVacations()
    {
        PresentTeamMemberVacationsRequest request = new();
        PresentTeamMemberVacationsResponse response = await requestBus.Send<PresentTeamMemberVacationsRequest, PresentTeamMemberVacationsResponse>(request);

        VacationGroups.Clear();
        GroupVacationsByMonth(response.Vacations);
    }

    private void GroupVacationsByMonth(IEnumerable<VacationInfo> vacationInfos)
    {
        IEnumerable<VacationViewModel> vacationViewModels = vacationInfos
            .Select(VacationViewModel.From)
            .OrderByDescending(x => x.SignificantDate);

        foreach (VacationViewModel vacationViewModel in vacationViewModels)
        {
            if (vacationViewModel.StartDate == null || vacationViewModel.EndDate == null)
            {
                DateTime? date = vacationViewModel.SignificantDate;
                if (date != null)
                {
                    DateTimeMonth dateTimeMonth = new(date.Value);
                    AddVacation(dateTimeMonth, vacationViewModel);
                }
            }
            else
            {
                DateTime date = vacationViewModel.StartDate.Value;
                DateTimeMonth dateTimeMonth = new(date);

                while (dateTimeMonth <= vacationViewModel.EndDate.Value)
                {
                    AddVacation(dateTimeMonth, vacationViewModel);

                    dateTimeMonth = dateTimeMonth.AddMonths(1);
                }
            }
        }
    }

    private void AddVacation(DateTimeMonth dateTimeMonth, VacationViewModel vacationViewModel)
    {
        VacationGroupViewModel vacationGroupViewModel = VacationGroups.FirstOrDefault(x => x.Month == dateTimeMonth);

        if (vacationGroupViewModel == null)
        {
            vacationGroupViewModel = new VacationGroupViewModel
            {
                Month = dateTimeMonth,
                Vacations = new List<VacationViewModel>()
            };
            VacationGroups.Add(vacationGroupViewModel);
        }

        vacationGroupViewModel.Vacations.Add(vacationViewModel);
    }
}