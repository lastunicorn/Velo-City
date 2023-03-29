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
using System.IO;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;
using Newtonsoft.Json;

namespace DustInTheWind.VeloCity.JsonFiles;

internal class JsonDatabaseFile
{
    private readonly string filePath;

    public JsonDatabaseDocument Document { get; set; }

    public WarningException LastWarning { get; private set; }

    public JsonDatabaseFile(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public void Open()
    {
        LastWarning = null;

        if (!File.Exists(filePath))
            throw new DatabaseNotFoundException(filePath);

        string json = File.ReadAllText(filePath);
        Document = JsonConvert.DeserializeObject<JsonDatabaseDocument>(json);

        DatabaseVersionValidator databaseVersionValidator = new();

        try
        {
            databaseVersionValidator.CheckDatabaseVersion(Document?.DatabaseInfo?.DatabaseVersion);
        }
        finally
        {
            LastWarning = databaseVersionValidator.Warning;
        }
    }

    public void Save()
    {
        JsonSerializerSettings settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        string json = JsonConvert.SerializeObject(Document, settings);
        File.WriteAllText(filePath, json);
    }
}