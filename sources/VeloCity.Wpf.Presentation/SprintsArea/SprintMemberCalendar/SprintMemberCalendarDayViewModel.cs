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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMemberCalendar;
using DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar
{
    public class SprintMemberCalendarDayViewModel : DataGridRowViewModel
    {
        private readonly IRequestBus requestBus;
        private int teamMemberId = -1;
        private ChartBarValue<SprintMemberCalendarDayViewModel> chartBarValue;
        private bool canAddVacation;
        private bool canRemoveVacation;
        private HoursValue? absenceHours;
        private bool hasAbsenceHours;

        public override bool IsSelectable => true;

        public DateTime Date { get; private set; }

        public bool IsCurrentDay { get; private set; }

        public bool IsWorkDay { get; private set; }

        public HoursValue? WorkHours { get; private set; }

        public bool HasWorkHours => WorkHours?.Value > 0;

        public ChartBarValue<SprintMemberCalendarDayViewModel> ChartBarValue
        {
            get => IsWorkDay ? chartBarValue : null;
            set => chartBarValue = value;
        }

        public HoursValue? AbsenceHours
        {
            get => absenceHours;
            set
            {
                if (IsInitializeMode)
                {
                    absenceHours = value;
                    OnPropertyChanged();

                    HasAbsenceHours = AbsenceHours?.Value > 0;
                }
                else
                {
                    _ = UpdateVacationHours(value);
                }
            }
        }

        private async Task UpdateVacationHours(HoursValue? value)
        {
            UpdateVacationHoursRequest request = new()
            {
                TeamMemberId = teamMemberId,
                Date = Date,
                Hours = value
            };

            await requestBus.Send(request);
        }

        public bool HasAbsenceHours
        {
            get => hasAbsenceHours;
            set
            {
                hasAbsenceHours = value;
                OnPropertyChanged();
            }
        }

        public string AbsenceDetails { get; private set; }

        public bool CanAddVacation
        {
            get => canAddVacation;
            set
            {
                canAddVacation = value;
                OnPropertyChanged();
            }
        }

        public bool CanRemoveVacation
        {
            get => canRemoveVacation;
            set
            {
                canRemoveVacation = value;
                OnPropertyChanged();
            }
        }

        public SprintMemberCalendarDayViewModel(IRequestBus requestBus, SprintMemberDayDto sprintMemberDay)
        {
            if (sprintMemberDay == null) throw new ArgumentNullException(nameof(sprintMemberDay));
            this.requestBus = requestBus ?? throw new ArgumentNullException(nameof(requestBus));

            RunInInitializeMode(() =>
            {
                teamMemberId = sprintMemberDay.TeamMemberId;

                Date = sprintMemberDay.Date;
                IsCurrentDay = sprintMemberDay.IsCurrentDay;
                IsWorkDay = sprintMemberDay.IsWorkDay;
                WorkHours = sprintMemberDay.WorkHours;
                AbsenceHours = sprintMemberDay.AbsenceHours;
                AbsenceDetails = CreateAbsenceDetails(sprintMemberDay);

                CanAddVacation = sprintMemberDay.CanAddVacation;
                CanRemoveVacation = sprintMemberDay.CanRemoveVacation;
            });
        }

        private static string CreateAbsenceDetails(SprintMemberDayDto sprintMemberDay)
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