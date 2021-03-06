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

using System.Collections.Generic;

namespace DustInTheWind.VeloCity.Domain
{
    public class TeamMember
    {
        private List<VelocityPenalty> velocityPenalties;

        public int Id { get; set; }

        public PersonName Name { get; set; }

        public EmploymentCollection Employments { get; set; }

        public string Comments { get; set; }

        public VacationCollection Vacations { get; set; }

        public List<VelocityPenalty> VelocityPenalties
        {
            get => velocityPenalties;
            set
            {
                if (velocityPenalties != null)
                {
                    foreach (VelocityPenalty velocityPenalty in velocityPenalties)
                        velocityPenalty.TeamMember = null;
                }

                velocityPenalties = value;

                if (velocityPenalties != null)
                {
                    foreach (VelocityPenalty velocityPenalty in velocityPenalties)
                        velocityPenalty.TeamMember = this;
                }
            }
        }

        public SprintMember ToSprintMember(Sprint sprint)
        {
            return new SprintMember(this, sprint);
        }

        public override string ToString()
        {
            return Name.FullName;
        }
    }
}