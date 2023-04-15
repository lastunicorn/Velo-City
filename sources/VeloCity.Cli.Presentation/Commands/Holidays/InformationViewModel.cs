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

using DustInTheWind.VeloCity.Cli.Application.PresentOfficialHolidays;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Holidays;

public class InformationViewModel
{
    private readonly PresentOfficialHolidaysResponse response;

    public InformationViewModel(PresentOfficialHolidaysResponse response)
    {
        this.response = response ?? throw new ArgumentNullException(nameof(response));
    }

    public override string ToString()
    {
        return response.RequestType switch
        {
            RequestType.BySprint => $"The official holidays for the sprint {response.SprintNumber} ({response.SprintDateInterval}):",
            RequestType.ByYear => $"The official holidays for the year {response.Year}:",
            RequestType.ByCurrentYear => $"The official holidays for the current year {response.Year}:",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}