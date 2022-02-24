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
using System.Threading;
using DustInTheWind.VeloCity.Installer.CustomActions.WinForms;
using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.VeloCity.Installer.CustomActions
{
    public static class SelectDatabaseJsonFileCustomActions
    {
        [CustomAction("SelectDatabaseJsonFile")]
        public static ActionResult Execute(Session session)
        {
            try
            {
                session.Log("Begin SelectDatabaseJsonFile Custom Action");

                Thread task = new Thread(GetFile);
                task.SetApartmentState(ApartmentState.STA);
                task.Start(session);
                task.Join();

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("ERROR: {0}", ex);
                return ActionResult.Failure;
            }
            finally
            {
                session.Log("End SelectDatabaseJsonFile Custom Action");
            }
        }

        private static void GetFile(object o)
        {
            if (o is Session session)
            {
                string filePath = ChooseJsonFile.GetFile();

                if (filePath != null)
                    session["DATABASE_JSON_LOCATION"] = filePath;
            }
        }
    }
}