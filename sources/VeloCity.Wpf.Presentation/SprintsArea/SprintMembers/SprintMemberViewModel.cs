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

using DustInTheWind.VeloCity.ChartTools;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintMembers;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMembers;

public class SprintMemberViewModel : DataGridRowViewModel
{
    public override bool IsSelectable => true;

    public PersonName Name { get; }

    public HoursValue WorkHours { get; }

    public bool HasWorkHours => WorkHours.Value > 0;

    public HoursValue AbsenceHours { get; }

    public bool HasAbsenceHours => AbsenceHours.Value > 0;

    public ChartBarValue<SprintMemberViewModel> ChartBarValue { get; set; }

    public ShowSprintMemberCalendarCommand ShowSprintMemberCalendarCommand { get; }

    public SprintMemberViewModel(IRequestBus requestBus, EventBus eventBus, SprintMemberDto sprintMember)
    {
        if (sprintMember == null) throw new ArgumentNullException(nameof(sprintMember));

        Name = sprintMember.Name;
        WorkHours = sprintMember.WorkHours;
        AbsenceHours = sprintMember.AbsenceHours;

        ShowSprintMemberCalendarCommand = new ShowSprintMemberCalendarCommand(requestBus, eventBus)
        {
            TeamMemberId = sprintMember.TeamMemberId,
            SprintId = sprintMember.SprintId
        };
    }
}