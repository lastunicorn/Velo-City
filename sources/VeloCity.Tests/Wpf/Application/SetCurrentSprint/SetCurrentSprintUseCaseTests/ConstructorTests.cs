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
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.SetCurrentSprint;
using FluentAssertions;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.SetCurrentSprint.SetCurrentSprintUseCaseTests
{
    public class ConstructorTests
    {
        [Fact]
        public void HavingNullApplicationState_WhenInstantiationgUseCase_ThenThrows()
        {
            EventBus eventBus = new();

            Action action = () =>
            {
                _ = new SetCurrentSprintUseCase(null, eventBus);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingNullEventBus()
        {
            ApplicationState applicationState = new ApplicationState();

            Action action = () =>
            {
                _ = new SetCurrentSprintUseCase(applicationState, null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void HavingAllDependencies_WhenInstantiationgUseCase_ThenDoesNotThrow()
        {
            ApplicationState applicationState = new ApplicationState();
            EventBus eventBus = new();

            Action action = () =>
            {
                _ = new SetCurrentSprintUseCase(applicationState, eventBus);
            };

            action.Should().NotThrow();
        }
    }
}
