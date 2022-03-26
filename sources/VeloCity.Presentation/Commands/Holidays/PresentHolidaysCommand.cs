﻿// Velo City
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
using System.Collections.Generic;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Application.PresentOfficialHolidays;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.Holidays
{
    [Command("holidays", ShortDescription = "Displays the holidays for a specific year or sprint.", Order = 5)]
    [CommandUsage("holidays")]
    [CommandUsage("holidays [year]")]
    [CommandUsage("holidays -year [year]")]
    public class PresentHolidaysCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Name = "year", Order = 1, IsOptional = true)]
        public int? Year { get; set; }

        [CommandParameter(Name = "sprint", IsOptional = true)]
        public int? Sprint { get; set; }

        public List<OfficialHoliday> OfficialHolidays { get; private set; }

        public RequestTypeViewModel RequestType { get; private set; }

        public PresentHolidaysCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentOfficialHolidaysRequest request = new()
            {
                Year = Year,
                SprintNumber = Sprint
            };
            PresentOfficialHolidaysResponse response = await mediator.Send(request);

            OfficialHolidays = response.OfficialHolidays;
            RequestType = new RequestTypeViewModel(response);
        }
    }
}