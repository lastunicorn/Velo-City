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
using DustInTheWind.VeloCity.Application.PresentSprints;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentSprints
{
    public class PresentSprintsView
    {
        public void Display(PresentSprintsResponse response)
        {
            foreach (SprintOverview sprintOverview in response.SprintOverviews)
            {
                Console.WriteLine();
                
                Console.WriteLine(new string('=', 79));
                Console.WriteLine($"{sprintOverview.Name} ({sprintOverview.StartDate:d} - {sprintOverview.EndDate:d})");
                Console.WriteLine(new string('=', 79));
                
                Console.WriteLine();
                
                Console.WriteLine($"Total Work Hours: {sprintOverview.TotalWorkHours} h");
                
                Console.WriteLine();
                
                Console.WriteLine($"Estimated Story Points: {sprintOverview.EstimatedStoryPoints} SP");
                Console.WriteLine($"Estimated Velocity: {sprintOverview.EstimatedVelocity} SP/h");
                Console.WriteLine($"Commitment Story Points: {sprintOverview.CommitmentStoryPoints} SP");
                
                Console.WriteLine();
                
                Console.WriteLine($"Actual Story Points: {sprintOverview.ActualStoryPoints} SP");
                Console.WriteLine($"Actual Velocity: {sprintOverview.ActualVelocity} SP/h");

            }
        }
    }
}