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

using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Domain.TeamMemberModel;

namespace DustInTheWind.VeloCity.Domain.SprintModel;

public class SprintMemberDay
{
    public SprintDay SprintDay { get; }

    public TeamMember TeamMember { get; }

    public HoursValue WorkHours { get; private set; }

    public HoursValue AbsenceHours { get; private set; }

    public AbsenceReason AbsenceReason { get; private set; }

    public string AbsenceComments { get; private set; }

    public bool IsWorkDay => AbsenceReason is AbsenceReason.None or AbsenceReason.Vacation or AbsenceReason.OfficialHoliday;

    public SprintMemberDay(TeamMember teamMember, SprintDay sprintDay)
    {
        TeamMember = teamMember ?? throw new ArgumentNullException(nameof(teamMember));
        SprintDay = sprintDay ?? throw new ArgumentNullException(nameof(sprintDay));

        Analyze();
    }

    private void Analyze()
    {
        WorkHours = 0;
        AbsenceHours = 0;
        AbsenceReason = AbsenceReason.None;
        AbsenceComments = null;

        if (SprintDay == null)
            return;

        Employment employment = TeamMember.Employments?.GetEmploymentFor(SprintDay.Date);

        bool isEmployed = employment != null;
        if (!isEmployed)
        {
            AbsenceReason = AbsenceReason.Unemployed;

            return;
        }

        if (SprintDay.IsWeekEnd)
        {
            AbsenceReason = AbsenceReason.WeekEnd;

            return;
        }

        List<OfficialHolidayInstance> officialHolidays = SprintDay.OfficialHolidays
            .Where(x => string.Equals(x.Country, employment.Country, StringComparison.InvariantCultureIgnoreCase))
            .ToList();

        if (officialHolidays.Any())
        {
            AbsenceHours = employment.HoursPerDay;
            AbsenceReason = AbsenceReason.OfficialHoliday;
            AbsenceComments = CalculateAbsenceComments(officialHolidays);

            return;
        }

        bool isWorkDay = employment.IsWorkDay(SprintDay.Date.DayOfWeek);
        if (!isWorkDay)
        {
            AbsenceReason = AbsenceReason.Contract;

            return;
        }

        Vacation[] vacations = TeamMember.GetVacationsFor(SprintDay.Date)
            .ToArray();

        bool vacationsExist = vacations is { Length: > 0 };

        if (vacationsExist)
        {
            Vacation[] wholeDayVacations = vacations
                .Where(x => x.HourCount == null)
                .ToArray();

            bool isWholeDayVacation = wholeDayVacations.Length > 0;
            if (isWholeDayVacation)
            {
                AbsenceHours = employment.HoursPerDay;
                AbsenceReason = AbsenceReason.Vacation;
                AbsenceComments = CalculateAbsenceComments(wholeDayVacations);

                return;
            }

            int vacationHours = vacations
                .Where(x => x.HourCount != null)
                .Sum(x => x.HourCount.Value);

            if (vacationHours > employment.HoursPerDay)
            {
                AbsenceHours = employment.HoursPerDay;
                AbsenceReason = AbsenceReason.Vacation;
                AbsenceComments = CalculateAbsenceComments(vacations);

                return;
            }

            WorkHours = employment.HoursPerDay - vacationHours;
            AbsenceHours = vacationHours;
            AbsenceReason = AbsenceReason.Vacation;
            AbsenceComments = CalculateAbsenceComments(vacations);

            return;
        }

        WorkHours = employment.HoursPerDay;
    }

    private static string CalculateAbsenceComments(IEnumerable<OfficialHolidayInstance> officialHolidays)
    {
        IEnumerable<string> officialHolidayNames = officialHolidays.Select(x => x.Name);
        return string.Join(", ", officialHolidayNames);
    }

    private static string CalculateAbsenceComments(IEnumerable<Vacation> vacations)
    {
        string[] vacationComments = vacations
            .Select(x => x.Comments)
            .Where(x => x != null)
            .ToArray();

        return vacationComments.Length > 0
            ? string.Join("; ", vacationComments)
            : null;
    }

    public Employment GetEmployment()
    {
        return TeamMember.Employments?.GetEmploymentFor(SprintDay.Date);
    }

    public string GetCountry()
    {
        return GetEmployment()?.Country;
    }

    public override string ToString()
    {
        return $"{SprintDay.Date} - {TeamMember.Name.FullName}";
    }
}