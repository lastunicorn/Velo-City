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
using System.Reflection;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Wpf.Application.PresentMainView;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IMediator mediator;
        private List<SprintViewModel> sprints;
        private SprintOverviewViewModel sprintOverview;
        private SprintViewModel selectedSprint;
        private string detailsTitle;
        private SprintCalendarViewModel sprintCalendar;

        public string Title
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                AssemblyName assemblyName = assembly.GetName();
                Version version = assemblyName.Version;

                return $"VeloCity {version.ToString(3)}";
            }
        }

        public List<SprintViewModel> Sprints
        {
            get => sprints;
            private set
            {
                sprints = value;
                OnPropertyChanged();
            }
        }

        public SprintViewModel SelectedSprint
        {
            get => selectedSprint;
            set
            {
                selectedSprint = value;
                OnPropertyChanged();

                SprintOverview = null;
                DetailsTitle = BuildDetailsTitle();

                if (selectedSprint != null)
                    _ = RetrieveSprintDetails(selectedSprint.SprintId);
            }
        }

        private string BuildDetailsTitle()
        {
            return SelectedSprint == null
                ? null
                : $"{SelectedSprint.SprintName} ({selectedSprint.SprintNumber})";
        }

        public string DetailsTitle
        {
            get => detailsTitle;
            set
            {
                detailsTitle = value;
                OnPropertyChanged();
            }
        }

        public SprintOverviewViewModel SprintOverview
        {
            get => sprintOverview;
            private set
            {
                sprintOverview = value;
                OnPropertyChanged();
            }
        }

        public SprintCalendarViewModel SprintCalendar
        {
            get => sprintCalendar;
            set
            {
                sprintCalendar = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _ = Initialize();
        }

        private async Task Initialize()
        {
            PresentMainViewRequest request = new();

            PresentMainViewResponse response = await mediator.Send(request);

            Sprints = response.Sprints
                .Select(x => new SprintViewModel(x))
                .ToList();
        }

        private async Task RetrieveSprintDetails(int sprintId)
        {
            SprintOverview = null;
            SprintCalendar = null;

            PresentSprintRequest request = new()
            {
                SprintNumber = sprintId
            };

            PresentSprintResponse response = await mediator.Send(request);

            SprintOverview = new SprintOverviewViewModel(response);
            SprintCalendar = new SprintCalendarViewModel(response.SprintDays, response.SprintMembers);
        }
    }
}