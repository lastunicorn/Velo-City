using System.Collections.Generic;

namespace VeloCity.DataAccess.Jsonfiles
{
    public class DatabaseDocument
    {
        public List<JSprint> Sprints { get; set; }

        public List<JTeamMember> TeamMembers { get; set; }

        public List<JOfficialHoliday> OfficialHolidays { get; set; }
    }
}