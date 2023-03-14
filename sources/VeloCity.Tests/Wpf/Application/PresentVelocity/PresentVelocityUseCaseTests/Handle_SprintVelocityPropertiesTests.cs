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
using DustInTheWind.VeloCity.Wpf.Application.PresentVelocity;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentVelocity.PresentVelocityUseCaseTests;

public class Handle_SprintVelocityPropertiesTests
{
    private readonly PresentVelocityUseCase useCase;
    private readonly Mock<ISprintRepository> sprintRepository;
    private readonly List<Sprint> sprintsFromRepository;

    public Handle_SprintVelocityPropertiesTests()
    {
        Mock<IUnitOfWork> unitOfWork = new Mock<IUnitOfWork>();
        sprintRepository = new Mock<ISprintRepository>();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        useCase = new PresentVelocityUseCase(unitOfWork.Object);

        sprintsFromRepository = new List<Sprint>();

        sprintRepository
            .Setup(x => x.GetLastClosed(It.IsAny<uint>()))
            .Returns(sprintsFromRepository);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintNumber()
    {
        sprintsFromRepository.Add(new Sprint
        {
            Number = 33
        });

        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintVelocities[0].SprintNumber.Should().Be(33);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsCommitmentStoryPoints()
    {
        TeamMember teamMember = new()
        {
            Employments = new EmploymentCollection
            {
                new Employment
                {
                    EmploymentWeek = new EmploymentWeek(),
                    HoursPerDay = 8,
                    StartDate = new DateTime(2000, 01, 01)
                }
            }
        };
        Sprint sprint = new()
        {
            ActualStoryPoints = 40,
            DateInterval = new DateInterval(new DateTime(2023, 03, 06), new DateTime(2023, 03, 10))
        };
        sprint.AddSprintMember(teamMember);
        sprintsFromRepository.Add(sprint);

        PresentVelocityRequest request = new();
        PresentVelocityResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintVelocities[0].Velocity.Should().Be((Velocity)1);
    }
}