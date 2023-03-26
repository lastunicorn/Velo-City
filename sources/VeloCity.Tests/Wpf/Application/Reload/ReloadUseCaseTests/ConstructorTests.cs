﻿// VeloCity
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
using DustInTheWind.VeloCity.Wpf.Application.Reload;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.Reload.ReloadUseCaseTests;

public class ConstructorTests
{
    [Fact]
    public void HavingNullEventBus()
    {
        Mock<IDataStorage> dataStorage = new();

        Action action = () =>
        {
            _ = new ReloadUseCase(null, dataStorage.Object);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingNullDataStorage_WhenInstantiationgUseCase_ThenThrows()
    {
        EventBus eventBus = new();

        Action action = () =>
        {
            _ = new ReloadUseCase(eventBus, null);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingAllDependencies_WhenInstantiationgUseCase_ThenDoesNotThrow()
    {
        EventBus eventBus = new();
        Mock<IDataStorage> dataStorage = new();

        Action action = () =>
        {
            _ = new ReloadUseCase(eventBus, dataStorage.Object);
        };

        action.Should().NotThrow();
    }
}