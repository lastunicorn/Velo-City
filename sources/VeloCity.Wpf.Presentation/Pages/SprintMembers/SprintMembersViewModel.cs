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
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers
{
    public class SprintMembersViewModel
    {
        public List<SprintMemberOverviewViewModel> SprintMembersOverview { get; }
        
        public SprintMembersViewModel(PresentSprintResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            SprintMembersOverview = CreateSprintMemberOverviewItems(response.SprintMembers);
            CreateChartBars();
        }

        private static List<SprintMemberOverviewViewModel> CreateSprintMemberOverviewItems(IEnumerable<SprintMember> sprintMembers)
        {
            return sprintMembers
                .Select(x => new SprintMemberOverviewViewModel(x))
                .ToList();
        }

        private void CreateChartBars()
        {
            Chart chart = new()
            {
                ActualSize = 100
            };

            IEnumerable<ChartBar> chartBars = SprintMembersOverview
                .Select(x =>
                {
                    ChartBar chartBar = new()
                    {
                        MaxValue = x.WorkHours + x.AbsenceHours,
                        FillValue = x.WorkHours
                    };

                    x.ChartBar = chartBar;

                    return chartBar;
                });

            chart.AddRange(chartBars);
            chart.Calculate();
        }
    }
}