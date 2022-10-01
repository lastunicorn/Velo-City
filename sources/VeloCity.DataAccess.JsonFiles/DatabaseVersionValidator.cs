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
using System.Linq;
using System.Reflection;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.JsonFiles
{
    /**
     * Ver 2.0.0
     * [ new      ] /DatabaseInfo/DatabaseVersion: Version (mandatory)
     * [ new      ] /OfficialHolidays/Recurrence : enum (mandatory)
     * [ changed  ] /OfficialHolidays/Date : DateTime - year is set to 100 for recurring holidays.
     * [ new      ] /OfficialHolidays/StartYear : int (optional)
     * [ new      ] /OfficialHolidays/EndYear : int (optional)
     * [ new      ] /OfficialHolidays/Country : string (optional)
     * [ obsolete ] /TeamMembers/Name: string (optional)
     * [ new      ] /TeamMembers/FirstName: string (optional)
     * [ new      ] /TeamMembers/MiddleName: string (optional)
     * [ new      ] /TeamMembers/LastName: string (optional)
     * [ new      ] /TeamMembers/Nickname: string (optional)
     * [ new      ] /TeamMembers/Employments/Country: string (optional)
     */
    internal class DatabaseVersionValidator
    {
        private static readonly Version CurrentApplicationVersion;

        private static readonly Dictionary<Version, Version> DatabaseVersionsByApplicationVersion = new()
        {
            { new Version(1, 0, 0), new Version(1, 0, 0) },
            { new Version(1, 7, 0), new Version(2, 0, 0) }
        };

        static DatabaseVersionValidator()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            CurrentApplicationVersion = assemblyName.Version;
        }

        public WarningException Warning { get; private set; }

        public void CheckDatabaseVersion(Version databaseVersion)
        {
            Version expectedDatabaseVersion = CalculateExpectedDatabaseVersion();

            if (databaseVersion == null)
            {
                Warning = new MissingDatabaseVersionWarning(expectedDatabaseVersion);
            }
            else
            {
                if (databaseVersion.Major > expectedDatabaseVersion.Major)
                    throw new TooBigDatabaseVersionException(databaseVersion, expectedDatabaseVersion);

                if (databaseVersion.Major < expectedDatabaseVersion.Major)
                    throw new TooSmallDatabaseVersionException(databaseVersion, expectedDatabaseVersion);

                if (databaseVersion != expectedDatabaseVersion)
                    Warning = new DatabaseVersionWarning(databaseVersion, expectedDatabaseVersion);
            }
        }

        private static Version CalculateExpectedDatabaseVersion()
        {
            Version suggestedApplicationVersion = DatabaseVersionsByApplicationVersion
                .Where(x => x.Key <= CurrentApplicationVersion)
                .Max(x => x.Key);

            return DatabaseVersionsByApplicationVersion[suggestedApplicationVersion];
        }
    }
}