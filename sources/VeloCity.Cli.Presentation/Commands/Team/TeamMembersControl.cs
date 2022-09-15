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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Team
{
    internal class TeamMembersControl : BlockControl
    {
        private readonly DataGridFactory dataGridFactory;

        public TeamMembersControl(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        public List<TeamMember> TeamMembers { get; set; }

        protected override void DoDisplayContent(ControlDisplay display)
        {
            if (TeamMembers == null)
                return;

            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = $"Team ({TeamMembers.Count} members)";
            dataGrid.DisplayBorderBetweenRows = true;

            foreach (TeamMember teamMember in TeamMembers)
            {
                ContentRow row = new();

                ContentCell nameCell = CreateNameCell(teamMember);
                row.AddCell(nameCell);

                ContentCell employmentsCell = CreateEmploymentsCell(teamMember);
                row.AddCell(employmentsCell);

                dataGrid.Rows.Add(row);
            }

            dataGrid.Display();
        }

        private static ContentCell CreateNameCell(TeamMember teamMember)
        {
            return new ContentCell()
            {
                Content = teamMember.Name.FullNameWithNickname
            };
        }

        private static ContentCell CreateEmploymentsCell(TeamMember teamMember)
        {
            IEnumerable<string> employmentsAsString = teamMember.Employments
                .Select(RenderEmployment);

            return new ContentCell()
            {
                Content = string.Join(Environment.NewLine, employmentsAsString)
            };
        }

        private static string RenderEmployment(Employment employment)
        {
            List<string> items = new();

            items.Add($"{employment.HoursPerDay.Value} h/day");

            if (employment.EmploymentWeek is { IsDefault: false })
            {
                IEnumerable<string> shortDayNames = employment.EmploymentWeek
                    .Select(x => CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(x));

                string workingDaysAsString = string.Join(", ", shortDayNames);

                if (string.IsNullOrEmpty(workingDaysAsString))
                    workingDaysAsString = "<none>";

                items.Add(workingDaysAsString);
            }

            items.Add(employment.TimeInterval.ToString());

            return string.Join(" | ", items);
        }
    }
}