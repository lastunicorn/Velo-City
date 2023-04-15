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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application.PresentCommitment;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.PresentCommitment.PresentCommitmentUseCaseTests;

public class Handle_SprintCommitmentPropertiesTests
{
    private readonly PresentCommitmentUseCase useCase;
    private readonly List<Sprint> sprintsFromRepository;

    public Handle_SprintCommitmentPropertiesTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        useCase = new PresentCommitmentUseCase(unitOfWork.Object);

        sprintsFromRepository = new List<Sprint>();

        sprintRepository
            .Setup(x => x.GetLastClosed(It.IsAny<uint>()))
            .ReturnsAsync(sprintsFromRepository);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsSprintNumber()
    {
        sprintsFromRepository.Add(new Sprint
        {
            Number = 45
        });

        PresentCommitmentRequest request = new();
        PresentCommitmentResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintsCommitments[0].SprintNumber.Should().Be(45);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsCommitmentStoryPoints()
    {
        sprintsFromRepository.Add(new Sprint
        {
            CommitmentStoryPoints = 23
        });

        PresentCommitmentRequest request = new();
        PresentCommitmentResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintsCommitments[0].CommitmentStoryPoints.Should().Be((StoryPoints)23);
    }

    [Fact]
    public async Task HavingOneSprintInRepository_WhenUseCaseIsExecuted_ThenResponseContainsActualStoryPoints()
    {
        sprintsFromRepository.Add(new Sprint
        {
            ActualStoryPoints = 28
        });

        PresentCommitmentRequest request = new();
        PresentCommitmentResponse response = await useCase.Handle(request, CancellationToken.None);

        response.SprintsCommitments[0].ActualStoryPoints.Should().Be((StoryPoints)28);
    }
}