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
using System.IO;
using Microsoft.Deployment.WindowsInstaller;
using Newtonsoft.Json;

namespace DustInTheWind.VeloCity.Installer.CustomActions
{
    public static class ReadFromConfigFileCustomAction
    {
        [CustomAction("ReadFromConfigFile")]
        public static ActionResult Execute(Session session)
        {
            try
            {
                session.Log("Begin ReadFromConfigFile Custom Action");

                string installDir = session["INSTALLDIR_CLI"];

                const string configFileName = "appsettings.json";
                string configFilePath = Path.Combine(installDir, configFileName);

                if (!File.Exists(configFilePath))
                {
                    session.Log($"WARNING: Configuration file could not be found: '{configFilePath}'.");
                    return ActionResult.Success;
                }

                session.Log($"Open config file {configFilePath}");
                string inputJson = File.ReadAllText(configFilePath);

                dynamic jsonObj = JsonConvert.DeserializeObject(inputJson);
                dynamic databaseJsonLocation = jsonObj?["DatabaseLocation"];

                if (databaseJsonLocation != null)
                    session["DATABASE_JSON_LOCATION"] = databaseJsonLocation;
                else
                    session.Log("DatabaseLocation property not found in the config file.");

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("ERROR: {0}", ex);
                return ActionResult.Failure;
            }
            finally
            {
                session.Log("End ReadFromConfigFile Custom Action");
            }
        }
    }
}