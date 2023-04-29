// VeloCity
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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.NewTeamMemberConfirmation;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.CreateNewTeamMember;

internal class CreateNewTeamMemberUseCase : IRequestHandler<CreateNewTeamMemberRequest>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ISystemClock systemClock;
    private readonly EventBus eventBus;
    private readonly ApplicationState applicationState;
    private readonly IUserTerminal userTerminal;

    public CreateNewTeamMemberUseCase(IUnitOfWork unitOfWork, ISystemClock systemClock, EventBus eventBus,
        ApplicationState applicationState, IUserTerminal userTerminal)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.userTerminal = userTerminal ?? throw new ArgumentNullException(nameof(userTerminal));
    }

    public async Task<Unit> Handle(CreateNewTeamMemberRequest request, CancellationToken cancellationToken)
    {
        NewTeamMemberConfirmationResponse confirmationResponse = RequestUserConfirmationToCreateNewTeamMember();

        if (confirmationResponse?.IsAccepted == true)
        {
            TeamMember teamMember = await CreateNewTeamMember(confirmationResponse);
            await unitOfWork.SaveChanges();

            applicationState.SelectedTeamMemberId = teamMember.Id;

            await PublishTeamMemberListChangedEvent(cancellationToken, teamMember);
        }

        return Unit.Value;
    }

    private NewTeamMemberConfirmationResponse RequestUserConfirmationToCreateNewTeamMember()
    {
        NewTeamMemberConfirmationRequest request = new()
        {
            EmploymentHours = 8,
            EmploymentWeek = EmploymentWeek.NewDefault,
            EmploymentCountry = "RO",
            StartDate = systemClock.Today
        };
        return userTerminal.ConfirmNewTeamMember(request);
    }

    private async Task<TeamMember> CreateNewTeamMember(NewTeamMemberConfirmationResponse confirmationResponse)
    {
        TeamMember teamMember = new()
        {
            Name = confirmationResponse.TeamMemberName,
            Employments = new EmploymentCollection
            {
                new()
                {
                    HoursPerDay = confirmationResponse.EmploymentHours,
                    EmploymentWeek = confirmationResponse.EmploymentWeek,
                    Country = confirmationResponse.EmploymentCountry,
                    StartDate = confirmationResponse.StartDate
                }
            }
        };

        await unitOfWork.TeamMemberRepository.Add(teamMember);
        return teamMember;
    }

    private async Task PublishTeamMemberListChangedEvent(CancellationToken cancellationToken, TeamMember teamMember)
    {
        TeamMemberListChangedEvent teamMemberListChangedEvent = new()
        {
            NewTeamMemberId = teamMember.Id
        };
        await eventBus.Publish(teamMemberListChangedEvent, cancellationToken);
    }
}