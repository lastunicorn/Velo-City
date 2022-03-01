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
using DustInTheWind.VeloCity.Domain;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class Config : IConfig
    {
        private readonly IConfiguration config;
        
        public ErrorMessageLevel ErrorMessageLevel
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection("ErrorMessageLevel");

                return configurationSection.Exists()
                    ? (ErrorMessageLevel)Enum.Parse(typeof(ErrorMessageLevel), configurationSection.Value, true)
                    : Domain.ErrorMessageLevel.Simple;
            }
        }

        public string DatabaseLocation
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection("DatabaseLocation");

                return configurationSection.Exists()
                    ? configurationSection.Value
                    : "velo-city-database.json";
            }
        }

        public string DatabaseEditor
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection("DatabaseEditor");

                return configurationSection.Exists()
                    ? configurationSection.Value
                    : null;
            }
        }

        public string DatabaseEditorArguments
        {
            get
            {
                IConfigurationSection configurationSection = config.GetSection("DatabaseEditorArguments");

                return configurationSection.Exists()
                    ? configurationSection.Value
                    : null;
            }
        }

        public Config()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}