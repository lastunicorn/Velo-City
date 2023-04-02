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

using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.VeloCity.Installer.CustomActions
{
    public static class ReadFromConfigFileCustomAction
    {
        [CustomAction("ReadFromConfigFile")]
        public static ActionResult Execute(Session session)
        {
            ExecutionContext executionContext = new ExecutionContext(session);

            return executionContext.Execute("ReadFromConfigFile", log =>
            {
                try
                {
                    string installDir = session["INSTALLDIR_CLI"];

                    string databaseJsonLocation = ReadDatabaseJsonLocation(installDir);

                    if (databaseJsonLocation != null)
                        session["DATABASE_JSON_LOCATION"] = databaseJsonLocation;
                    else
                        log.Warning("DatabaseLocation property not found in the config file.");
                }
                catch (MissingConfigurationFileException ex)
                {
                    log.Warning(ex);
                }
            });
        }

        private static string ReadDatabaseJsonLocation(string installDir)
        {
            ConfigFile configFile = new ConfigFile(installDir);

            configFile.Open();
            return configFile.DatabaseLocation;
        }
    }
}