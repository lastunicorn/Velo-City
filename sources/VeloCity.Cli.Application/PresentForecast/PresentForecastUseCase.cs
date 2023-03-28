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

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentForecast
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

        public async Task<PresentForecastResponse> Handle(PresentForecastRequest request, CancellationToken cancellationToken)
        {
            Forecast forecast = await CalculateForecast(request);

            return CreateResponse(forecast);
        }

        private async Task<Forecast> CalculateForecast(PresentForecastRequest request)
        {
            Forecast forecast = new(unitOfWork)
            {
                EndDate = request.EndDate,
                AnalysisLookBack = request.AnalysisLookBack ?? config.AnalysisLookBack,
                ExcludedSprints = request.ExcludedSprints,
                ExcludedTeamMembers = request.ExcludedTeamMembers
            };
            await forecast.Calculate();

            return forecast;
        }

        private static PresentForecastResponse CreateResponse(Forecast forecast)
        {
            return new PresentForecastResponse
            {
                StartDate = forecast.ActualStartDate,
                EndDate = forecast.ActualEndDate,
                TotalWorkHours = forecast.TotalWorkHours,
                EstimatedVelocity = forecast.EstimatedVelocity,
                EstimatedStoryPoints = forecast.EstimatedStoryPoints,
                EstimatedStoryPointsWithVelocityPenalties = forecast.EstimatedStoryPointsWithVelocityPenalties,
                PreviouslyClosedSprints = forecast.HistorySprints?
                    .Select(x => x.Number)
                    .ToList(),
                Sprints = forecast.ForecastSprints
            };
        }
    }
}