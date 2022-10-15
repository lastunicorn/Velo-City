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
using System.Reflection;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentMain;
using DustInTheWind.VeloCity.Wpf.Presentation.ChartsArea.Charts;
using DustInTheWind.VeloCity.Wpf.Presentation.Commands;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.CloseSprintConfirmation;
using DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.Sprints;
using DustInTheWind.VeloCity.Wpf.Presentation.TeamMembersArea.Team;

namespace DustInTheWind.VeloCity.Wpf.Presentation.MainArea.Main
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IRequestBus requestBus;
        private readonly EventBus eventBus;
        private string databaseConnectionString;
        private SprintsPageViewModel sprintsPageViewModel;
        private TeamPageViewModel teamPageViewModel;
        private ChartsPageViewModel chartsPageViewModel;
        private RefreshCommand refreshCommand;
        private ViewModelBase popupPageViewModel;

        public string Title
        {
            get
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                AssemblyName assemblyName = assembly.GetName();
                Version version = assemblyName.Version;

                return $"VeloCity {version.ToString(3)} beta";
            }
        }

        public string DatabaseConnectionString
        {
            get => databaseConnectionString;
            set
            {
                databaseConnectionString = value;
                OnPropertyChanged();
            }
        }

        public SprintsPageViewModel SprintsPageViewModel => sprintsPageViewModel ??= new SprintsPageViewModel(requestBus, eventBus);

        public TeamPageViewModel TeamPageViewModel => teamPageViewModel ??= new TeamPageViewModel(requestBus, eventBus);

        public ChartsPageViewModel ChartsPageViewModel => chartsPageViewModel ??= new ChartsPageViewModel(requestBus, eventBus);

        public ViewModelBase PopupPageViewModel
        {
            get => popupPageViewModel;
            set
            {
                popupPageViewModel = value;
                OnPropertyChanged();
            }
        }

        public RefreshCommand RefreshCommand => refreshCommand ??= new RefreshCommand(requestBus);

        public MainViewModel(IRequestBus requestBus, EventBus eventBus)
        {
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            _ = Initialize();
        }

        private async Task Initialize()
        {
            PresentMainRequest request = new();
            PresentMainResponse response = await requestBus.Send<PresentMainRequest, PresentMainResponse>(request);

            DatabaseConnectionString = response.DatabaseConnectionString;

            await Task.Delay(1000);

            PopupPageViewModel = new SprintCloseConfirmationViewModel
            {
                SprintNumber = 23,
                SprintName = "My sprint"
            };
        }
    }
}