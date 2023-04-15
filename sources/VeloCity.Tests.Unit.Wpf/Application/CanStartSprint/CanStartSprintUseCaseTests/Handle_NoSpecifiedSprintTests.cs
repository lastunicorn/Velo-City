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
using DustInTheWind.VeloCity.Wpf.Application;
using DustInTheWind.VeloCity.Wpf.Application.CanStartSprint;

namespace DustInTheWind.VeloCity.Tests.Unit.Wpf.Application.CanStartSprint.CanStartSprintUseCaseTests;

public class Handle_NoSpecifiedSprintTests
{
    private readonly ApplicationState applicationState;
    private readonly CanStartSprintUseCase useCase;

    public Handle_NoSpecifiedSprintTests()
    {
        Mock<IUnitOfWork> unitOfWork = new();
        Mock<ISprintRepository> sprintRepository = new();

        unitOfWork
            .Setup(x => x.SprintRepository)
            .Returns(sprintRepository.Object);

        applicationState = new ApplicationState();

        useCase = new CanStartSprintUseCase(unitOfWork.Object, applicationState);
    }

    [Fact]
    public async Task HavingNoSprintWithIdSpecifiedInApplicationState_WhenUseCaseIsExecuted_ThenCanStartSprintIsFalse()
    {
        applicationState.SelectedSprintId = null;

        CanStartSprintRequest request = new();
        CanStartSprintResponse response = await useCase.Handle(request, CancellationToken.None);

        response.CanStartSprint.Should().BeFalse();
    }
}