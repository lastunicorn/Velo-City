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
using System.Collections.Generic;
using System.Threading.Tasks;
using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentConfig;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Config
{
    [Command("config", ShortDescription = "Inspect and change the application's configuration values.", Order = 100)]
    public class ConfigCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(DisplayName = "property name", Name = "get", Order = 1, IsOptional = true)]
        public string PropertyName { get; set; }

        public List<ConfigItem> ConfigValues { get; private set; }

        public ConfigCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentConfigRequest request = new()
            {
                ConfigPropertyName = PropertyName
            };
            PresentConfigResponse response = await mediator.Send(request);

            ConfigValues = response.ConfigValues;
        }
    }
}