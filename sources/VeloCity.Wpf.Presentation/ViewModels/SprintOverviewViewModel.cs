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
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Styles;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ViewModels
{
    public class SprintOverviewViewModel
    {
        public List<PropertyGroup> PropertyGroups { get; }

        public SprintOverviewViewModel(PresentSprintResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            DateTime? startDate = response.SprintDateInterval.StartDate;
            DateTime? endDate = response.SprintDateInterval.EndDate;

            PropertyGroups = new List<PropertyGroup>
            {
                new("Overview")
                {
                    Items = new List<PropertyGroupItem>
                    {
                        new("Time Interval", new DateIntervalViewModel(startDate, endDate)),
                        new("State", new SprintStateViewModel(response.SprintState))
                    }
                },
                new("Size")
                {
                    Items = new List<PropertyGroupItem>
                    {
                        new("Work Days", new DaysViewModel(response.WorkDaysCount)),
                        new("Total Work Hours", response.TotalWorkHours)
                    }
                },
                new("Before Starting")
                {
                    Items = new List<PropertyGroupItem>
                    {
                        new("Estimated Story Points", new StoryPointsViewModel(response.EstimatedStoryPoints)),
                        new("Estimated Velocity", new VelocityViewModel(response.EstimatedVelocity)),
                        new("Commitment Story Points", response.CommitmentStoryPoints)
                    }
                },
                new("After Close")
                {
                    Items = new List<PropertyGroupItem>
                    {
                        new("Actual Story Points", new StoryPointsViewModel(response.ActualStoryPoints)),
                        new("Actual Velocity", new VelocityViewModel(response.ActualVelocity))
                    }
                }
            };
        }
    }
}