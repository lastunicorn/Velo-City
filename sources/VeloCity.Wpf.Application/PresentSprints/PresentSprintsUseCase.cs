// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprints;

internal class PresentSprintsUseCase : IRequestHandler<PresentSprintsRequest, PresentSprintsResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ApplicationState applicationState;

    public PresentSprintsUseCase(IUnitOfWork unitOfWork, ApplicationState applicationState)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
    }

    public async Task<PresentSprintsResponse> Handle(PresentSprintsRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<Sprint> allSprints = await unitOfWork.SprintRepository.GetAll();

        return new PresentSprintsResponse
        {
            Sprints = allSprints
                .OrderByDescending(x => x.StartDate)
                .Select(x => new SprintDto(x))
                .ToList(),
            CurrentSprintId = applicationState.SelectedSprintId
        };
    }
}