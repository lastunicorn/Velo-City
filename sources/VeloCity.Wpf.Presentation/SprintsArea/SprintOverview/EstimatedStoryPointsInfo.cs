﻿// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview;

internal class EstimatedStoryPointsInfo : InfoBase
{
    public List<int> PreviousSprintNumbers { get; set; }

    protected override IEnumerable<string> BuildMessage()
    {
        string previousSprints = string.Join(", ", PreviousSprintNumbers);
        yield return $"Story points that the team can burn if they will have the same velocity as the average from the last {PreviousSprintNumbers.Count} closed sprints: {previousSprints}";

        yield return "Estimated Capacity = Estimated Burn Velocity * Total Work Hours";
    }
}