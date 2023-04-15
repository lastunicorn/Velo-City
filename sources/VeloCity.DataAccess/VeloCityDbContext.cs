// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using System.Data;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;
using DustInTheWind.VeloCity.JsonFiles;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess;

public class VeloCityDbContext
{
    private readonly JsonDatabase jsonDatabase;
    private Guid databaseId = Guid.Empty;

    private SprintCollection sprints;
    private TeamMemberCollection teamMembers;
    private List<OfficialHoliday> officialHolidays;

    public DataAccessException LastError => jsonDatabase.LastError;

    public WarningException LastWarning => jsonDatabase.LastWarning;

    public SprintCollection Sprints
    {
        get
        {
            if (sprints == null)
                LoadAllSprints();

            return sprints;
        }
    }

    public TeamMemberCollection TeamMembers
    {
        get
        {
            if (teamMembers == null)
                LoadAllTeamMembers();

            return teamMembers;
        }
    }

    public List<OfficialHoliday> OfficialHolidays
    {
        get
        {
            if (officialHolidays == null)
                LoadAllOfficialHolidays();

            return officialHolidays;
        }
    }

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

    private void LoadAllSprints()
    {
        if (jsonDatabase.State != DatabaseState.Opened)
            throw new DataAccessException("The database is not accessible.");

        sprints = jsonDatabase.Sprints.ToEntities().ToSprintCollection();

        foreach (Sprint sprint in sprints)
        {
            AddHolidays(sprint);
            AddTeamMembers(sprint);
        }
    }

    private void LoadAllTeamMembers()
    {
        if (jsonDatabase.State != DatabaseState.Opened)
            throw new DataAccessException("The database is not accessible.");

        teamMembers = jsonDatabase.TeamMembers.ToEntities(this).ToTeamMemberCollection();
    }

    private void LoadAllOfficialHolidays()
    {
        if (jsonDatabase.State != DatabaseState.Opened)
            throw new DataAccessException("The database is not accessible.");

        officialHolidays = jsonDatabase.OfficialHolidays.ToEntities().ToList();
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

    public Task SaveChanges()
    {
        return Task.Run(() =>
         {
             if (jsonDatabase.InstanceId != databaseId)
                 throw new Exception("The database was changed, current context cannot save the data.");

             VerifySprintIdsAreUnique();
             VerifySTeamMemberIdsAreUnique();

             jsonDatabase.Sprints = Sprints.ToJEntities().ToList();
             jsonDatabase.TeamMembers = TeamMembers.ToJEntities().ToList();
             jsonDatabase.OfficialHolidays = OfficialHolidays.ToJEntities().ToList();

             jsonDatabase.Persist();
         });
    }

    private void VerifySprintIdsAreUnique()
    {
        IEnumerable<int> duplicateIds = Sprints.GetDuplicateIds();
        string idsString = string.Join(", ", duplicateIds);

        if (!string.IsNullOrEmpty(idsString))
            throw new DataException($"There are duplicate Sprint ids in the database context: {idsString}.");
    }

    private void VerifySTeamMemberIdsAreUnique()
    {
        IEnumerable<int> duplicateIds = TeamMembers.GetDuplicateIds();
        string idsString = string.Join(", ", duplicateIds);

        if (!string.IsNullOrEmpty(idsString))
            throw new DataException($"There are duplicate Team Member ids in the database context: {idsString}.");
    }
}