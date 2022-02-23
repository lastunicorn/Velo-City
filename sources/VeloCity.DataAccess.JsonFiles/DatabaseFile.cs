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

using System;
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.VeloCity.JsonFiles
{
    public class DatabaseFile
    {
        private const string FileName = "database.json";
        private readonly string rootDirectoryPath;

        public DatabaseDocument Document { get; set; }

        public DatabaseFile(string rootDirectoryPath)
        {
            this.rootDirectoryPath = rootDirectoryPath ?? throw new ArgumentNullException(nameof(rootDirectoryPath));
        }

        public void Open()
        {
            string json = File.ReadAllText(FileName);
            Document = JsonConvert.DeserializeObject<DatabaseDocument>(json);
        }

        public void Save()
        {
            JsonSerializerSettings settings = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(Document, settings);
            File.WriteAllText(FileName, json);
        }
    }
}