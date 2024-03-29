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

using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.VeloCity.Installer.CustomActions
{
    public static class UpdateConfigFileCustomActions
    {
        [CustomAction("UpdateConfigFile")]
        public static ActionResult Execute(Session session)
        {
            ExecutionContext executionContext = new ExecutionContext(session);

            return executionContext.Execute("ReadFromConfigFile", log =>
            {
                string databaseJsonLocation = session.CustomActionData["DatabaseJsonLocation"];

                string installDirCli = session.CustomActionData["InstallDirCli"];
                UpdateConfigFile(installDirCli, databaseJsonLocation);

                string installDirGui = session.CustomActionData["InstallDirGui"];
                UpdateConfigFile(installDirGui, databaseJsonLocation);
            });
        }

        private static void UpdateConfigFile(string installDir, string databaseFilePath)
        {
            ConfigFile configFile = new ConfigFile(installDir);

            configFile.Open();
            configFile.DatabaseLocation = databaseFilePath;
            configFile.Save();
        }
    }
}