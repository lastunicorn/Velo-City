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

using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Tests.Domain.TeamMemberModel.TeamMemberTests
{
    public class ConstructorTests
    {
        private readonly TeamMember teamMember;

        public ConstructorTests()
        {
            teamMember = new TeamMember();
        }

        [Fact]
        public void WhenNewInstanceIsCreated_ThenIdIsZero()
        {
            teamMember.Id.Should().Be(0);
        }

        [Fact]
        public void WhenNewInstanceIsCreated_ThenNameIsEmpty()
        {
            teamMember.Name.FirstName.Should().BeNull();
            teamMember.Name.MiddleName.Should().BeNull();
            teamMember.Name.LastName.Should().BeNull();
            teamMember.Name.Nickname.Should().BeNull();
        }

        [Fact]
        public void WhenNewInstanceIsCreated_ThenEmploymentsIsNull()
        {
            teamMember.Employments.Should().BeNull();
        }

        [Fact]
        public void WhenNewInstanceIsCreated_ThenCommentsIsNull()
        {
            teamMember.Comments.Should().BeNull();
        }
    }
}