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

using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentOfficialHolidays;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Holidays;

[Command("holidays", ShortDescription = "The holidays for a specific year or sprint.", Order = 7)]
public class HolidaysCommand : ICommand
{
    private readonly IMediator mediator;

    [CommandParameter(Name = "year", ShortName = 'y', Order = 1, IsOptional = true)]
    public int? Year { get; set; }

    [CommandParameter(Name = "sprint", ShortName = 's', IsOptional = true)]
    public int? Sprint { get; set; }

    [CommandParameter(Name = "country", ShortName = 'c', IsOptional = true)]
    public string Country { get; set; }

    public List<OfficialHolidayInstance> OfficialHolidays { get; private set; }

    public InformationViewModel Information { get; private set; }

    public HolidaysCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        PresentOfficialHolidaysRequest request = new()
        {
            Year = Year,
            SprintNumber = Sprint,
            Country = Country
        };
        PresentOfficialHolidaysResponse response = await mediator.Send(request);

        OfficialHolidays = response.OfficialHolidays;
        Information = new InformationViewModel(response);
    }
}