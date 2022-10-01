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
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintMembers;
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintOverview;
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.TeamOverview;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.SprintCalendar;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint
{
    public class SprintView : IView<SprintCommand>
    {
        private readonly DataGridFactory dataGridFactory;

        public SprintView(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public void Display(SprintCommand command)
        {
            DisplayOverview(command);
            DisplaySprintCalendar(command);

            if (command.ShowTeam)
            {
                foreach (SprintMember sprintMember in command.SprintMembers)
                    DisplaySprintMemberDetails(sprintMember);
            }
            else
            {
                DisplayTeamOverview(command);
            }
        }

        private void DisplayOverview(SprintCommand command)
        {
            SprintOverviewControl sprintOverviewControl = new(dataGridFactory)
            {
                ViewModel = command.SprintOverviewViewModel
            };
            sprintOverviewControl.Display();
        }

        private void DisplaySprintCalendar(SprintCommand command)
        {
            SprintCalendarControl sprintCalendarControl = new(dataGridFactory)
            {
                ViewModel = command.SprintCalendarViewModel
            };

            sprintCalendarControl.Display();
        }

        private void DisplaySprintMemberDetails(SprintMember sprintMember)
        {
            SprintMemberDetailsControl sprintMemberDetailsControl = new(dataGridFactory)
            {
                SprintMember = sprintMember
            };
            sprintMemberDetailsControl.Display();
        }

        private void DisplayTeamOverview(SprintCommand command)
        {
            TeamOverviewControl teamOverviewControl = new(dataGridFactory)
            {
                ViewModel = command.TeamOverviewViewModel
            };
            teamOverviewControl.Display();
        }
    }
}