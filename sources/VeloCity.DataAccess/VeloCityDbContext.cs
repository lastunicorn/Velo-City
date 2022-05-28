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
using DustInTheWind.VeloCity.Domain.Configuring;
using DustInTheWind.VeloCity.Domain.DataAccess;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    public class VeloCityDbContext
    {
        private readonly IConfig config;
        private DatabaseFile databaseFile;

        public DatabaseState State { get; private set; }

        public DataAccessException LastError { get; private set; }

        public WarningException LastWarning { get; private set; }

        public List<TeamMember> TeamMembers { get; } = new();

        public List<OfficialHoliday> OfficialHolidays { get; } = new();

        public List<Vacation> Vacations { get; } = new();

        public List<Sprint> Sprints { get; } = new();

        public VeloCityDbContext(IConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void Open()
        {
            if (State == DatabaseState.Opened)
                return;

            try
            {
                CloseInternal();

                databaseFile = new DatabaseFile(config.DatabaseLocation);
                databaseFile.Open();

                LoadAllData();
                PopulateSprints();

                State = DatabaseState.Opened;
            }
            catch (DataAccessException ex)
            {
                LastError = ex;
                State = DatabaseState.Error;
                throw;
            }
            catch (Exception ex)
            {
                DataAccessException dataAccessException = new(ex);
                LastError = dataAccessException;
                State = DatabaseState.Error;

                throw dataAccessException;
            }
            finally
            {
                LastWarning = databaseFile.LastWarning;
            }
        }

        private void CloseInternal()
        {
            TeamMembers.Clear();
            OfficialHolidays.Clear();
            Vacations.Clear();
            Sprints.Clear();

            LastError = null;
            LastWarning = null;

            State = DatabaseState.Closed;
        }

        private void LoadAllData()
        {
            IEnumerable<Sprint> sprints = databaseFile.Document.Sprints.ToEntities();
            Sprints.AddRange(sprints);

            IEnumerable<TeamMember> teamMembers = databaseFile.Document.TeamMembers.ToEntities(this);
            TeamMembers.AddRange(teamMembers);

            IEnumerable<OfficialHoliday> officialHolidays = databaseFile.Document.OfficialHolidays.ToEntities();
            OfficialHolidays.AddRange(officialHolidays);
        }

        private void PopulateSprints()
        {
            foreach (Sprint sprint in Sprints)
            {
                AddHolidays(sprint);
                AddTeamMembers(sprint);
            }
        }

        private void AddHolidays(Sprint sprint)
        {
            List<OfficialHoliday> officialHolidays = OfficialHolidays
                .Where(x => x.Match(sprint.StartDate, sprint.EndDate))
                .ToList();

            sprint.OfficialHolidays.AddRange(officialHolidays);
        }

        private void AddTeamMembers(Sprint sprint)
        {
            IEnumerable<TeamMember> teamMembers = TeamMembers
                .Where(x => x.Employments?.Any(e => e.TimeInterval.IsIntersecting(sprint.DateInterval)) ?? false);

            foreach (TeamMember teamMember in teamMembers)
                sprint.AddSprintMember(teamMember);
        }
    }
}