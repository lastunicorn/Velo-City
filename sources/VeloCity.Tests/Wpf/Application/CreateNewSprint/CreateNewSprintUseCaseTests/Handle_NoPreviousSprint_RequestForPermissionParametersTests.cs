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
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Ports.UserAccess.SprintNewConfirmation;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CreateNewSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CreateNewSprint.CreateNewSprintUseCaseTests;

public class Handle_NoPreviousSprint_RequestForPermissionParametersTests
{
    private readonly CreateNewSprintUseCase useCase;
    private SprintNewConfirmationRequest confirmationRequest;

    public Handle_NoPreviousSprint_RequestForPermissionParametersTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        sprintRepository
            .Setup(x => x.GetLast())
            .Returns(null as Sprint);

        Mock<IUserInterface> userInterface = new();
        EventBus eventBus = new();
        ApplicationState applicationState = new();

        confirmationRequest = null;

        userInterface
            .Setup(x => x.ConfirmNewSprint(It.IsAny<SprintNewConfirmationRequest>()))
            .Returns(new SprintNewConfirmationResponse())
            .Callback<SprintNewConfirmationRequest>(request => confirmationRequest = request);

        useCase = new CreateNewSprintUseCase(unitOfWork.Object, userInterface.Object, eventBus, applicationState);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenTheRequestForPermissionContainsNullSprintTitle()
    {
        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        confirmationRequest.SprintTitle.Should().BeNull();
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenTheRequestForPermissionContainsSprintNumber1()
    {
        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        confirmationRequest.SprintNumber.Should().Be(1);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenTheRequestForPermissionContainsSprintStartDateToday()
    {
        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        confirmationRequest.SprintStartDate.Should().Be(DateTime.Today);
    }

    [Fact]
    public async Task HavingNoSprintInRepository_WhenUseCaseIsExecuted_ThenTheRequestForPermissionContainsSprintLength14()
    {
        CreateNewSprintRequest request = new();
        await useCase.Handle(request, CancellationToken.None);

        confirmationRequest.SprintLength.Should().Be(14);
    }
}