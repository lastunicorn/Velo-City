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

using DustInTheWind.VeloCity.JsonFiles.DatabaseVersionModel;

namespace DustInTheWind.VeloCity.Tests.Unit.DataAccess.JsonFiles.DatabaseVersionModel.DatabaseVersionsTableTests;

public class ValidateVersion_LibraryVersion110_Tests
{
    [Fact]
    public void HavingLibraryVersion110_WhenValidatingDatabaseVersionNull_ThenReturnsWarning()
    {
        Version libraryVersion = new(1, 1, 0);
        DatabaseVersionsTable databaseVersionsTable = new(libraryVersion);

        Version databaseVersion = null;
        VersionValidationResult result = databaseVersionsTable.ValidateVersion(databaseVersion);

        result.Exception.Should().BeNull();
        result.Warning.Should().BeOfType<MissingDatabaseVersionWarning>();
    }

    [Fact]
    public void HavingLibraryVersion110_WhenValidatingDatabaseVersion010_ThenReturnsNoErrorOrWarning()
    {
        Version libraryVersion = new(1, 1, 0);
        DatabaseVersionsTable databaseVersionsTable = new(libraryVersion);

        Version databaseVersion = new(0, 1, 0);
        VersionValidationResult result = databaseVersionsTable.ValidateVersion(databaseVersion);

        result.Exception.Should().BeOfType<TooSmallDatabaseVersionException>();
        result.Warning.Should().BeNull();
    }

    [Fact]
    public void HavingLibraryVersion110_WhenValidatingDatabaseVersion100_ThenReturnsNoErrorOrWarning()
    {
        Version libraryVersion = new(1, 1, 0);
        DatabaseVersionsTable databaseVersionsTable = new(libraryVersion);

        Version databaseVersion = new(1, 0, 0);
        VersionValidationResult result = databaseVersionsTable.ValidateVersion(databaseVersion);

        result.Exception.Should().BeNull();
        result.Warning.Should().BeNull();
    }

    [Fact]
    public void HavingLibraryVersion110_WhenValidatingDatabaseVersion110_ThenReturnsWarning()
    {
        Version libraryVersion = new(1, 1, 0);
        DatabaseVersionsTable databaseVersionsTable = new(libraryVersion);

        Version databaseVersion = new(1, 1, 0);
        VersionValidationResult result = databaseVersionsTable.ValidateVersion(databaseVersion);

        result.Exception.Should().BeNull();
        result.Warning.Should().BeOfType<DatabaseVersionWarning>();
    }

    [Fact]
    public void HavingLibraryVersion110_WhenValidatingDatabaseVersion200_ThenReturnsError()
    {
        Version libraryVersion = new(1, 1, 0);
        DatabaseVersionsTable databaseVersionsTable = new(libraryVersion);

        Version databaseVersion = new(2, 0, 0);
        VersionValidationResult result = databaseVersionsTable.ValidateVersion(databaseVersion);

        result.Exception.Should().BeOfType<TooBigDatabaseVersionException>();
        result.Warning.Should().BeNull();
    }

    [Fact]
    public void HavingLibraryVersion110_WhenValidatingDatabaseVersion210_ThenReturnsError()
    {
        Version libraryVersion = new(1, 1, 0);
        DatabaseVersionsTable databaseVersionsTable = new(libraryVersion);

        Version databaseVersion = new(2, 1, 0);
        VersionValidationResult result = databaseVersionsTable.ValidateVersion(databaseVersion);

        result.Exception.Should().BeOfType<TooBigDatabaseVersionException>();
        result.Warning.Should().BeNull();
    }
}