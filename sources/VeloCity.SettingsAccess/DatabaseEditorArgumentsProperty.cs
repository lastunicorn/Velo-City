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

using DustInTheWind.VeloCity.Ports.SettingsAccess;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.SettingsAccess;

internal class DatabaseEditorArgumentsProperty
{
    public const string PropertyName = "DatabaseEditorArguments";

    private readonly IConfiguration config;

    public string Value
    {
        get
        {
            IConfigurationSection configurationSection = config.GetSection(PropertyName);

            return configurationSection.Exists()
                ? configurationSection.Value
                : null;
        }
    }

    public ConfigItem Raw => new()
    {
        Name = PropertyName,
        Value = Value
    };

    public DatabaseEditorArgumentsProperty(IConfiguration config)
    {
        this.config = config ?? throw new ArgumentNullException(nameof(config));
    }
}