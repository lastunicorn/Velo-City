﻿// VeloCity
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

using DustInTheWind.VeloCity.Ports.SettingsAccess;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.SettingsAccess;

internal class DataGridStyleProperty
{
    public const string PropertyName = "DataGridStyle";

    private readonly IConfiguration config;

    public DataGridStyle Value
    {
        get
        {
            try
            {
                IConfigurationSection configurationSection = config.GetSection(PropertyName);

                return configurationSection.Exists()
                    ? (DataGridStyle)Enum.Parse(typeof(DataGridStyle), configurationSection.Value, true)
                    : DataGridStyle.PlusMinus;
            }
            catch (Exception ex)
            {
                throw new ConfigurationElementException(PropertyName, ex);
            }
        }
    }

    public ConfigItem Raw => new()
    {
        Name = PropertyName,
        Value = Value.ToString()
    };

    public DataGridStyleProperty(IConfiguration config)
    {
        this.config = config ?? throw new ArgumentNullException(nameof(config));
    }
}