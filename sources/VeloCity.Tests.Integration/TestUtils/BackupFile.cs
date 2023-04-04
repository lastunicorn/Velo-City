﻿// VeloCity
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

namespace DustInTheWind.VeloCity.Tests.Integration.TestUtils;

internal sealed class BackupFile : IDisposable
{
    private readonly string filePath;
    private string tempFilePath;

    public BackupFile(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public void CreateBackup()
    {
        tempFilePath = Path.GetTempFileName();

        using Stream destinationStream = File.Open(tempFilePath, FileMode.Create);
        using Stream sourceStream = File.OpenRead(filePath);

        sourceStream.CopyTo(destinationStream);
    }

    public void RestoreFromBackup()
    {
        File.Copy(tempFilePath, filePath, true);
    }

    public void DeleteBackup()
    {
        File.Delete(tempFilePath);
    }

    public void Dispose()
    {
        DeleteBackup();
    }
}