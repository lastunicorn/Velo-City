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

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    internal class SprintMemberDetailsControl : Control
    {
        public SprintMember SprintMember { get; set; }

        protected override void DoDisplay()
        {
            int totalWorkHours = SprintMember.Days.Sum(x => x.WorkHours);
            string titleText = $"{SprintMember.Name} - {totalWorkHours}h";

            DataGrid dataGrid = new()
            {
                Title = titleText,
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

            dataGrid.Columns.Add("Details");

            IEnumerable<ContentRow> contentRowSelect = SprintMember.Days
                .Where(x => x.AbsenceReason != AbsenceReason.WeekEnd &&
                            x.AbsenceReason != AbsenceReason.OfficialHoliday)
                .Select(ToDataRow);

            foreach (ContentRow contentRow in contentRowSelect)
                dataGrid.Rows.Add(contentRow);

            dataGrid.Display();
        }

        private static ContentRow ToDataRow(SprintMemberDay sprintMemberDay)
        {
            ContentRow dataRow = new();

            dataRow.AddCell($"{sprintMemberDay.Date:d} ({sprintMemberDay.Date:dddd})");

            string workHours = sprintMemberDay.WorkHours == 0
                ? "-"
                : sprintMemberDay.WorkHours.ToString();
            dataRow.AddCell(workHours + " h");

            string absenceHours = sprintMemberDay.AbsenceHours == 0
                ? "-"
                : sprintMemberDay.AbsenceHours.ToString();
            dataRow.AddCell(absenceHours + " h");

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