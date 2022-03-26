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
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Database database;

        private OfficialHolidayRepository officialHolidayRepository;
        private SprintRepository sprintRepository;
        private TeamMemberRepository teamMemberRepository;

        public IOfficialHolidayRepository OfficialHolidayRepository => officialHolidayRepository ??= new OfficialHolidayRepository(database);

        public ISprintRepository SprintRepository => sprintRepository ??= new SprintRepository(database);

        public ITeamMemberRepository TeamMemberRepository => teamMemberRepository ??= new TeamMemberRepository(database);

        public UnitOfWork(Database database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
    }
}