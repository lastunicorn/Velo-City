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
using DustInTheWind.VeloCity.Application.PresentSprintCalendar;
using DustInTheWind.VeloCity.Domain;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprintCalendar
{
    public class PresentSprintCalendarCommand : ICommand
    {
        private readonly IMediator mediator;

        public string SprintName { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public List<SprintDay> Days { get; private set; }

        public PresentSprintCalendarCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(Arguments arguments)
        {
            PresentSprintCalendarRequest request = new()
            {
                SprintNumber = 24
            };

            PresentSprintCalendarResponse response = await mediator.Send(request);

            SprintName = response.SprintName;
            StartDate = response.StartDate;
            EndDate = response.EndDate;
            Days = response.Days;
        }
    }
}