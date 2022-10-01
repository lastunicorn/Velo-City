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

using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Cli.Application.PresentSprint;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.TeamOverview
{
    public class TeamOverviewViewModel
    {
        private readonly PresentSprintResponse response;

        public List<TeamMemberViewModel> TeamMembers => response.SprintMembers
            .Select(x => new TeamMemberViewModel(x))
            .ToList();

        public List<NoteBase> Notes { get; set; }

        public TeamOverviewViewModel(PresentSprintResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));

            Notes = new List<NoteBase> { new TeamDetailsNote() };
        }
    }
}