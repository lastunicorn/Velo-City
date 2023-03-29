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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.PresentTeamMemberEmployments;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentTeamMemberEmployments.PresentTeamMemberEmploymentsUseCaseTests;

public class Handle_EmploymentInfoTests
{
    private readonly PresentTeamMemberEmploymentsUseCase useCase;
    private readonly TeamMember teamMemberFromRepository;

    public Handle_EmploymentInfoTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ITeamMemberRepository> teamMemberRepository = new();
        ApplicationState applicationState = new();

        unitOfWork
            .Setup(x => x.TeamMemberRepository)
            .Returns(teamMemberRepository.Object);

        applicationState.SelectedTeamMemberId = 6;

        teamMemberFromRepository = new TeamMember
        {
            Employments = new EmploymentCollection()
        };

        teamMemberRepository
            .Setup(x => x.Get(It.IsAny<int>()))
            .ReturnsAsync(teamMemberFromRepository);

        useCase = new PresentTeamMemberEmploymentsUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneEmploymentInRepository_WhenUseCaseIsExecuted_ThenResponseContainsDateInterval()
    {
        // Arrange

        Employment employment = new()
        {
            TimeInterval = new DateInterval(new DateTime(2012, 04, 01), new DateTime(2017, 07, 14))
        };
        teamMemberFromRepository.Employments.Add(employment);

        // Act

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        // Assert

        DateInterval expectedDateInterval = new(new DateTime(2012, 04, 01), new DateTime(2017, 07, 14));
        response.Employments[0].TimeInterval.Should().Be(expectedDateInterval);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneEmploymentInRepository_WhenUseCaseIsExecuted_ThenResponseContainsHoursPerDay()
    {
        // Arrange

        Employment employment = new()
        {
            HoursPerDay = 6
        };
        teamMemberFromRepository.Employments.Add(employment);

        // Act

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        // Assert

        response.Employments[0].HoursPerDay.Should().Be((HoursValue)6);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneEmploymentInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEmploymentWeek()
    {
        // Arrange

        Employment employment = new()
        {
            EmploymentWeek = new EmploymentWeek(new[]
            {
                DayOfWeek.Monday,
                DayOfWeek.Wednesday
            })
        };
        teamMemberFromRepository.Employments.Add(employment);

        // Act

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        // Assert

        DayOfWeek[] expectedEmploymentWeek =
        {
            DayOfWeek.Monday,
            DayOfWeek.Wednesday
        };
        response.Employments[0].EmploymentWeek.Should().BeEquivalentTo(expectedEmploymentWeek);
    }

    [Fact]
    public async Task HavingTeamMemberWithOneEmploymentInRepository_WhenUseCaseIsExecuted_ThenResponseContainsCountry()
    {
        // Arrange

        Employment employment = new()
        {
            Country = "RO"
        };
        teamMemberFromRepository.Employments.Add(employment);

        // Act

        PresentTeamMemberEmploymentsRequest request = new();
        PresentTeamMemberEmploymentsResponse response = await useCase.Handle(request, CancellationToken.None);

        // Assert

        response.Employments[0].Country.Should().BeEquivalentTo("RO");
    }
}