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
    internal class WorkDaysControl : Control
    {
        public List<DateTime> WorkDays { get; set; }

        public List<SprintMember> SprintMembers { get; set; }

        protected override void DoDisplay()
        {
            if (WorkDays == null || WorkDays.Count == 0)
                return;

            DataGrid dataGrid = new()
            {
                Title = $"Work Days ({WorkDays.Count} days) - Overview",
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.DarkGray
                },
                Margin = "0 1 0 0"
            };

            dataGrid.Columns.Add("Date");

            Column workColumn = new("Work")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(workColumn);

            Column vacationColumn = new("Vacation")
            {
                CellHorizontalAlignment = HorizontalAlignment.Right
            };
            dataGrid.Columns.Add(vacationColumn);

            foreach (DateTime date in WorkDays)
            {
                ContentRow dataRow = new();

                dataRow.AddCell($"{date:d} ({date:dddd})");

                List<SprintMemberDay> sprintMemberDays;

                if (SprintMembers != null)
                {
                    sprintMemberDays = SprintMembers
                       .Select(x => x.Days?.FirstOrDefault(z => z.Date == date))
                       .Where(x => x != null)
                       .ToList();
                }
                else
                {
                    sprintMemberDays = new List<SprintMemberDay>();
                }

                ContentCell workHoursCell = CreateWorkHoursCell(sprintMemberDays);
                dataRow.AddCell(workHoursCell);

                ContentCell absenceCell = CreateAbsenceCell(sprintMemberDays);
                dataRow.AddCell(absenceCell);

                dataGrid.Rows.Add(dataRow);
            }

            dataGrid.Display();
        }

        private static ContentCell CreateWorkHoursCell(IEnumerable<SprintMemberDay> sprintMemberDays)
        {
            int workHours = sprintMemberDays.Sum(x => x.WorkHours);

            ContentCell workHoursCell = new();

            if (workHours == 0)
            {
                workHoursCell.Content = "- h";
            }
            else
            {
                workHoursCell.Content = $"{workHours} h";
                workHoursCell.ForegroundColor = ConsoleColor.Green;
            }

            return workHoursCell;
        }

        private static ContentCell CreateAbsenceCell(IEnumerable<SprintMemberDay> sprintMemberDays)
        {
            int absenceHours = sprintMemberDays.Sum(x => x.AbsenceHours);

            ContentCell absenceCell = new();

            if (absenceHours == 0)
            {
                absenceCell.Content = "- h";
            }
            else
            {
                absenceCell.Content = $"{absenceHours} h";
                absenceCell.ForegroundColor = ConsoleColor.Yellow;
            }

            return absenceCell;
        }
    }
}