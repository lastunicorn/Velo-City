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
using System.Linq;
using System.Text;
using DustInTheWind.VeloCity.Application.PresentVelocity;

namespace DustInTheWind.VeloCity.Presentation.Commands.PresentVelocity
{
    public class PresentVelocityView
    {
        public void Display(PresentVelocityResponse response)
        {
            Console.WriteLine("Velocity History:");

            float maxValue = response.SprintVelocities.Max(x => x.Velocity);
            int chartMaxValue = 30;

            foreach (SprintVelocity sprintVelocity in response.SprintVelocities)
            {
                float value = sprintVelocity.Velocity;
                int chartValue = (int)Math.Round(value * chartMaxValue / maxValue);
                
                StringBuilder sb = new();
                sb.Append($"-  {sprintVelocity.SprintNumber} - {sprintVelocity.SprintName} - {sprintVelocity.Velocity:N4} SP/h - ");
                sb.Append(new string('*', chartValue));
                Console.WriteLine(sb);
            }
        }
    }
}