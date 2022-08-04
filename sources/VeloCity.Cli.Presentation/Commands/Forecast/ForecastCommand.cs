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
using System.Linq;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Cli.Application.PresentForecast;
using DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintOverview;
using DustInTheWind.VeloCity.Cli.Presentation.UserControls.Notes;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Forecast
{
    [Command("forecast", ShortDescription = "Calculates the forecast for a specific time.", Order = 4)]
    public class ForecastCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(Name = "date", ShortName = 'd', IsOptional = true)]
        public DateTime? Date { get; set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public HoursValue TotalWorkHours { get; private set; }

        public Velocity EstimatedVelocity { get; private set; }

        public StoryPoints EstimatedStoryPoints { get; private set; }

        public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; private set; }

        public List<NoteBase> Notes { get; private set; }

        public List<SprintForecast> Sprints { get; private set; }

        public ForecastCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentForecastRequest request = new()
            {
                EndDate = Date
            };
            PresentForecastResponse response = await mediator.Send(request);

            StartDate = response.StartDate;
            EndDate = response.EndDate;
            TotalWorkHours = response.TotalWorkHours;
            EstimatedVelocity = response.EstimatedVelocity;
            EstimatedStoryPoints = response.EstimatedStoryPoints;
            EstimatedStoryPointsWithVelocityPenalties = response.EstimatedStoryPointsWithVelocityPenalties;
            Sprints = response.Sprints;

            Notes = CreateNotes(response).ToList();
        }

        private static IEnumerable<NoteBase> CreateNotes(PresentForecastResponse response)
        {
            bool previousSprintsExist = response.PreviouslyClosedSprints is { Count: > 0 };

            if (previousSprintsExist)
            {
                yield return new PreviousSprintsCalculationNote
                {
                    PreviousSprintNumbers = response.PreviouslyClosedSprints
                };
            }
            else
            {
                yield return new NoPreviousSprintsNote();
            }

            if (!response.EstimatedStoryPointsWithVelocityPenalties.IsNull)
                yield return new VelocityPenaltiesNote();
        }
    }
}