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
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.UserAccess;
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CanCloseSprint;
using DustInTheWind.VeloCity.Wpf.Application.CloseSprint;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.CanCloseSprint.CanClssoseSprintUseCaseTests
{
    public class ConstructorTests
    {
        [Fact]
        public void HavingNullUnitOfWork_WhenInstantiationgUseCase_ThenThrows()
        {
            ApplicationState applicationState = new();
            EventBus eventBus = new();
            Mock<IUserInterface> userInterface = new();

            Action action = () =>
            {
                _ = new CloseSprintUseCase(null, applicationState, eventBus, userInterface.Object);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingNullApplicationState_WhenInstantiationgUseCase_ThenThrows()
        {
            Mock<IUnitOfWork> unitOfWork = new();
            EventBus eventBus = new();
            Mock<IUserInterface> userInterface = new();

            Action action = () =>
            {
                _ = new CloseSprintUseCase(unitOfWork.Object, null, eventBus, userInterface.Object);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingNullEventBus_WhenInstantiationgUseCase_ThenThrows()
        {
            Mock<IUnitOfWork> unitOfWork = new();
            ApplicationState applicationState = new();
            Mock<IUserInterface> userInterface = new();

            Action action = () =>
            {
                _ = new CloseSprintUseCase(unitOfWork.Object, applicationState, null, userInterface.Object);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingNullUserInterface_WhenInstantiationgUseCase_ThenThrows()
        {
            Mock<IUnitOfWork> unitOfWork = new();
            ApplicationState applicationState = new();
            EventBus eventBus = new();

            Action action = () =>
            {
                _ = new CloseSprintUseCase(unitOfWork.Object, applicationState, eventBus, null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingAllDependencies_WhenInstantiationgUseCase_ThenDoesNotThrow()
        {
            Mock<IUnitOfWork> unitOfWork = new();
            ApplicationState applicationState = new();
            EventBus eventBus = new();
            Mock<IUserInterface> userInterface = new();

            Action action = () =>
            {
                _ = new CloseSprintUseCase(unitOfWork.Object, applicationState, eventBus, userInterface.Object);
            };

            action.Should().NotThrow();
        }
    }
}
