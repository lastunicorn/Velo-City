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
using DustInTheWind.VeloCity.Cli.Application.PresentSprint;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintOverview
{
    public class SprintOverviewViewModel
    {
        private readonly PresentSprintResponse response;

        public string SprintName => response.SprintName;

        public DateTime StartDate => response.SprintDateInterval.StartDate!.Value;

        public DateTime EndDate => response.SprintDateInterval.EndDate!.Value;

        public SprintStateViewModel State { get; }

        public int? WorkDaysCount { get; }

        public HoursValue TotalWorkHours => response.TotalWorkHours;

        public StoryPoints EstimatedStoryPoints => response.EstimatedStoryPoints;

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties => response.EstimatedStoryPointsWithVelocityPenalties;

        public Velocity EstimatedVelocity => response.EstimatedVelocity;

        public StoryPoints CommitmentStoryPoints => response.CommitmentStoryPoints;

        public StoryPoints ActualStoryPoints => response.ActualStoryPoints;

        public Velocity ActualVelocity => response.ActualVelocity;

        public List<NoteBase> Notes { get; }

        public SprintOverviewViewModel(PresentSprintResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));

            State = new SprintStateViewModel(response.SprintState);
            WorkDaysCount = response.WorkDaysCount;
            Notes = CreateNotes().ToList();
        }

        private IEnumerable<NoteBase> CreateNotes()
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