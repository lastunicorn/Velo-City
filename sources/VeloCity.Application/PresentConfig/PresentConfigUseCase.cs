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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using MediatR;

namespace DustInTheWind.VeloCity.Application.PresentConfig
{
    internal class PresentConfigUseCase : IRequestHandler<PresentConfigRequest, PresentConfigResponse>
    {
        private readonly IConfig config;

        public PresentConfigUseCase(IConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<PresentConfigResponse> Handle(PresentConfigRequest request, CancellationToken cancellationToken)
        {
            List<ConfigItem> values = config.GetAllValuesRaw();

            if (request.ConfigPropertyName != null)
            {
                values = values
                    .Where(x => x.Name == request.ConfigPropertyName)
                    .ToList();

                if (values.Count == 0)
                    throw new Exception($"There is no property with the name {request.ConfigPropertyName}.");
            }

            PresentConfigResponse response = new()
            {
                ConfigValues = values
            };

            return Task.FromResult(response);
        }
    }
}