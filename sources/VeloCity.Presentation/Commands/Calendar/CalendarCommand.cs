// Velo City
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
using DustInTheWind.VeloCity.Application.PresentSprintCalendar;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.Calendar
{
    [Command("calendar", ShortDescription = "The calendar details for a sprint.", Order = 6)]
    public class CalendarCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Name = "sprint", ShortName = 's', Order = 1, IsOptional = true)]
        public int? SprintNumber { get; set; }

        [CommandParameter(Name = "start-date", ShortName = 'a', IsOptional = true)]
        public DateTime StartDate { get; set; }

        [CommandParameter(Name = "end-date", ShortName = 'z', IsOptional = true)]
        public DateTime EndDate { get; set; }

        public string SprintName { get; private set; }

        public List<SprintDay> Days { get; private set; }

        public List<SprintMember> SprintMembers { get; private set; }

        public CalendarCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentSprintCalendarRequest request = new()
            {
                SprintNumber = SprintNumber,
                StartDate = StartDate,
                EndDate = EndDate
            };

            PresentSprintCalendarResponse response = await mediator.Send(request);

            SprintName = response.SprintName;
            StartDate = response.StartDate;
            EndDate = response.EndDate;
            Days = response.Days;
            SprintMembers = response.SprintMembers;
        }
    }
}