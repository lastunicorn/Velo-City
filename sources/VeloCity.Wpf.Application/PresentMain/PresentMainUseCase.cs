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
using DustInTheWind.VeloCity.Domain.Configuring;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentMain
{
    public class PresentMainUseCase : IRequestHandler<PresentMainRequest, PresentMainResponse>
    {
        private readonly IConfig config;

        public PresentMainUseCase(IConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<PresentMainResponse> Handle(PresentMainRequest request, CancellationToken cancellationToken)
        {
            PresentMainResponse response = new()
            {
                DatabaseConnectionString = config.DatabaseLocation
            };

            return Task.FromResult(response);
        }
    }
}