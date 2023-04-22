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

namespace DustInTheWind.VeloCity.JsonFiles.DatabaseVersionModel;

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

internal class DatabaseVersionsTable
{
    private readonly Version libraryVersion;

    private static readonly Dictionary<Version, Version> DatabaseVersionsByLibraryVersion = new()
    {
        { new Version(1, 0, 0), new Version(1, 0, 0) },
        { new Version(1, 7, 0), new Version(2, 0, 0) }
    };

    public DatabaseVersionsTable(Version libraryVersion)
    {
        this.libraryVersion = libraryVersion ?? throw new ArgumentNullException(nameof(libraryVersion));
    }

    public VersionValidationResult ValidateVersion(Version databaseVersion)
    {
        VersionValidationResult result = new();
        Version expectedDatabaseVersion = ComputeExpectedDatabaseVersion();

        if (databaseVersion == null)
        {
            result.Warning = new MissingDatabaseVersionWarning(expectedDatabaseVersion);
        }
        else
        {
            if (databaseVersion.Major > expectedDatabaseVersion.Major)
                result.Exception = new TooBigDatabaseVersionException(databaseVersion, expectedDatabaseVersion);
            else if (databaseVersion.Major < expectedDatabaseVersion.Major)
                result.Exception = new TooSmallDatabaseVersionException(databaseVersion, expectedDatabaseVersion);
            else if (databaseVersion != expectedDatabaseVersion)
                result.Warning = new DatabaseVersionWarning(databaseVersion, expectedDatabaseVersion);
        }

        return result;
    }

    private Version ComputeExpectedDatabaseVersion()
    {
        return DatabaseVersionsByLibraryVersion
            .Where(x => x.Key <= libraryVersion)
            .MaxBy(x => x.Key)
            .Value;
    }
}