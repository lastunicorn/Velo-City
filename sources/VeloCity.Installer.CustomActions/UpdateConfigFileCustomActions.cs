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
using Microsoft.Deployment.WindowsInstaller;
using Newtonsoft.Json;

namespace DustInTheWind.VeloCity.Installer.CustomActions
{
    public static class UpdateConfigFileCustomActions
    {
        [CustomAction("UpdateConfigFile")]
        public static ActionResult Execute(Session session)
        {
            try
            {
                session.Log("Begin UpdateConfigFile Custom Action");

                string databaseJsonLocation = session.CustomActionData["DatabaseJsonLocation"];
                
                string installDirCli = session.CustomActionData["InstallDirCli"];
                UpdateConfigFile(installDirCli, databaseJsonLocation);
                
                string installDirGui = session.CustomActionData["InstallDirGui"];
                UpdateConfigFile(installDirGui, databaseJsonLocation);

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("ERROR: {0}", ex);
                return ActionResult.Failure;
            }
            finally
            {
                session.Log("End UpdateConfigFile Custom Action");
            }
        }

        private static void UpdateConfigFile(string installDir, string databaseFilePath)
        {
            const string configFileName = "appsettings.json";
            string configFilePath = Path.Combine(installDir, configFileName);

            if (!File.Exists(configFilePath))
                throw new MissingConfigurationFileException(configFilePath);

            string inputJson = File.ReadAllText(configFilePath);

            dynamic jsonObj = JsonConvert.DeserializeObject(inputJson);
            jsonObj["DatabaseLocation"] = databaseFilePath;

            string outputJson = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(configFilePath, outputJson);
        }
    }
}