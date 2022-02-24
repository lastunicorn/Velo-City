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

using Microsoft.Extensions.Configuration;

namespace DustInTheWind.VeloCity.Bootstrapper
{
    internal class Config
    {
        private readonly IConfiguration config;

        public bool DebugVerbose
        {
            get
            {
                IConfigurationSection debugConfigurationSection = config.GetSection("Debug");

                if (debugConfigurationSection.Exists())
                {
                    IConfigurationSection verboseConfigurationSection = debugConfigurationSection.GetSection("Verbose");

                    if (verboseConfigurationSection.Exists())
                    {
                        string rawValue = verboseConfigurationSection.Value;
                        bool success = bool.TryParse(rawValue, out bool value);

                        return success && value;
                    }
                }

                return false;
            }
        }

        public string DatabaseLocation
        {
            get
            {
                IConfigurationSection debugConfigurationSection = config.GetSection("DatabaseLocation");

                return debugConfigurationSection.Exists()
                    ? debugConfigurationSection.Value
                    : "database.json";
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