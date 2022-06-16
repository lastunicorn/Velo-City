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
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Cli.Application.PresentVacations;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using FluentAssertions;
using Moq;
using Xunit;

namespace DustInTheWind.VeloCity.Tests.Application.PresentVacations
{
    public class PresentVacationsUseCaseTests
    {
        private readonly PresentVacationsUseCase useCase;
        private readonly Mock<ITeamMemberRepository> teamMemberRepository;

        public PresentVacationsUseCaseTests()
        {
            teamMemberRepository = new();

            Mock<IUnitOfWork> unitOfWork = new();
            unitOfWork
                .Setup(x => x.TeamMemberRepository)
                .Returns(teamMemberRepository.Object);

            useCase = new PresentVacationsUseCase(unitOfWork.Object, Mock.Of<ISystemClock>());
        }

        [Fact]
        public async Task HavingNoTeamMembersInRepository_WhenUseCaseIsExecuted_ThenReturnsEmptyList()
        {
            PresentVacationsRequest request = new();
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.TeamMemberVacations.Should().BeEmpty();
        }

        [Fact]
        public async Task HavingOneTeamMemberWithNoVacationsInRepository_WhenUseCaseIsExecuted_ThenReturnsOneEmptyItem()
        {
            TeamMember teamMember = new();
            teamMemberRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>()))
                .Returns(new[] { teamMember });

            PresentVacationsRequest request = new();
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.TeamMemberVacations.Should().HaveCount(1);
            response.TeamMemberVacations[0].Vacations.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task HavingOneTeamMemberWithOneVacationInRepository_WhenUseCaseIsExecuted_ThenReturnsOneItemWithOneVacation()
        {
            TeamMember teamMember = new()
            {
                Vacations = new VacationCollection
                {
                    new VacationOnce
                    {
                        Date = new DateTime(2020, 05, 28)
                    }
                }
            };
            teamMemberRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>()))
                .Returns(new[] { teamMember });

            PresentVacationsRequest request = new();
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.TeamMemberVacations.Should().HaveCount(1);
            response.TeamMemberVacations[0].Vacations[0].Should().BeSameAs(teamMember.Vacations[0]);
        }

        [Fact]
        public async Task HavingOneTeamMemberWithTwoVacationsInRepository_WhenUseCaseIsExecuted_ThenReturnsOneItemWithThoseTwoVacations()
        {
            TeamMember teamMember = new()
            {
                Vacations = new VacationCollection
                {
                    new VacationOnce
                    {
                        Date = new DateTime(2020, 05, 28)
                    },
                    new VacationDaily
                    {
                        DateInterval = new DateInterval(new DateTime(2020, 05, 28), new DateTime(2020, 05, 30))
                    }
                }
            };
            teamMemberRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>()))
                .Returns(new[] { teamMember });

            PresentVacationsRequest request = new();
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.TeamMemberVacations.Should().HaveCount(1);
            response.TeamMemberVacations[0].Vacations.Should().BeEquivalentTo(teamMember.Vacations);
        }

        [Fact]
        public async Task HavingRequestWithSpecificName_WhenUseCaseIsExecuted_ThenTeamMembersWithThatNameAreSearchedInRepository()
        {
            TeamMember teamMember = new()
            {
                Vacations = new VacationCollection
                {
                    new VacationOnce
                    {
                        Date = new DateTime(2020, 05, 28)
                    }
                }
            };
            teamMemberRepository
                .Setup(x => x.Find("team member name"))
                .Returns(new[] { teamMember });

            PresentVacationsRequest request = new()
            {
                TeamMemberName = "team member name"
            };
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            teamMemberRepository.Verify(x => x.GetByDate(It.IsAny<DateTime>()), Times.Never);
            teamMemberRepository.Verify(x => x.Find("team member name"), Times.Once);
        }

        [Fact]
        public async Task HavingOneTeamMemberAndRequestWithSpecificName_WhenUseCaseIsExecuted_ThenReturnsOneItem()
        {
            TeamMember teamMember = new();
            teamMemberRepository
                .Setup(x => x.Find("team member name"))
                .Returns(new[] { teamMember });

            PresentVacationsRequest request = new()
            {
                TeamMemberName = "team member name"
            };
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.TeamMemberVacations.Should().HaveCount(1);
        }

        [Fact]
        public async Task HavingTwoTeamMembersWithDifferentNamesOutOfOrder_WhenUseCaseIsExecuted_ThenTheTwoItemsAreReturnedSortedByName()
        {
            TeamMember teamMember1 = new() { Name = PersonName.Parse("bbb") };
            TeamMember teamMember2 = new() { Name = PersonName.Parse("aaa") };
            teamMemberRepository
                .Setup(x => x.GetByDate(It.IsAny<DateTime>()))
                .Returns(new[] { teamMember1, teamMember2 });

            PresentVacationsRequest request = new();
            PresentVacationsResponse response = await useCase.Handle(request, CancellationToken.None);

            response.TeamMemberVacations.Should().HaveCount(2);
            response.TeamMemberVacations.Should().BeInAscendingOrder(x => x.PersonName);
        }
    }
}