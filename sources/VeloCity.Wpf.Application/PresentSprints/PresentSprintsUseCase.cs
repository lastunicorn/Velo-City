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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentSprints
{
    internal class PresentSprintsUseCase : IRequestHandler<PresentSprintsRequest, PresentSprintsResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentSprintsUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentSprintsResponse> Handle(PresentSprintsRequest request, CancellationToken cancellationToken)
        {
            PresentSprintsResponse response = new()
            {
                Sprints = unitOfWork.SprintRepository.GetAll()
                    .OrderByDescending(x => x.StartDate)
                    .Select(x => new SprintInfo(x))
                    .ToList()
            };

            return Task.FromResult(response);
        }
    }
}