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

using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    public class AnalyzeSprintView
    {
        public void Display(AnalyzeSprintResponse response)
        {
            DisplayOverview(response);

            if (response.WorkDays is { Count: > 0 })
                DisplayWorkDays(response);

            foreach (SprintMember sprintMember in response.SprintMembers)
                DisplaySprintMemberDetails(sprintMember);
        }

        private static void DisplayOverview(AnalyzeSprintResponse response)
        {
            SprintOverviewControl sprintOverviewControl = new()
            {
                Response = response
            };
            sprintOverviewControl.Display();
        }

        private static void DisplayWorkDays(AnalyzeSprintResponse response)
        {
            WorkDaysControl workDaysControl = new()
            {
                WorkDays = response.WorkDays,
                SprintMembers = response.SprintMembers
            };

            workDaysControl.Display();
        }

        private static void DisplaySprintMemberDetails(SprintMember sprintMember)
        {
            SprintMemberDetailsControl sprintMemberDetailsControl = new()
            {
                SprintMember = sprintMember
            };
            sprintMemberDetailsControl.Display();
        }
    }
}