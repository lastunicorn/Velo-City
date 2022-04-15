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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Application.PresentForecast
{
    internal class PresentForecastUseCase : IRequestHandler<PresentForecastRequest, PresentForecastResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfig config;

        public PresentForecastUseCase(IUnitOfWork unitOfWork, IConfig config)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<PresentForecastResponse> Handle(PresentForecastRequest request, CancellationToken cancellationToken)
        {
            SprintFactory sprintFactory = new(unitOfWork, request.ExcludedTeamMembers);
            List<Sprint> historySprints = sprintFactory.GetLastClosed(config.AnalysisLookBack, request.ExcludedSprints);
            SprintsSpace sprintsSpace = CreateSprintsSpace(request.Date, sprintFactory);

            Forecast forecast = new()
            {
                HistorySprints = historySprints,
                FutureSprints = sprintsSpace.AllSprints
            };
            forecast.Calculate();

            PresentForecastResponse response = new()
            {
                StartDate = sprintsSpace.ActualStartDate,
                EndDate = sprintsSpace.ActualEndDate,
                TotalWorkHours = forecast.TotalWorkHours,
                EstimatedVelocity = forecast.EstimatedVelocity,
                EstimatedStoryPoints = forecast.EstimatedStoryPoints,
                EstimatedStoryPointsWithVelocityPenalties = forecast.EstimatedStoryPointsWithVelocityPenalties,
                Sprints = forecast.ForecastSprints
            };

            return Task.FromResult(response);
        }

        private static SprintsSpace CreateSprintsSpace(DateTime? requestedEndDate, SprintFactory sprintFactory)
        {
            Sprint currentSprint = sprintFactory.GetCurrentSprint();

            DateTime startDate = currentSprint.EndDate.AddDays(1);
            DateTime endDate = requestedEndDate ?? startDate.AddDays(30);

            SprintsSpace sprintsSpace = new(sprintFactory)
            {
                DateInterval = new DateInterval(startDate, endDate),
                ExistingSprints = sprintFactory.GetExistingSprints(startDate, endDate)
            };
            sprintsSpace.GenerateMissingSprints();

            return sprintsSpace;
        }
    }
}