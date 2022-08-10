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
using System.Reflection;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.Sprints;
using DustInTheWind.VeloCity.Wpf.Presentation.Pages.Team;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.Main
{
    public class MainViewModel : ViewModelBase
    {
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

        public SprintsPageViewModel SprintsPageViewModel { get; }
        
        public TeamPageViewModel TeamPageViewModel { get; }

        public MainViewModel(IMediator mediator)
        {
            if (mediator == null) throw new ArgumentNullException(nameof(mediator));

            SprintsPageViewModel = new SprintsPageViewModel(mediator);
            TeamPageViewModel = new TeamPageViewModel(mediator);
        }
    }
}