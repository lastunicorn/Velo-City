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

namespace DustInTheWind.VeloCity.Domain
{
    public class VelocityPenalty
    {
        private Sprint sprint;

        public Sprint Sprint
        {
            get => sprint;
            set
            {
                sprint?.VelocityPenalties.Remove(this);
                sprint = value;
                sprint?.VelocityPenalties.Add(this);
            }
        }

        public TeamMember TeamMember { get; set; }

        public int Value { get; set; }

        public int Duration { get; set; }

        public string Comments { get; set; }
    }
}