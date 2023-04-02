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

using DustInTheWind.VeloCity.Ports.SettingsAccess;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.SettingsAccess;

internal class ErrorMessageLevelProperty
{
    public const string PropertyName = "ErrorMessageLevel";

    private readonly IConfiguration config;

    public ErrorMessageLevel Value
    {
        get
        {
            try
            {
                IConfigurationSection configurationSection = config.GetSection(PropertyName);

                return configurationSection.Exists()
                    ? (ErrorMessageLevel)Enum.Parse(typeof(ErrorMessageLevel), configurationSection.Value, true)
                    : ErrorMessageLevel.Simple;
            }
            catch (Exception ex)
            {
                throw new ConfigurationElementException(PropertyName, ex);
            }
        }
    }

    public ErrorMessageLevelProperty(IConfiguration config)
    {
        this.config = config ?? throw new ArgumentNullException(nameof(config));
    }
}