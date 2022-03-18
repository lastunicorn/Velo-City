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
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class SprintOverviewViewModel
    {
        private readonly AnalyzeSprintResponse response;

        public SprintOverviewViewModel(AnalyzeSprintResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public string Title => $"{response.SprintName} ({response.StartDate:d} - {response.EndDate:d})";

        public string State => response.SprintState switch
        {
            SprintState.Unknown => "unknown",
            SprintState.New => "new",
            SprintState.InProgress => "in progress",
            SprintState.Closed => "closed",
            _ => throw new ArgumentOutOfRangeException()
        };

        public int? WorkDays => response.WorkDays?.Count;

        public int TotalWorkHours => response.TotalWorkHours;

        public string EstimatedStoryPoints => response.EstimatedStoryPoints == null
            ? "-"
            : response.EstimatedStoryPoints.ToString();

        public string EstimatedVelocity => response.EstimatedVelocity == null
            ? "-"
            : response.EstimatedVelocity.ToString();

        public int CommitmentStoryPoints => response.CommitmentStoryPoints;

        public int ActualStoryPoints => response.ActualStoryPoints;

        public float ActualVelocity => response.ActualVelocity;

        public List<string> Notes
        {
            get
            {
                List<string> notes = new();

                bool previousSprintsExist = response.PreviousSprints is { Count: > 0 };

                if (previousSprintsExist)
                {
                    string previousSprints = string.Join(", ", response.PreviousSprints);
                    notes.Add($"The estimations were calculated based on previous {response.PreviousSprints.Count} closed sprints: {previousSprints}");
                }
                else
                {
                    notes.Add("Could not calculate an estimation because no previous closed sprints exist.");
                }

                if (response.ExcludesSprints is { Count: > 0 })
                {
                    string excludedSprints = string.Join(",", response.ExcludesSprints);
                    notes.Add($"Excluded sprints: {excludedSprints} (These sprints were excluded from the velocity calculation algorithm.)");
                }

                return notes;
            }
        }
    }
}