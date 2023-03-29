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

using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;
using DustInTheWind.VeloCity.Wpf.Presentation.Styles.Converters;

namespace DustInTheWind.VeloCity.Tests.Wpf.Presentation.Styles.Converters.SprintStateToTextConverterTests;

public class Convert_FromSprintStateTests
{
    private readonly SprintStateToTextConverter converter;

    public Convert_FromSprintStateTests()
    {
        converter = new SprintStateToTextConverter();
    }

    [Theory]
    [InlineData(SprintState.Unknown, "Unknown")]
    [InlineData(SprintState.New, "New")]
    [InlineData(SprintState.InProgress, "In Progress")]
    [InlineData(SprintState.Closed, "Closed")]
    public void HavingSprintStateValue_WhenConverting_ThenReturnsCorrectString(SprintState value, string expectedText)
    {
        string actualText = (string)converter.Convert(value, null, null, null);

        actualText.Should().Be(expectedText);
    }

    [Fact]
    public void HavingNotDeclaredSprintStateValue_WhenConverting_ThenThrows()
    {
        SprintState value = (SprintState)34504;

        Action action = () =>
        {
            _ = converter.Convert(value, null, null, null);
        };

        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}