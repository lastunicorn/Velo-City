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

using DustInTheWind.VeloCity.Ports.SettingsAccess;
using DustInTheWind.VeloCity.Wpf.Application.PresentMain;

namespace DustInTheWind.VeloCity.Tests.Wpf.Application.PresentMain.PresentMainUseCaseTests;

public class HandleTests
{
    [Fact]
    public async Task HavingADatabaseLocationSpecifiedInConfig_WhenUseCaseIsExecuted_ThenReturnsThatLocationInResponse()
    {
        Mock<IConfig> config = new();
        config
            .SetupGet(x => x.DatabaseLocation)
            .Returns("some-location");

        PresentMainUseCase useCase = new(config.Object);
        PresentMainRequest request = new();

        PresentMainResponse response = await useCase.Handle(request, CancellationToken.None);

        response.DatabaseConnectionString.Should().Be("some-location");
    }
}