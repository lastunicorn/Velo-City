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
using System.Collections.Generic;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.Cli.Application.PresentForecast;

public class SprintFactory
{
    private readonly IUnitOfWork unitOfWork;

    public SprintFactory(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Sprint GenerateImaginarySprint(DateTime startDate, DateTime endDate)
    {
        Sprint sprint = new()
        {
            DateInterval = new DateInterval(startDate, endDate)
        };

        PopulateOfficialHolidays(sprint);
        PopulateSprintMembers(sprint);

        return sprint;
    }

    private void PopulateOfficialHolidays(Sprint sprint)
    {
        IEnumerable<OfficialHoliday> officialHolidays = unitOfWork.OfficialHolidayRepository.Get(sprint.DateInterval);
        sprint.OfficialHolidays.AddRange(officialHolidays);
    }

    private void PopulateSprintMembers(Sprint sprint)
    {
        IEnumerable<TeamMember> teamMembers = unitOfWork.TeamMemberRepository.GetByDateInterval(sprint.DateInterval);
        sprint.AddSprintMembers(teamMembers);
    }
}