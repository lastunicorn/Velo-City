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

using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview;

internal class EstimatedStoryPointsWithVelocityPenaltiesInfo : InfoBase
{
    public List<VelocityPenaltyDto> VelocityPenalties { get; set; }

    protected override IEnumerable<string> BuildMessage()
    {
        if (VelocityPenalties == null)
        {
            yield return "Same as the 'Estimated Capacity', but velocity penalties are applied for each team member.";
        }
        else
        {
            yield return "Same as the 'Estimated Capacity', but velocity penalties are applied for each team member:";

            IEnumerable<string> items = VelocityPenalties
                .Select(x => $"    - {x.PersonName.ShortName} ({x.PenaltyValue}%)");

            yield return string.Join(Environment.NewLine, items);
        }
    }
}