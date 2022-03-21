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
using System.Text;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint.SprintMembers
{
    internal class SprintMemberDetailsControl : Control
    {
        private readonly DataGridFactory dataGridFactory;

        public SprintMember SprintMember { get; set; }

        public SprintMemberDetailsControl(DataGridFactory dataGridFactory)
        {
            this.dataGridFactory = dataGridFactory;
        }

        protected override void DoDisplay()
        {
            HoursValue totalWorkHours = SprintMember.Days.Sum(x => x.WorkHours);
            totalWorkHours.ZeroCharacter = '0';

            DataGrid dataGrid = dataGridFactory.Create();
            dataGrid.Title = $"{SprintMember.Name} - {totalWorkHours}";

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

            dataGrid.Columns.Add("Details");

            IEnumerable<ContentRow> contentRowSelect = SprintMember.Days
                .Where(x =>
                {
                    bool isWeekDay = x.Date.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday);
                    if (isWeekDay)
                        return true;

                    bool hasWorkHoursInWeekEnd = x.WorkHours > 0;
                    if (hasWorkHoursInWeekEnd)
                        return true;

                    bool isOfficialHoliday = x.AbsenceReason == AbsenceReason.OfficialHoliday;
                    return isOfficialHoliday;
                })
                .Where(x => x.AbsenceReason != AbsenceReason.OfficialHoliday)
                .Select(ToDataRow);

            foreach (ContentRow contentRow in contentRowSelect)
                dataGrid.Rows.Add(contentRow);

            dataGrid.Display();
        }

        private static ContentRow ToDataRow(SprintMemberDay sprintMemberDay)
        {
            ContentRow dataRow = new();

            dataRow.AddCell($"{sprintMemberDay.Date:d} ({sprintMemberDay.Date:ddd})");

            HoursValue workHours = sprintMemberDay.WorkHours;
            dataRow.AddCell(workHours);

            HoursValue absenceHours = sprintMemberDay.AbsenceHours;
            dataRow.AddCell(absenceHours);

            if (sprintMemberDay.AbsenceReason != AbsenceReason.None)
            {
                StringBuilder sb = new();

                sb.Append(sprintMemberDay.AbsenceReason);

                if (sprintMemberDay.AbsenceComments != null)
                    sb.Append($" ({sprintMemberDay.AbsenceComments})");

                dataRow.AddCell(sb);
            }
            else
            {
                dataRow.AddCell(string.Empty);
            }

            switch (sprintMemberDay.AbsenceReason)
            {
                case AbsenceReason.None:
                    dataRow.ForegroundColor = ConsoleColor.Green;
                    break;

                case AbsenceReason.Vacation:
                    dataRow.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }

            return dataRow;
        }
    }
}