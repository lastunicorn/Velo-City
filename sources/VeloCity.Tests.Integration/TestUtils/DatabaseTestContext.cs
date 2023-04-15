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

using DustInTheWind.VeloCity.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.Tests.Integration.TestUtils;

public class DatabaseTestContext
{
    private readonly string filePath;

    public JsonDatabase JsonDatabase { get; private set; }

    public VeloCityDbContext VeloCityDbContext { get; private set; }

    public DatabaseAssertsContext Asserts { get; }

    private DatabaseTestContext(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));

        Asserts = new DatabaseAssertsContext(filePath);
    }

    public async Task Execute(Func<DatabaseTestContext, Task> action)
    {
        using BackupFile backupFile = new(filePath);
        backupFile.CreateBackup();

        try
        {
            OpenDatabase();

            await action(this);
        }
        finally
        {
            backupFile.RestoreFromBackup();
        }
    }

    public void Execute(Action<DatabaseTestContext> action)
    {
        using BackupFile backupFile = new(filePath);
        backupFile.CreateBackup();

        try
        {
            OpenDatabase();

            action(this);
        }
        finally
        {
            backupFile.RestoreFromBackup();
        }
    }

    private void OpenDatabase()
    {
        JsonDatabase = new JsonDatabase
        {
            PersistenceLocation = filePath
        };
        JsonDatabase.Open();

        VeloCityDbContext = new VeloCityDbContext(JsonDatabase);
    }

    public static DatabaseTestContext WithDatabase(string filePath)
    {
        return new DatabaseTestContext(filePath);
    }

    public static DatabaseTestContext WithDatabase(string directoryPath, string fileName)
    {
        string databaseFilePath = Path.Combine(directoryPath, fileName);

        return new DatabaseTestContext(databaseFilePath);
    }
}