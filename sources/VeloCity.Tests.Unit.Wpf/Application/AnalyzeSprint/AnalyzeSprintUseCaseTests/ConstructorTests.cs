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

using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.AnalyzeSprint.AnalyzeSprintUseCaseTests;

public class ConstructorTests
{
    [Fact]
    public void HavingNullUnitOfWork_WhenInstantiatingUseCase_ThenThrows()
    {
        Mock<IConfig> config = new();

        Action action = () =>
        {
            _ = new AnalyzeSprintUseCase(null, config.Object);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingNullConfig_WhenInstantiatingUseCase_ThenThrows()
    {
        Mock<IUnitOfWork> unitOfWork = new();

        Action action = () =>
        {
            _ = new AnalyzeSprintUseCase(unitOfWork.Object, null);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void HavingAllDependencies_WhenInstantiatingUseCase_ThenDoesNotThrow()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<IConfig> config = new();

        Action action = () =>
        {
            _ = new AnalyzeSprintUseCase(unitOfWork.Object, config.Object);
        };

        action.Should().NotThrow();
    }
}