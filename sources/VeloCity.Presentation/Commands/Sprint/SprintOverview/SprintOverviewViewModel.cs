﻿// Velo City
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
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.Sprint.SprintOverview
{
    public class SprintOverviewViewModel
    {
        private readonly AnalyzeSprintResponse response;

        public string SprintName => response.SprintName;

        public DateTime StartDate => response.StartDate;

        public DateTime EndDate => response.EndDate;

        public string State => response.SprintState switch
        {
            SprintState.Unknown => "unknown",
            SprintState.New => "new",
            SprintState.InProgress => "in progress",
            SprintState.Closed => "closed",
            _ => throw new ArgumentOutOfRangeException()
        };

        public int? WorkDays => response.WorkDays?
            .Where(x => x.IsWorkDay)
            .Count();

        public int TotalWorkHours => response.TotalWorkHours;

        public StoryPoints EstimatedStoryPoints => response.EstimatedStoryPoints;

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties => response.EstimatedStoryPointsWithVelocityPenalties;

        public Velocity EstimatedVelocity => response.EstimatedVelocity;

        public StoryPoints CommitmentStoryPoints => response.CommitmentStoryPoints;

        public StoryPoints ActualStoryPoints => response.ActualStoryPoints;

        public Velocity ActualVelocity => response.ActualVelocity;

        public List<NoteBase> Notes
        {
            get
            {
                List<NoteBase> notes = new();

                bool previousSprintsExist = response.PreviousSprints is { Count: > 0 };

                if (previousSprintsExist)
                {
                    notes.Add(new PreviousSprintsCalculationNote
                    {
                        PreviousSprintNumbers = response.PreviousSprints
                    });
                }
                else
                {
                    notes.Add(new NoPreviousSprintsNote());
                }

                if (response.ExcludedSprints is { Count: > 0 })
                {
                    notes.Add(new ExcludedSprintsNote
                    {
                        ExcludesSprintNumbers = response.ExcludedSprints
                    });
                }

                if (response.EstimatedStoryPointsWithVelocityPenalties != null)
                {
                    notes.Add(new VelocityPenaltiesNote
                    {
                        VelocityPenalties = response.VelocityPenalties
                    });
                }

                return notes;
            }
        }

        public SprintOverviewViewModel(AnalyzeSprintResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
        }
    }
}