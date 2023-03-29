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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprintDetails
{
    internal class PresentSprintDetailsUseCase : IRequestHandler<PresentSprintDetailRequest, PresentSprintDetailResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;

        public PresentSprintDetailsUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public async Task<PresentSprintDetailResponse> Handle(PresentSprintDetailRequest request, CancellationToken cancellationToken)
        {
            Sprint currentSprint = await RetrieveSprintToAnalyze(request);

            return CreateResponse(currentSprint);
        }

        private async Task<Sprint> RetrieveSprintToAnalyze(PresentSprintDetailRequest request)
        {
            return request.SprintId == null
                ? applicationState.SelectedSprintId == null
                    ? await RetrieveDefaultSprintToAnalyze()
                    : await RetrieveSpecificSprintToAnalyze(applicationState.SelectedSprintId.Value)
                : await RetrieveSpecificSprintToAnalyze(request.SprintId.Value);
        }

        private async Task<Sprint> RetrieveDefaultSprintToAnalyze()
        {
            Sprint sprint = await unitOfWork.SprintRepository.GetLastInProgress();

            return sprint ?? throw new NoSprintInProgressException();
        }

        private async Task<Sprint> RetrieveSpecificSprintToAnalyze(int sprintId)
        {
            Sprint sprint = await unitOfWork.SprintRepository.Get(sprintId);
            
            return sprint ?? throw new SprintDoesNotExistException(sprintId);
        }

        private static PresentSprintDetailResponse CreateResponse(Sprint sprint)
        {
            return new PresentSprintDetailResponse
            {
                SprintId = sprint.Id,
                SprintTitle = sprint.Title,
                SprintNumber = sprint.Number,
                SprintState = sprint.State
            };
        }
    }
}