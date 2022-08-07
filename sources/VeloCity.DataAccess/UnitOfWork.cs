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
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VeloCityDbContext dbContext;
        private OfficialHolidayRepository officialHolidayRepository;
        private SprintRepository sprintRepository;
        private TeamMemberRepository teamMemberRepository;

        private VeloCityDbContext DbContext
        {
            get
            {
                if (dbContext.State != DatabaseState.Opened)
                    dbContext.Open();

                return dbContext;
            }
        }

        public Exception DatabaseError => dbContext.LastError;

        public WarningException DatabaseWarning => dbContext.LastWarning;

        public IOfficialHolidayRepository OfficialHolidayRepository => officialHolidayRepository ??= new OfficialHolidayRepository(DbContext);

        public ISprintRepository SprintRepository => sprintRepository ??= new SprintRepository(DbContext);

        public ITeamMemberRepository TeamMemberRepository => teamMemberRepository ??= new TeamMemberRepository(DbContext);

        public UnitOfWork(VeloCityDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}