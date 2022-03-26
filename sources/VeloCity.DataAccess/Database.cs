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
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess
{
    public class Database
    {
        private readonly DatabaseFile databaseFile;
        public readonly List<TeamMember> TeamMembers = new();
        public readonly List<OfficialHoliday> OfficialHolidays = new();
        public List<Vacation> Vacations = new();
        public readonly List<Sprint> Sprints = new();

        public Database(DatabaseFile databaseFile)
        {
            this.databaseFile = databaseFile ?? throw new ArgumentNullException(nameof(databaseFile));

            LoadAll();

            foreach (Sprint sprint in Sprints)
            {
                sprint.OfficialHolidays = OfficialHolidays
                    .Where(x => x.Date >= sprint.StartDate && x.Date <= sprint.EndDate)
                    .ToList();
            }
        }

        private void LoadAll()
        {
            TeamMembers.Clear();
            OfficialHolidays.Clear();
            Vacations.Clear();
            Sprints.Clear();

            databaseFile.Open();

            IEnumerable<Sprint> sprints = databaseFile.Document.Sprints.ToEntities();
            Sprints.AddRange(sprints);

            IEnumerable<TeamMember> teamMembers = databaseFile.Document.TeamMembers.ToEntities(this);
            TeamMembers.AddRange(teamMembers);

            IEnumerable<OfficialHoliday> officialHolidays = databaseFile.Document.OfficialHolidays.ToEntities();
            OfficialHolidays.AddRange(officialHolidays);
        }

        private void SaveAll()
        {
            DatabaseDocument databaseDocument = new()
            {
                Sprints = Sprints.ToJEntities().ToList(),
                TeamMembers = TeamMembers
                    .Select(x =>
                    {
                        x.Vacations = Vacations
                            .Where(z => z.TeamMember == x)
                            .ToList();

                        return x;
                    })
                    .ToJEntities()
                    .ToList(),
                OfficialHolidays = OfficialHolidays
                    .ToJEntities()
                    .ToList()
            };

            DatabaseFile databaseFile = new(string.Empty);
            databaseFile.Document = databaseDocument;
            databaseFile.Save();
        }
    }
}