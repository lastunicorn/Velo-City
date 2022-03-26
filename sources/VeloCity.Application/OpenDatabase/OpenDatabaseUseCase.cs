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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using MediatR;

namespace DustInTheWind.VeloCity.Application.OpenDatabase
{
    internal class OpenDatabaseUseCase : IRequestHandler<OpenDatabaseRequest, OpenDatabaseResponse>
    {
        private readonly IConfig config;

        public OpenDatabaseUseCase(IConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public Task<OpenDatabaseResponse> Handle(OpenDatabaseRequest request, CancellationToken cancellationToken)
        {
            string databaseFilePath = config.DatabaseLocation;

            CheckDatabaseFileExists(databaseFilePath);
            DatabaseEditorType databaseEditorType = OpenDatabaseFile(databaseFilePath);

            OpenDatabaseResponse response = new()
            {
                DatabaseFilePath = databaseFilePath,
                DatabaseEditorType = databaseEditorType
            };

            return Task.FromResult(response);
        }

        private static void CheckDatabaseFileExists(string databaseFilePath)
        {
            if (!File.Exists(databaseFilePath))
                throw new DatabaseFileNotFoundException(databaseFilePath);
        }

        private DatabaseEditorType OpenDatabaseFile(string databaseFilePath)
        {
            try
            {
                string fileName;
                string arguments;

                bool isCustomEditorProvided = !string.IsNullOrEmpty(config.DatabaseEditor);
                if (isCustomEditorProvided)
                {
                    fileName = config.DatabaseEditor;

                    bool areCustomArgumentsProvided = !string.IsNullOrEmpty(config.DatabaseEditorArguments);
                    arguments = areCustomArgumentsProvided
                        ? string.Format(config.DatabaseEditorArguments, databaseFilePath)
                        : $@"""{databaseFilePath}""";
                }
                else
                {
                    fileName = $@"""{databaseFilePath}""";
                    arguments = string.Empty;
                }

                Process process = new()
                {
                    StartInfo = new()
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        UseShellExecute = true,
                    }
                };

                process.Start();

                return isCustomEditorProvided
                    ? DatabaseEditorType.Custom
                    : DatabaseEditorType.Default;
            }
            catch (Exception ex)
            {
                throw new DatabaseOpenException(ex);
            }
        }
    }
}