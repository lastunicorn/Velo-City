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

using DustInTheWind.VeloCity.Cli.Application.PresentVacations;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Vacations;

public class TeamMemberVacationViewModel
{
    private readonly SortedList<DateMonth, MonthOfVacationsViewModel> groupedVacations = new();

    public PersonName PersonName { get; }

    public List<MonthOfVacationsViewModel> MonthsOfVacations { get; }

    public TeamMemberVacationViewModel(TeamMemberVacations teamMemberVacations)
    {
        if (teamMemberVacations == null) throw new ArgumentNullException(nameof(teamMemberVacations));

        PersonName = teamMemberVacations.PersonName;
        MonthsOfVacations = GroupVacationsByMonth(teamMemberVacations.Vacations);
    }

    private List<MonthOfVacationsViewModel> GroupVacationsByMonth(IEnumerable<Vacation> vacations)
    {
        IEnumerable<VacationViewModel> vacationViewModels = vacations
            .Select(VacationViewModel.From);

        foreach (VacationViewModel vacationViewModel in vacationViewModels)
        {
            if (vacationViewModel.StartDate == null || vacationViewModel.EndDate == null)
            {
                DateTime? date = vacationViewModel.SignificantDate;
                if (date != null)
                {
                    DateMonth dateTimeMonth = new(date.Value);
                    AddVacation(dateTimeMonth, vacationViewModel);
                }
            }
            else
            {
                DateTime date = vacationViewModel.StartDate.Value;
                DateMonth dateTimeMonth = new(date);

                while (dateTimeMonth <= vacationViewModel.EndDate.Value)
                {
                    AddVacation(dateTimeMonth, vacationViewModel);

                    dateTimeMonth = dateTimeMonth.AddMonths(1);
                }
            }
        }

        return groupedVacations.Values.ToList();
    }

    private void AddVacation(DateMonth dateTimeMonth, VacationViewModel vacationViewModel)
    {
        MonthOfVacationsViewModel monthOfVacationsViewModel;

        if (groupedVacations.ContainsKey(dateTimeMonth))
        {
            monthOfVacationsViewModel = groupedVacations[dateTimeMonth];
        }
        else
        {
            monthOfVacationsViewModel = new MonthOfVacationsViewModel(dateTimeMonth);
            groupedVacations.Add(dateTimeMonth, monthOfVacationsViewModel);
        }

        monthOfVacationsViewModel.Vacations.Add(vacationViewModel);
    }
}