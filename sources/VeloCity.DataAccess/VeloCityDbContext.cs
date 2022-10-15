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

using System;
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess
{
    public class VeloCityDbContext
    {
        private readonly JsonDatabase jsonDatabase;
        private Guid databaseId = Guid.Empty;

        private List<Sprint> sprints;
        private List<TeamMember> teamMembers;
        private List<OfficialHoliday> officialHolidays;

        public DataAccessException LastError => jsonDatabase.LastError;

        public WarningException LastWarning => jsonDatabase.LastWarning;

        public List<Sprint> Sprints => sprints ??= LoadAllSprints();

        public List<TeamMember> TeamMembers => teamMembers ??= LoadAllTeamMembers();

        public List<OfficialHoliday> OfficialHolidays => officialHolidays ??= LoadAllOfficialHolidays();

        public VeloCityDbContext(JsonDatabase jsonDatabase)
        {
            this.jsonDatabase = jsonDatabase ?? throw new ArgumentNullException(nameof(jsonDatabase));

            jsonDatabase.Opened += HandleJsonDatabaseOpened;

            if (jsonDatabase.State == DatabaseState.Opened)
                databaseId = jsonDatabase.InstanceId;
        }

        private void HandleJsonDatabaseOpened(object sender, EventArgs e)
        {
            if (databaseId == Guid.Empty && sender is JsonDatabase jsonDatabase)
                databaseId = jsonDatabase.InstanceId;
        }

        private List<Sprint> LoadAllSprints()
        {
            if (jsonDatabase.State != DatabaseState.Opened)
                throw new DataAccessException("The database is not accessible.");

            IEnumerable<Sprint> databaseSprints = jsonDatabase.Sprints.ToEntities();
            List<Sprint> fullyLoadedSprints = new();

            foreach (Sprint sprint in databaseSprints)
            {
                AddHolidays(sprint);
                AddTeamMembers(sprint);

                fullyLoadedSprints.Add(sprint);
            }

            return fullyLoadedSprints;
        }

        private List<TeamMember> LoadAllTeamMembers()
        {
            if (jsonDatabase.State != DatabaseState.Opened)
                throw new DataAccessException("The database is not accessible.");

            return jsonDatabase.TeamMembers.ToEntities(this).ToList();
        }

        private List<OfficialHoliday> LoadAllOfficialHolidays()
        {
            if (jsonDatabase.State != DatabaseState.Opened)
                throw new DataAccessException("The database is not accessible.");

            return jsonDatabase.OfficialHolidays.ToEntities().ToList();
        }

        private void AddHolidays(Sprint sprint)
        {
            List<OfficialHoliday> matchedOfficialHolidays = OfficialHolidays
                .Where(x => x.Match(sprint.StartDate, sprint.EndDate))
                .ToList();

            sprint.OfficialHolidays.AddRange(matchedOfficialHolidays);
        }

        private void AddTeamMembers(Sprint sprint)
        {
            IEnumerable<TeamMember> matchedTeamMembers = TeamMembers
                .Where(x => x.Employments?.Any(e => e.TimeInterval.IsIntersecting(sprint.DateInterval)) ?? false);

            foreach (TeamMember teamMember in matchedTeamMembers)
                sprint.AddSprintMember(teamMember);
        }

        public void SaveChanges()
        {
            if (jsonDatabase.InstanceId != databaseId)
                throw new Exception("The database was change, current context cannot save the data.");

            jsonDatabase.Sprints = Sprints.ToJEntities().ToList();
            jsonDatabase.TeamMembers = TeamMembers.ToJEntities().ToList();
            jsonDatabase.OfficialHolidays = OfficialHolidays.ToJEntities().ToList();

            jsonDatabase.Persist();
        }
    }
}