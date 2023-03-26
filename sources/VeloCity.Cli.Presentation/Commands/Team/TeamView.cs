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
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Team
{
    public class TeamView : IView<TeamCommand>
    {
        private readonly DataGridFactory dataGridFactory;

        public TeamView(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public void Display(TeamCommand command)
        {
            Console.WriteLine(command.Information);

            if (command.TeamMembers == null || command.TeamMembers.Count == 0)
                CustomConsole.WriteLineWarning("There are no team members.");
            else
                DisplayTeamMembersGrid(command.TeamMembers);
        }

        private void DisplayTeamMembersGrid(List<TeamMember> teamMembers)
        {
            TeamMembersControl teamMembersControl = new(dataGridFactory)
            {
                TeamMembers = teamMembers
            };
            teamMembersControl.Display();
        }
    }
}