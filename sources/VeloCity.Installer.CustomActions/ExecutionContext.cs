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
using Microsoft.Deployment.WindowsInstaller;

namespace DustInTheWind.VeloCity.Installer.CustomActions
{
    internal class ExecutionContext
    {
        private readonly Log log;

        public ExecutionContext(Session session)
        {
            log = new Log(session);
        }

        public ActionResult Execute(string customActionName, Action<Log> action)
        {
            try
            {
                log.Info($"Begin {customActionName} Custom Action");

                action(log);

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return ActionResult.Failure;
            }
            finally
            {
                log.Info($"End {customActionName} Custom Action");
            }
        }
    }
}