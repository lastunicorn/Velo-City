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
using System.Text;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.SprintMembers
{
    public class SprintMemberDetailsDayViewModel : DataGridRowViewModel
    {
        public override bool IsSelectable => true;

        public DateTime Date { get; }

        public bool IsWorkDay { get; }

        public HoursValue? WorkHours { get; }

        public bool HasWorkHours => WorkHours?.Value > 0;

        public HoursValue? AbsenceHours { get; }

        public bool HasAbsenceHours => AbsenceHours?.Value > 0;


        public string AbsenceDetails { get; }

        public SprintMemberDetailsDayViewModel(SprintMemberDay sprintMemberDay)
        {
            if (sprintMemberDay == null) throw new ArgumentNullException(nameof(sprintMemberDay));
            
            Date = sprintMemberDay.SprintDay.Date;

            IsWorkDay = sprintMemberDay.IsWorkDay;

            if (IsWorkDay)
            {
                WorkHours = sprintMemberDay.WorkHours;
                AbsenceHours = sprintMemberDay.AbsenceHours;
            }

            AbsenceDetails = CreateAbsenceDetails(sprintMemberDay);
        }

        private static string CreateAbsenceDetails(SprintMemberDay sprintMemberDay)
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

                        return sb.ToString();
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

                        return sb.ToString();
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
    }
}