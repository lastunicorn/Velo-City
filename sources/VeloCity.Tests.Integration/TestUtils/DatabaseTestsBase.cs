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

using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.Tests.Integration.TestUtils;

public class DatabaseTestsBase : IDisposable
{
    private readonly BackupFile backupFile;
    private readonly string filePath;

    protected JsonDatabase JsonDatabase { get; private set; }

    protected VeloCityDbContext VeloCityDbContext { get; private set; }

    protected DatabaseAssertsContext DatabaseAsserts { get;}

    public DatabaseTestsBase(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

        backupFile = new BackupFile(filePath);

        DatabaseAsserts = new DatabaseAssertsContext(filePath);
    }

    protected void OpenDatabase()
    {
        backupFile.CreateBackup();

        JsonDatabase = new JsonDatabase
        {
            PersistenceLocation = filePath
        };
        JsonDatabase.Open();

        VeloCityDbContext = new VeloCityDbContext(JsonDatabase);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            backupFile.RestoreFromBackup();
            backupFile.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}