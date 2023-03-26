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
using System.Text;
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintMembers
{
    internal class SprintMemberDataGridRow : ContentRow
    {
        public SprintMemberDataGridRow(SprintMemberDay sprintMemberDay)
        {
            ContentCell dateCell = CreateDateCell(sprintMemberDay);
            AddCell(dateCell);

            ContentCell workHoursCell = CreateWorkHoursCell(sprintMemberDay);
            AddCell(workHoursCell);

            ContentCell absenceHoursCell = CreateAbsenceHoursCell(sprintMemberDay);
            AddCell(absenceHoursCell);

            ContentCell absenceReasonCell = CreateAbsenceReasonCell(sprintMemberDay);
            AddCell(absenceReasonCell);

            ForegroundColor = CalculateRowColor(sprintMemberDay);
        }

        private static ContentCell CreateDateCell(SprintMemberDay sprintMemberDay)
        {
            return new ContentCell
            {
                Content = $"{sprintMemberDay.SprintDay.Date:d} ({sprintMemberDay.SprintDay.Date:ddd})"
            };
        }

        private static ContentCell CreateWorkHoursCell(SprintMemberDay sprintMemberDay)
        {
            return new ContentCell
            {
                Content = sprintMemberDay.AbsenceReason is AbsenceReason.None or AbsenceReason.Vacation or AbsenceReason.OfficialHoliday
                    ? sprintMemberDay.WorkHours.ToString()
                    : string.Empty
            };
        }

        private static ContentCell CreateAbsenceHoursCell(SprintMemberDay sprintMemberDay)
        {
            return new ContentCell
            {
                Content = sprintMemberDay.AbsenceReason is AbsenceReason.None or AbsenceReason.Vacation or AbsenceReason.OfficialHoliday
                    ? sprintMemberDay.AbsenceHours.ToString()
                    : string.Empty
            };
        }

        private static ContentCell CreateAbsenceReasonCell(SprintMemberDay sprintMemberDay)
        {
            return new ContentCell
            {
                Content = CreateAbsenceReasonCellContent(sprintMemberDay)
            };
        }

        private static string CreateAbsenceReasonCellContent(SprintMemberDay sprintMemberDay)
        {
            switch (sprintMemberDay.AbsenceReason)
            {
                case AbsenceReason.None:
                case AbsenceReason.WeekEnd:
                    return string.Empty;

                case AbsenceReason.OfficialHoliday:
                {
                    StringBuilder sb = new();

                    if (sprintMemberDay.AbsenceComments != null)
                        sb.Append(sprintMemberDay.AbsenceComments);

                    return new ContentCell
                    {
                        Content = sb.ToString()
                    };
                }

                case AbsenceReason.Vacation:
                case AbsenceReason.Unemployed:
                case AbsenceReason.Contract:
                {
                    StringBuilder sb = new();

                    string absenceReason = ToString(sprintMemberDay.AbsenceReason);
                    sb.Append(absenceReason);

                    if (sprintMemberDay.AbsenceComments != null)
                        sb.Append($" ({sprintMemberDay.AbsenceComments})");

                    return new ContentCell
                    {
                        Content = sb.ToString()
                    };
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string ToString(AbsenceReason absenceReason)
        {
            return absenceReason switch
            {
                AbsenceReason.None => string.Empty,
                AbsenceReason.WeekEnd => "Week-end",
                AbsenceReason.OfficialHoliday => "Official Holiday",
                AbsenceReason.Vacation => "Vacation",
                AbsenceReason.Unemployed => "Unemployed",
                AbsenceReason.Contract => "Contract",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static ConsoleColor? CalculateRowColor(SprintMemberDay sprintMemberDay)
        {
            return sprintMemberDay.AbsenceReason switch
            {
                AbsenceReason.None => ConsoleColor.Green,
                AbsenceReason.Vacation => ConsoleColor.Yellow,
                AbsenceReason.WeekEnd => ConsoleColor.DarkGray,
                _ => null
            };
        }
    }
}