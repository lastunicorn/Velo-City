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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails
{
    internal class PresentSprintDetailsUseCase : IRequestHandler<PresentSprintDetailRequest, PresentSprintDetailResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfig config;
        private readonly ApplicationState applicationState;

        public PresentSprintDetailsUseCase(IUnitOfWork unitOfWork, IConfig config, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public Task<PresentSprintDetailResponse> Handle(PresentSprintDetailRequest request, CancellationToken cancellationToken)
        {
            Sprint currentSprint = RetrieveSprintToAnalyze(request);

            PresentSprintDetailResponse response = CreateResponse(currentSprint);

            return Task.FromResult(response);
        }

        private Sprint RetrieveSprintToAnalyze(PresentSprintDetailRequest request)
        {
            Sprint sprint = request.SprintNumber == null
                ? applicationState.SelectedSprintId == null
                    ? RetrieveDefaultSprintToAnalyze()
                    : RetrieveSpecificSprintToAnalyze(applicationState.SelectedSprintId.Value)
                : RetrieveSpecificSprintToAnalyze(request.SprintNumber.Value);

            return sprint;
        }

        private Sprint RetrieveDefaultSprintToAnalyze()
        {
            Sprint sprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (sprint == null)
                throw new NoSprintInProgressException();

            return sprint;
        }

        private Sprint RetrieveSpecificSprintToAnalyze(int sprintNumber)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            return sprint;
        }

        private static PresentSprintDetailResponse CreateResponse(Sprint sprint)
        {
            return new PresentSprintDetailResponse
            {
                SprintName = sprint.Name,
                SprintNumber = sprint.Number,
            };
        }
    }
}