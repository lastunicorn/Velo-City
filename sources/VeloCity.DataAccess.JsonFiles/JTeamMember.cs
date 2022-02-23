using System.Collections.Generic;

namespace DustInTheWind.VeloCity.JsonFiles
{
    public class JTeamMember
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HoursPerDay { get; set; }

        public List<JVacationDay> VacationDays { get; set; }
    }
}