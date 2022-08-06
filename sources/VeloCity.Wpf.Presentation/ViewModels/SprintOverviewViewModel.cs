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
using DustInTheWind.VeloCity.Wpf.Application.PresentSprint;
using DustInTheWind.VeloCity.Wpf.Presentation.Styles;

namespace DustInTheWind.VeloCity.Wpf.Presentation.ViewModels
{
    public class SprintOverviewViewModel
    {
        public List<PropertyGroup> PropertyGroups { get; set; }

        public List<NoteBase> Notes { get; }

        public SprintOverviewViewModel(PresentSprintResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            PropertyGroups = CreatePropertyGroups(response);

            Notes = CreateNotes(response).ToList();
        }

        private static List<PropertyGroup> CreatePropertyGroups(PresentSprintResponse response)
        {
            DateTime? startDate = response.SprintDateInterval.StartDate;
            DateTime? endDate = response.SprintDateInterval.EndDate;

            PropertyGroup overviewGroup = new("Overview")
            {
                Items = new List<PropertyGroupItem>
                {
                    new("Time Interval", new DateIntervalViewModel(startDate, endDate)),
                    new("State", new SprintStateViewModel(response.SprintState))
                }
            };

            PropertyGroup sizeGroup = new("Size")
            {
                Items = new List<PropertyGroupItem>
                {
                    new("Work Days", new DaysViewModel(response.WorkDaysCount)),
                    new("Total Work Hours", response.TotalWorkHours)
                }
            };
            
            PropertyGroup beforeStartingGroup = new("Before Starting")
            {
                Items = new List<PropertyGroupItem>()
            };

            PropertyGroupItem estimatedStoryPointsItem = new("Estimated Story Points", new StoryPointsViewModel(response.EstimatedStoryPoints));
            beforeStartingGroup.Items.Add(estimatedStoryPointsItem);

            if (!response.EstimatedStoryPointsWithVelocityPenalties.IsNull)
            {
                PropertyGroupItem estimatedStoryPointsWithVelocityPenaltiesItem = new("Estimated Story Points (*)", new StoryPointsViewModel(response.EstimatedStoryPointsWithVelocityPenalties));
                beforeStartingGroup.Items.Add(estimatedStoryPointsWithVelocityPenaltiesItem);
            }

            PropertyGroupItem estimatedVelocityItem = new("Estimated Velocity", new VelocityViewModel(response.EstimatedVelocity));
            beforeStartingGroup.Items.Add(estimatedVelocityItem);
            
            PropertyGroupItem commitmentStoryPointsItem = new("Commitment Story Points", response.CommitmentStoryPoints);
            beforeStartingGroup.Items.Add(commitmentStoryPointsItem);
            
            PropertyGroup afterCloseGroup = new("After Close")
            {
                Items = new List<PropertyGroupItem>
                {
                    new("Actual Story Points", new StoryPointsViewModel(response.ActualStoryPoints)),
                    new("Actual Velocity", new VelocityViewModel(response.ActualVelocity))
                }
            };

            return new List<PropertyGroup>
            {
                overviewGroup,
                sizeGroup,
                beforeStartingGroup,
                afterCloseGroup
            };
        }

        private static IEnumerable<NoteBase> CreateNotes(PresentSprintResponse response)
        {
            bool previousSprintsExist = response.PreviouslyClosedSprints is { Count: > 0 };

            if (previousSprintsExist)
            {
                yield return new PreviousSprintsCalculationNote
                {
                    PreviousSprintNumbers = response.PreviouslyClosedSprints
                };
            }
            else
            {
                yield return new NoPreviousSprintsNote();
            }

            if (response.ExcludedSprints is { Count: > 0 })
            {
                yield return new ExcludedSprintsNote
                {
                    ExcludesSprintNumbers = response.ExcludedSprints
                };
            }

            if (response.EstimatedStoryPointsWithVelocityPenalties.IsNotNull)
            {
                yield return new VelocityPenaltiesNote
                {
                    VelocityPenalties = response.VelocityPenalties
                };
            }
        }
    }
}