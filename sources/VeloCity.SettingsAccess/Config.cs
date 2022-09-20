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
using System.Collections.Generic;
using System.Globalization;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.SettingsAccess
{
    public class Config : IConfig
    {
        private const string CulturePropertyName = "Culture";
        private const string ErrorMessageLevelPropertyName = "ErrorMessageLevel";
        private const string DatabaseLocationPropertyName = "DatabaseLocation";
        private const string DatabaseEditorPropertyName = "DatabaseEditor";
        private const string DatabaseEditorArgumentsPropertyName = "DatabaseEditorArguments";
        private const string DataGridStylePropertyName = "DataGridStyle";
        private const string AnalysisLookBackPropertyName = "AnalysisLookBack";

        private readonly IConfiguration config;

        public CultureInfo Culture
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection(CulturePropertyName);

                return configurationSection.Exists()
                    ? new CultureInfo(configurationSection.Value)
                    : CultureInfo.CurrentCulture;
            }
        }

        public ErrorMessageLevel ErrorMessageLevel
        {
            get
            {
                try
                {
                    IConfigurationSection configurationSection = config.GetSection(ErrorMessageLevelPropertyName);

                    return configurationSection.Exists()
                        ? (ErrorMessageLevel)Enum.Parse(typeof(ErrorMessageLevel), configurationSection.Value, true)
                        : ErrorMessageLevel.Simple;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationElementException(ErrorMessageLevelPropertyName, ex);
                }
            }
        }

        public string DatabaseLocation
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection(DatabaseLocationPropertyName);

                return configurationSection.Exists()
                    ? configurationSection.Value
                    : "velo-city-database.json";
            }
        }

        public string DatabaseEditor
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection(DatabaseEditorPropertyName);

                return configurationSection.Exists()
                    ? configurationSection.Value
                    : null;
            }
        }

        public string DatabaseEditorArguments
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection(DatabaseEditorArgumentsPropertyName);

                return configurationSection.Exists()
                    ? configurationSection.Value
                    : null;
            }
        }

        public DataGridStyle DataGridStyle
        {
            get
            {
                try
                {
                    IConfigurationSection configurationSection = config.GetSection(DataGridStylePropertyName);

                    return configurationSection.Exists()
                        ? (DataGridStyle)Enum.Parse(typeof(DataGridStyle), configurationSection.Value, true)
                        : DataGridStyle.PlusMinus;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationElementException(DataGridStylePropertyName, ex);
                }
            }
        }

        public uint AnalysisLookBack
        {
            get
            {
                try
                {
                    IConfigurationSection configurationSection = config.GetSection(AnalysisLookBackPropertyName);

                    return configurationSection.Exists()
                        ? uint.Parse(configurationSection.Value)
                        : 3;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationElementException(AnalysisLookBackPropertyName, ex);
                }
            }
        }

        public Config()
        {
            try
            {
                config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
            catch (Exception ex)
            {
                throw new ConfigurationOpenException(ex);
            }
        }

        public List<ConfigItem> GetAllValuesRaw()
        {
            return new List<ConfigItem>
            {
                new()
                {
                    Name = ErrorMessageLevelPropertyName,
                    Value = ErrorMessageLevel.ToString()
                },
                new()
                {
                    Name = DatabaseLocationPropertyName,
                    Value = DatabaseLocation
                },
                new()
                {
                    Name = DatabaseEditorPropertyName,
                    Value = DatabaseEditor
                },
                new()
                {
                    Name = DatabaseEditorArgumentsPropertyName,
                    Value = DatabaseEditorArguments
                },
                new()
                {
                    Name = DataGridStylePropertyName,
                    Value = DataGridStyle.ToString()
                }
            };
        }
    }
}