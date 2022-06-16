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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Application.PresentSprints
{
    public class SprintOverview
    {
        public string Name { get; }

        public int SprintNumber { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public HoursValue TotalWorkHours { get; }

        public StoryPoints CommitmentStoryPoints { get; }

        public StoryPoints ActualStoryPoints { get; }

        public Velocity ActualVelocity { get; }

        public SprintOverview(Sprint sprint)
        {
            Name = sprint.Name;
            SprintNumber = sprint.Number;
            StartDate = sprint.StartDate;
            EndDate = sprint.EndDate;
            TotalWorkHours = sprint.TotalWorkHours;
            CommitmentStoryPoints = sprint.CommitmentStoryPoints;
            ActualStoryPoints = sprint.ActualStoryPoints;
            ActualVelocity = sprint.Velocity;
        }
    }
}