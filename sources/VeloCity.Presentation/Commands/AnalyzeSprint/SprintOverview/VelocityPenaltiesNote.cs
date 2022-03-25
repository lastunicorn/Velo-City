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

using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Presentation.UserControls;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint.SprintOverview
{
    public class VelocityPenaltiesNote : NoteBase
    {
        public List<VelocityPenaltyInfo> VelocityPenalties { get; set; }

        protected override string BuildMessage()
        {
            if (VelocityPenalties == null)
                return "(*) The sprint includes velocity penalties.";

            IEnumerable<string> items = VelocityPenalties
                .Select(x => $"{x.PersonName.ShortName} ({x.PenaltyValue}%)");
            string allItems = string.Join(", ", items);
            return $"(*) The sprint includes velocity penalties for: {allItems}.";
        }
    }
}