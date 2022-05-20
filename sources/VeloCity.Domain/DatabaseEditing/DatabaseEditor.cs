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

namespace DustInTheWind.VeloCity.Domain.DatabaseEditing
{
    public class DatabaseEditor
    {
        public string Editor { get; set; }

        public string EditorArguments { get; set; }

        public DatabaseEditorType EditorType => string.IsNullOrEmpty(Editor)
            ? DatabaseEditorType.Default
            : DatabaseEditorType.Custom;

        public string DatabaseFilePath { get; set; }

        public void OpenDatabase()
        {
            try
            {
                Process process = new()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = CalculateFileNameToExecute(),
                        Arguments = CalculateArguments(),
                        UseShellExecute = true,
                    }
                };

                process.Start();
            }
            catch (Exception ex)
            {
                throw new DatabaseOpenException(ex);
            }
        }

        private string CalculateArguments()
        {
            bool isCustomEditorProvided = !string.IsNullOrEmpty(Editor);
            if (!isCustomEditorProvided)
                return string.Empty;

            bool areCustomArgumentsProvided = !string.IsNullOrEmpty(EditorArguments);
            return areCustomArgumentsProvided
                ? string.Format(EditorArguments, DatabaseFilePath)
                : $@"""{DatabaseFilePath}""";
        }

        private string CalculateFileNameToExecute()
        {
            bool isCustomEditorProvided = !string.IsNullOrEmpty(Editor);

            return isCustomEditorProvided
                ? Editor
                : $@"""{DatabaseFilePath}""";
        }
    }
}