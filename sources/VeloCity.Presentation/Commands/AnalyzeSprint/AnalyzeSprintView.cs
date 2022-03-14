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
using DustInTheWind.VeloCity.Presentation.Infrastructure;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class AnalyzeSprintView : IView<AnalyzeSprintCommand>
    {
        private readonly DataGridFactory dataGridFactory;

        public AnalyzeSprintView(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public void Display(AnalyzeSprintCommand command)
        {
            DisplayOverview(command);

            if (command.WorkDays is { Count: > 0 })
                DisplayWorkDays(command);

            foreach (SprintMember sprintMember in command.SprintMembers)
                DisplaySprintMemberDetails(sprintMember);
        }

        private void DisplayOverview(AnalyzeSprintCommand command)
        {
            SprintOverviewControl sprintOverviewControl = new(dataGridFactory)
            {
                Command = command
            };
            sprintOverviewControl.Display();
        }

        private void DisplayWorkDays(AnalyzeSprintCommand command)
        {
            WorkDaysControl workDaysControl = new(dataGridFactory)
            {
                WorkDays = command.WorkDays,
                SprintMembers = command.SprintMembers
            };

            workDaysControl.Display();
        }

        private void DisplaySprintMemberDetails(SprintMember sprintMember)
        {
            SprintMemberDetailsControl sprintMemberDetailsControl = new(dataGridFactory)
            {
                SprintMember = sprintMember
            };
            sprintMemberDetailsControl.Display();
        }
    }
}