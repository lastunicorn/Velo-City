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

using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.StartSprint;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.StartSprint.StartSprintUseCaseTests;

public class ConstructorTests
{
    [Fact]
    public void HavingNullUnitOfWork_WhenInstantiatingUseCase_ThenThrows()
    {
        ApplicationState applicationState = new();
        EventBus eventBus = new();
        Mock<IUserTerminal> userInterface = new();
        Mock<IRequestBus> requestBus = new();

        Action action = () =>
        {
            _ = new StartSprintUseCase(null, applicationState, eventBus, userInterface.Object, requestBus.Object);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingNullApplicationState_WhenInstantiatingUseCase_ThenThrows()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        EventBus eventBus = new();
        Mock<IUserTerminal> userInterface = new();
        Mock<IRequestBus> requestBus = new();

        Action action = () =>
        {
            _ = new StartSprintUseCase(unitOfWork.Object, null, eventBus, userInterface.Object, requestBus.Object);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingNullEventBus_WhenInstantiatingUseCase_ThenThrows()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        ApplicationState applicationState = new();
        Mock<IUserTerminal> userInterface = new();
        Mock<IRequestBus> requestBus = new();

        Action action = () =>
        {
            _ = new StartSprintUseCase(unitOfWork.Object, applicationState, null, userInterface.Object, requestBus.Object);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingNullUserInterface_WhenInstantiatingUseCase_ThenThrows()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        ApplicationState applicationState = new();
        EventBus eventBus = new();
        Mock<IRequestBus> requestBus = new();

        Action action = () =>
        {
            _ = new StartSprintUseCase(unitOfWork.Object, applicationState, eventBus, null, requestBus.Object);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingNullRequestBus_WhenInstantiatingUseCase_ThenThrows()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        ApplicationState applicationState = new();
        EventBus eventBus = new();
        Mock<IUserTerminal> userInterface = new();

        Action action = () =>
        {
            _ = new StartSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object, null);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingAllDependencies_WhenInstantiatingUseCase_ThenDoesNotThrow()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        ApplicationState applicationState = new();
        EventBus eventBus = new();
        Mock<IUserTerminal> userInterface = new();
        Mock<IRequestBus> requestBus = new();

        Action action = () =>
        {
            _ = new StartSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object, requestBus.Object);
        };

        action.Should().NotThrow();
    }
}