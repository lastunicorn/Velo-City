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
using DustInTheWind.ConsoleTools;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class AnalyzeSprintView
    {
        public void Display(AnalyzeSprintResponse response)
        {
            DisplaySprintInformation(response);

            foreach (SprintMember sprintMember in response.SprintMembers)
                DisplaySprintMemberDetails(sprintMember);
        }

        private static void DisplaySprintInformation(AnalyzeSprintResponse response)
        {
            DataGrid dataGrid = new()
            {
                Title = $"{response.SprintName} ({response.StartDate:d} - {response.EndDate:d})",
                Border =
                {
                    DisplayBorderBetweenRows = true
                },
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.DarkGray
                }
            };

            StringBuilder sb = CalculateWorkDays(response.WorkDays);
            dataGrid.Rows.Add("Work Days", sb.ToString());

            dataGrid.Rows.Add("Total Work Hours", $"{response.TotalWorkHours} h");
            dataGrid.Rows.Add("Estimated Story Points", $"{response.EstimatedStoryPoints} SP");
            dataGrid.Rows.Add("Estimated Velocity", $"{response.EstimatedVelocity} SP/h");
            dataGrid.Rows.Add("Commitment Story Points", $"{response.CommitmentStoryPoints} SP");
            dataGrid.Rows.Add("Actual Story Points", $"{response.ActualStoryPoints} SP");
            dataGrid.Rows.Add("Actual Velocity", $"{response.ActualVelocity} SP/h");

            dataGrid.Display();

            CustomConsole.WriteLine();
            CustomConsole.WriteLine(ConsoleColor.DarkYellow, "Notes:");

            string previousSprints = string.Join(",", response.PreviousSprints);
            CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"  - The estimations were calculated based on previous {response.LookBackSprintCount} closed sprints: {previousSprints}");

            if (response.ExcludesSprints is { Count: > 0 })
            {
                string excludedSprints = string.Join(",", response.ExcludesSprints);
                CustomConsole.WriteLine(ConsoleColor.DarkYellow, $"  - Excluded sprints: {excludedSprints} (These sprints were excluded from the velocity calculation algorithm.)");
            }
        }

        private static StringBuilder CalculateWorkDays(IReadOnlyList<DateTime> workDays)
        {
            StringBuilder sb = new();

            for (int i = 0; i < workDays.Count; i++)
            {
                DateTime dateTime = workDays[i];
                int dayIndex = i + 1;
                string line = $"day {dayIndex:D2}: {dateTime:d} ({dateTime:dddd})";

                bool isLastRow = i == workDays.Count - 1;
                if (isLastRow)
                    sb.Append(line);
                else
                    sb.AppendLine(line);
            }

            return sb;
        }

        private static void DisplaySprintMemberDetails(SprintMember sprintMember)
        {
            Console.WriteLine();

            int totalWorkHours = sprintMember.Days.Sum(x => x.WorkHours);
            string titleText = $"{sprintMember.Name} - {totalWorkHours}h";

            DataGrid dataGrid = new()
            {
                Title = titleText,
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.DarkGray
                }
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

            IEnumerable<ContentRow> contentRowSelect = sprintMember.Days
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

            string vacationHours = sprintMemberDay.AbsenceHours == 0
                ? "-"
                : sprintMemberDay.AbsenceHours.ToString();
            dataRow.AddCell(vacationHours + " h");

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