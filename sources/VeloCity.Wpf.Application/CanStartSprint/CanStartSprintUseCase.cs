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
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.CanStartSprint
{
    internal class CanStartSprintUseCase : IRequestHandler<CanStartSprintRequest, CanStartSprintResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationState applicationState;

        public CanStartSprintUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        }

        public Task<CanStartSprintResponse> Handle(CanStartSprintRequest request, CancellationToken cancellationToken)
        {
            CanStartSprintResponse response = new()
            {
                CanStartSprint = CanStartSelectedSprint()
            };

            return Task.FromResult(response);
        }

        private bool CanStartSelectedSprint()
        {
            Sprint sprint = RetrieveSelectedSprint();

            return sprint != null &&
                   IsCorrectState(sprint) &&
                   NoSprintIsInProgress() &&
                   IsFirstNewSprint(sprint);
        }

        private Sprint RetrieveSelectedSprint()
        {
            int? sprintId = applicationState.SelectedSprintNumber;

            return sprintId != null
                ? unitOfWork.SprintRepository.Get(sprintId.Value)
                : null;
        }

        private static bool IsCorrectState(Sprint sprint)
        {
            return sprint.State switch
            {
                SprintState.New => true,
                SprintState.Unknown => false,
                SprintState.InProgress => false,
                SprintState.Closed => false,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool NoSprintIsInProgress()
        {
            return !unitOfWork.SprintRepository.IsAnyInProgress();
        }

        private bool IsFirstNewSprint(Sprint sprint)
        {
            return unitOfWork.SprintRepository.IsFirstNewSprint(sprint.Id);
        }
    }
}