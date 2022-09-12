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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.Wpf.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.Refresh
{
    public class RefreshUseCase : IRequestHandler<RefreshRequest, Unit>
    {
        private readonly EventBus eventBus;
        private readonly IUnitOfWork unitOfWork;

        public RefreshUseCase(IUnitOfWork unitOfWork, EventBus eventBus)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Unit> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            try
            {
                unitOfWork.InvalidateCash();
            }
            catch (Exception ex)
            {
            }

            ReloadEvent reloadEvent = new();
            await eventBus.Publish(reloadEvent, cancellationToken);

            return Unit.Value;
        }
    }
}