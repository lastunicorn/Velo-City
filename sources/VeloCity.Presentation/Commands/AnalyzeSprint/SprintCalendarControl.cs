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
using System.Linq;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    internal class SprintCalendarControl : Control
    {
        private readonly DataGridFactory dataGridFactory;

        public List<DateTime> WorkDays { get; set; }

        public List<SprintMember> SprintMembers { get; set; }

        public SprintCalendarControl(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory ?? throw new ArgumentNullException(nameof(dataGridFactory));
        }

        protected override void DoDisplay()
        {
            if (WorkDays == null || WorkDays.Count == 0)
                return;

            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = "Sprint Calendar";

            dataGrid.Columns.Add("Date");

            Column workColumn = new("Work")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(workColumn);

            dataGrid.Columns.Add(string.Empty);

            Column vacationColumn = new("Vacation")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(vacationColumn);

            dataGrid.Columns.Add("Details");

            foreach (DateTime date in WorkDays)
            {
                ContentRow dataRow = CreateContentRow(date);
                dataGrid.Rows.Add(dataRow);
            }

            dataGrid.Display();
        }

        private ContentRow CreateContentRow(DateTime date)
        {
            List<SprintMemberDay> sprintMemberDays = GetAllSprintMemberDays(date);
            int workHours = sprintMemberDays.Sum(x => x.WorkHours);
            int absenceHours = sprintMemberDays.Sum(x => x.AbsenceHours);

            ContentRow dataRow = new();

            dataRow.AddCell($"{date:d} ({date:dddd})");

            ContentCell workHoursCell = CreateWorkHoursCell(workHours);
            dataRow.AddCell(workHoursCell);

            ContentCell chartCell = CreateChartCell(workHours, absenceHours);
            dataRow.AddCell(chartCell);

            ContentCell absenceCell = CreateAbsenceCell(absenceHours);
            dataRow.AddCell(absenceCell);

            ContentCell detailsCell = CreateDetailsCell(sprintMemberDays);
            dataRow.AddCell(detailsCell);

            return dataRow;
        }

        private List<SprintMemberDay> GetAllSprintMemberDays(DateTime date)
        {
            if (SprintMembers == null)
                return new List<SprintMemberDay>();

            return SprintMembers
                .Select(x => x.Days?.FirstOrDefault(z => z.Date == date))
                .Where(x => x != null)
                .ToList();
        }

        private static ContentCell CreateWorkHoursCell(int workHours)
        {
            if (workHours == 0)
            {
                return new ContentCell
                {
                    Content = "- h"
                };
            }

            return new ContentCell
            {
                Content = $"{workHours} h",
                ForegroundColor = ConsoleColor.Green
            };
        }

        private static ContentCell CreateChartCell(int workHours, int absenceHours)
        {
            ChartBar chartBar = new()
            {
                MaxValue = workHours + absenceHours,
                Value = workHours
            };

            return new ContentCell
            {
                Content = chartBar.ToString(),
                ForegroundColor = ConsoleColor.Green
            };
        }

        private static ContentCell CreateAbsenceCell(int absenceHours)
        {
            if (absenceHours == 0)
            {
                return new ContentCell
                {
                    Content = "- h"
                };
            }

            return new ContentCell
            {
                Content = $"{absenceHours} h",
                ForegroundColor = ConsoleColor.Yellow
            };
        }

        private ContentCell CreateDetailsCell(IEnumerable<SprintMemberDay> sprintMemberDays)
        {
            List<string> absentTeamMemberNames = sprintMemberDays
                .Where(x => x.AbsenceHours > 0)
                .Select(x => x.WorkHours == 0
                    ? x.TeamMember.Name
                    : x.TeamMember.Name + "*")
                .ToList();

            string cellContent = absentTeamMemberNames.Count == SprintMembers.Count
                ? "All"
                : string.Join(", ", absentTeamMemberNames);

            return new ContentCell
            {
                Content = cellContent,
                ForegroundColor = ConsoleColor.Yellow
            };
        }
    }
}