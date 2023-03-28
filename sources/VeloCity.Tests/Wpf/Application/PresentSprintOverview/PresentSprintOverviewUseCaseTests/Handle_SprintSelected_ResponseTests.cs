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

using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Wpf.Application.PresentSprintOverview;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentSprintOverview.PresentSprintOverviewUseCaseTests;

public class Handle_SprintSelected_ResponseTests
{
    private readonly PresentSprintOverviewUseCase useCase;
    private readonly Sprint sprintFromRepository;

    public Handle_SprintSelected_ResponseTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        ApplicationState applicationState = new()
        {
            SelectedSprintId = 375
        };

        sprintFromRepository = new Sprint();

        sprintRepository
            .Setup(x => x.Get(375))
            .Returns(sprintFromRepository);

        Mock<IRequestBus> requestBus = new();

        useCase = new PresentSprintOverviewUseCase(unitOfWork.Object, applicationState, requestBus.Object);

        AnalyzeSprintResponse analyzeSprintResponse = new();

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
}