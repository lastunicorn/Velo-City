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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintOverview.PresentSprintOverviewUseCaseTests;

public class Handle_NoSprintSelected_ResponseTests
{
    private readonly PresentSprintOverviewUseCase useCase;
    private readonly Sprint sprintFromRepository;
    private readonly AnalyzeSprintResponse analyzeSprintResponse;

    public Handle_NoSprintSelected_ResponseTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new()
        {
            SelectedSprintId = null
        };

        sprintFromRepository = new Sprint();

        sprintRepository
            .Setup(x => x.GetLastInProgress())
            .ReturnsAsync(sprintFromRepository);

        Mock<IRequestBus> requestBus = new();

        useCase = new PresentSprintOverviewUseCase(unitOfWork.Object, applicationState, requestBus.Object);

        analyzeSprintResponse = new AnalyzeSprintResponse();

        requestBus
            .Setup(x => x.Send<AnalyzeSprintRequest, AnalyzeSprintResponse>(It.IsAny<AnalyzeSprintRequest>(), CancellationToken.None))
            .ReturnsAsync(analyzeSprintResponse);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintState()
    {
        sprintFromRepository.State = SprintState.Closed;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintState.Should().Be(SprintState.Closed);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintDateInterval()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 01, 13), new DateTime(2023, 04, 8));

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        DateInterval expectedDateInterval = new(new DateTime(2023, 01, 13), new DateTime(2023, 04, 8));
        response.SprintDateInterval.Should().Be(expectedDateInterval);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintGoal()
    {
        sprintFromRepository.Goal = "goal 3";

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintGoal.Should().Be("goal 3");
    }

    [Fact]
    public async Task HavingOneSprintInRepositoryForOneWeek_WhenUseCaseIsExecuted_ThenResponseContainsSprintWorkDays()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 19));

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.WorkDaysCount.Should().Be(5);
    }

    [Fact]
    public async Task HavingOneSprintInRepositoryForOneWeekWithOneHalfTimeMember_WhenUseCaseIsExecuted_ThenResponseContainsSprintTotalWorkHours()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 19));
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 01, 01),
                    HoursPerDay = 4,
                    EmploymentWeek = new EmploymentWeek()
                }
            }
        };
        sprintFromRepository.AddSprintMember(teamMember);

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.TotalWorkHours.Should().Be((HoursValue)20);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEstimatedStoryPoints()
    {
        analyzeSprintResponse.EstimatedStoryPoints = 4;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.EstimatedStoryPoints.Should().Be((StoryPoints)4);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEstimatedStoryPointsWithVelocityPenalties()
    {
        analyzeSprintResponse.EstimatedStoryPointsWithVelocityPenalties = 43;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.EstimatedStoryPointsWithVelocityPenalties.Should().Be((StoryPoints)43);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsEstimatedVelocity()
    {
        analyzeSprintResponse.EstimatedVelocity = 84;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.EstimatedVelocity.Should().Be((Velocity)84);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsVelocityPenalties()
    {
        analyzeSprintResponse.VelocityPenalties = new List<VelocityPenaltyInstance>
        {
            new()
            {
                TeamMember = new TeamMember
                {
                    Name = new PersonName
                    {
                        FirstName = "Joe",
                        LastName = "Smith"
                    }
                },
                Value = 12
            },
            new()
            {
                TeamMember = new TeamMember
                {
                    Name = new PersonName
                    {
                        FirstName = "Mykelti",
                        LastName = "Williamson"
                    }
                },
                Value = 80
            }
        };

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.VelocityPenalties[0].PersonName.Should().Be(new PersonName
        {
            FirstName = "Joe",
            LastName = "Smith"
        });
        response.VelocityPenalties[0].PenaltyValue.Should().Be(12);
        response.VelocityPenalties[1].PersonName.Should().Be(new PersonName
        {
            FirstName = "Mykelti",
            LastName = "Williamson"
        });
        response.VelocityPenalties[1].PenaltyValue.Should().Be(80);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsCommitmentStoryPoints()
    {
        sprintFromRepository.CommitmentStoryPoints = 91;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CommitmentStoryPoints.Should().Be((StoryPoints)91);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsActualStoryPoints()
    {
        sprintFromRepository.ActualStoryPoints = 921;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.ActualStoryPoints.Should().Be((StoryPoints)921);
    }

    [Fact]
    public async Task HavingOneSprintInRepositoryForOneWeekWithOneHalfTimeMember_WhenUseCaseIsExecuted_ThenResponseContainsActualVelocity()
    {
        sprintFromRepository.DateInterval = new DateInterval(new DateTime(2023, 03, 13), new DateTime(2023, 03, 19));
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new()
                {
                    StartDate = new DateTime(2000, 01, 01),
                    HoursPerDay = 4,
                    EmploymentWeek = new EmploymentWeek()
                }
            }
        };
        sprintFromRepository.AddSprintMember(teamMember);
        sprintFromRepository.ActualStoryPoints = 40;

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.ActualVelocity.Should().Be((Velocity)2);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsTheListOfPreviouslyClosedSprintNumbers()
    {
        analyzeSprintResponse.HistorySprints = new SprintList
        {
            new() { Number = 234 },
            new() { Number = 1 },
            new() { Number = 376 }
        };

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.PreviouslyClosedSprintNumbers.Should().Equal(234, 1, 376);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsNullExcludedSprints()
    {
        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.ExcludedSprints.Should().BeNull();
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintComments()
    {
        sprintFromRepository.Comments = "some comments about the sprint";

        PresentSprintOverviewRequest request = new();
        PresentSprintOverviewResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintComments.Should().Be("some comments about the sprint");
    }
}