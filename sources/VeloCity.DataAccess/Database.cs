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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal static class Database
    {
        #region TeamMembers

        public static readonly List<TeamMember> TeamMembers = new()
        {
            new TeamMember
            {
                Id = 1,
                Name = "Florin Flestea",
                HoursPerDay = 2
            },
            new TeamMember
            {
                Id = 2,
                Name = "Alexandru Iuga"
            },
            new TeamMember
            {
                Id = 3,
                Name = "Raluca Danila"
            },
            new TeamMember
            {
                Id = 4,
                Name = "Flaviu Comanese"
            },
            new TeamMember
            {
                Id = 5,
                Name = "Virgil Radu"
            },
            new TeamMember
            {
                Id = 6,
                Name = "Oana Faur"
            }
        };

        #endregion

        #region OfficialFreeDays

        public static readonly List<OfficialFreeDay> OfficialFreeDays = new()
        {
            new OfficialFreeDay
            {
                Date = new DateTime(2021, 11, 30),
                Name = "Sfântul Andrei"
            },
            new OfficialFreeDay
            {
                Date = new DateTime(2021, 12, 1),
                Name = "Ziua națională a României"
            },
            new OfficialFreeDay
            {
                Date = new DateTime(2021, 01, 24),
                Name = "Ziua unirii principatelor"
            },
        };

        #endregion

        #region Vacations

        public static List<Vacation> Vacations = new()
        {
            #region Florin Flestea

            // Florin Flestea (November 2021)

            new Vacation
            {
                Date = new DateTime(2021, 11, 02),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 11, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },

            // Florin Flestea (December 2021)

            new Vacation
            {
                Date = new DateTime(2021, 12, 02),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 03),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 27),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 30),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },

            // Florin Flestea (January 2022)

            new Vacation
            {
                Date = new DateTime(2022, 01, 03),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 04),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 05),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 06),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 07),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },

            // Florin Flestea (February 2022)

            new Vacation
            {
                Date = new DateTime(2022, 02, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },

            // Florin Flestea (March 2022)

            new Vacation
            {
                Date = new DateTime(2022, 03, 01),
                TeamMember = TeamMembers.Single(x => x.Id == 1)
            },

            #endregion

            #region Alexandru Iuga

            // Alexandru Iuga (November 2021)

            new Vacation
            {
                Date = new DateTime(2021, 11, 08),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 11, 15),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 11, 19),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 11, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },

            // Alexandru Iuga (December 2021)

            new Vacation
            {
                Date = new DateTime(2021, 12, 2),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 3),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 6),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 7),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 8),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 9),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 10),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 13),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 20),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 27),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 30),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },

            // Alexandru Iuga (January 2022)

            new Vacation
            {
                Date = new DateTime(2022, 01, 03),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 10),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 17),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },

            // Alexandru Iuga (February 2022)

            new Vacation
            {
                Date = new DateTime(2022, 02, 07),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 02, 14),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 02, 21),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 02, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },

            // Alexandru Iuga (March 2022)

            new Vacation
            {
                Date = new DateTime(2022, 03, 07),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 03, 14),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 03, 21),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },
            new Vacation
            {
                Date = new DateTime(2022, 03, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 2)
            },

            #endregion

            #region Raluca Danila

            // Raluca Danila (October 2021)

            new Vacation
            {
                Date = new DateTime(2021, 10, 20),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 10, 21),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 10, 22),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },

            // Raluca Danila (November 2021)

            new Vacation
            {
                Date = new DateTime(2021, 11, 11),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 11, 12),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 11, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },

            // Raluca Danila (December 2021)

            new Vacation
            {
                Date = new DateTime(2021, 12, 22),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 23),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 24),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 27),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 30),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 3)
            },

            #endregion

            #region Flaviu Comanese

            // Flaviu Comanese (November 2021)

            new Vacation
            {
                Date = new DateTime(2021, 11, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },

            // Flaviu Comanese (December 2021)

            new Vacation
            {
                Date = new DateTime(2021, 12, 13),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 14),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 15),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 16),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 17),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 24),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 30),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },

            // Flaviu Comanese (January 2022)

            new Vacation
            {
                Date = new DateTime(2022, 01, 03),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },

            // Flaviu Comanese (February 2022)

            new Vacation
            {
                Date = new DateTime(2022, 02, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 4)
            },

            #endregion

            #region Virgil Radu

            // Virgil Radu (November 2021)

            new Vacation
            {
                Date = new DateTime(2021, 11, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },

            // Virgil Radu (December 2021)

            new Vacation
            {
                Date = new DateTime(2021, 12, 24),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 27),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 28),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 30),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },

            // Virgil Radu (January 2022)

            new Vacation
            {
                Date = new DateTime(2022, 01, 03),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 04),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 25),
                TeamMember = TeamMembers.Single(x => x.Id == 5)
            },

            #endregion

            #region Oana Faur

            // Oana Faur (December 2021)

            new Vacation
            {
                Date = new DateTime(2021, 12, 22),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 23),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 24),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 29),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 30),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2021, 12, 31),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },

            // Oana Faur (January 2022)

            new Vacation
            {
                Date = new DateTime(2022, 01, 03),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 04),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },
            new Vacation
            {
                Date = new DateTime(2022, 01, 25),
                TeamMember = TeamMembers.Single(x => x.Id == 6)
            },

            #endregion
        };

        #endregion

        #region Sprints

        public static readonly List<Sprint> Sprints = new()
        {
            new Sprint
            {
                Id = 18,
                Name = "Sprint 18",
                StartDate = new DateTime(2021, 11, 26),
                EndDate = new DateTime(2021, 12, 21),
                StoryPoints = 30
            },
            new Sprint
            {
                Id = 19,
                Name = "Sprint 19",
                StartDate = new DateTime(2021, 12, 22),
                EndDate = new DateTime(2022, 01, 11),
                StoryPoints = 7
            },
            new Sprint
            {
                Id = 20,
                Name = "Sprint 20",
                StartDate = new DateTime(2022, 01, 12),
                EndDate = new DateTime(2022, 01, 24),
                StoryPoints = 21
            },
            new Sprint
            {
                Id = 21,
                Name = "Sprint 21",
                StartDate = new DateTime(2022, 01, 25),
                EndDate = new DateTime(2022, 02, 06),
                StoryPoints = 13
            },
            new Sprint
            {
                Id = 22,
                Name = "Sprint 22",
                StartDate = new DateTime(2022, 02, 07),
                EndDate = new DateTime(2022, 02, 15),
                StoryPoints = 25
            },
            new Sprint
            {
                Id = 23,
                Name = "Sprint 23",
                StartDate = new DateTime(2022, 02, 16),
                EndDate = new DateTime(2022, 02, 22),
                StoryPoints = 23
            },
            new Sprint
            {
                Id = 24,
                Name = "Sprint 24",
                StartDate = new DateTime(2022, 02, 23),
                EndDate = new DateTime(2022, 03, 08),
                StoryPoints = 0
            }
        };

        #endregion

        static Database()
        {
            foreach (Sprint sprint in Sprints)
            {
                sprint.OfficialFreeDays = OfficialFreeDays
                    .Where(x => x.Date >= sprint.StartDate && x.Date <= sprint.EndDate)
                    .ToList();
            }
        }
    }
}