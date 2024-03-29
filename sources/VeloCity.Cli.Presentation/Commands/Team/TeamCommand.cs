﻿// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentTeam;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Team;

[Command("team", ShortDescription = "The composition of the team (team members).", Order = 6)]
public class TeamCommand : ICommand
{
    private readonly IMediator mediator;

    [CommandParameter(Name = "date", ShortName = 'd', IsOptional = true)]
    public DateTime? Date { get; set; }

    [CommandParameter(Name = "start-date", ShortName = 'a', IsOptional = true)]
    public DateTime? StartDate { get; set; }

    [CommandParameter(Name = "end-date", ShortName = 'z', IsOptional = true)]
    public DateTime? EndDate { get; set; }

    [CommandParameter(Name = "sprint", ShortName = 's', IsOptional = true)]
    public int? Sprint { get; set; }

    public List<TeamMember> TeamMembers { get; private set; }

    public InformationViewModel Information { get; private set; }

    public TeamCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        PresentTeamRequest request = new()
        {
            Date = Date,
            DateInterval = CalculateDateInterval(),
            SprintNumber = Sprint
        };
        PresentTeamResponse response = await mediator.Send(request);

        Information = new InformationViewModel(response);
        TeamMembers = response.TeamMembers;
    }

    private DateInterval? CalculateDateInterval()
    {
        return StartDate != null || EndDate != null
            ? new DateInterval(StartDate, EndDate)
            : null;
    }
}