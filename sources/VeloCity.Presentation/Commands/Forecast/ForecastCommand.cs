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
using DustInTheWind.VeloCity.Application.PresentForecast;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.Forecast
{
    [Command("forecast", ShortDescription = "Calculates a forecast for a specific time.", Order = 3)]
    public class ForecastCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Name = "date", ShortName = 'd', IsOptional = true)]
        public DateTime? Date { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public HoursValue TotalWorkHours { get; set; }

        public Velocity EstimatedVelocity { get; set; }

        public StoryPoints EstimatedStoryPoints { get; set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; set; }

        public List<SprintForecast> Sprints { get; set; }

        public ForecastCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentForecastRequest request = new()
            {
                Date = Date
            };
            PresentForecastResponse response = await mediator.Send(request);

            StartDate = response.StartDate;
            EndDate = response.EndDate;
            TotalWorkHours = response.TotalWorkHours;
            EstimatedVelocity = response.EstimatedVelocity;
            EstimatedStoryPoints = response.EstimatedStoryPoints;
            EstimatedStoryPointsWithVelocityPenalties = response.EstimatedStoryPointsWithVelocityPenalties;
            Sprints = response.Sprints;
        }
    }
}