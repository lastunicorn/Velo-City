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

internal class TeamMemberDayAnalyzer
{
    private List<OfficialHolidayInstance> validOfficialHolidays;

    public DateTime Date { get; init; }

    public List<OfficialHolidayInstance> OfficialHolidays { get; init; }

    public Employment Employment { get; init; }

    public List<Vacation> Vacations { get; init; }

    public HoursValue WorkHours { get; private set; }

    public HoursValue AbsenceHours { get; private set; }

    public AbsenceReason AbsenceReason { get; private set; }

    public string AbsenceComments { get; private set; }

    private void Analyze()
    {
        WorkHours = 0;
        AbsenceHours = 0;
        AbsenceReason = AbsenceReason.None;
        AbsenceComments = null;

        bool isEmployed = IsEmployed();
        if (!isEmployed)
        {
            AbsenceReason = AbsenceReason.Unemployed;
            return;
        }

        bool isWeekEnd = IsWeekEnd();
        if (isWeekEnd)
        {
            AbsenceReason = AbsenceReason.WeekEnd;
            return;
        }

        ComputeOfficialHolidays();

        bool hasOfficialHolidays = HasOfficialHolidays();
        if (hasOfficialHolidays)
        {
            AbsenceHours = Employment.HoursPerDay;
            AbsenceReason = AbsenceReason.OfficialHoliday;
            AbsenceComments = CalculateAbsenceComments();
            return;
        }

        bool isWorkDay = IsWorkDay();
        if (!isWorkDay)
        {
            AbsenceReason = AbsenceReason.Contract;
            return;
        }
        
        bool vacationsExist = Vacations.Any();
        if (vacationsExist)
        {
            Vacation[] wholeDayVacations = Vacations
                .Where(x => x.HourCount == null)
                .ToArray();

            bool isWholeDayVacation = wholeDayVacations.Length > 0;
            if (isWholeDayVacation)
            {
                AbsenceHours = Employment.HoursPerDay;
                AbsenceReason = AbsenceReason.Vacation;
                AbsenceComments = CalculateAbsenceComments(wholeDayVacations);

                return;
            }

            int vacationHours = Vacations
                .Where(x => x.HourCount != null)
                .Sum(x => x.HourCount.Value);

            if (vacationHours > Employment.HoursPerDay)
            {
                AbsenceHours = Employment.HoursPerDay;
                AbsenceReason = AbsenceReason.Vacation;
                AbsenceComments = CalculateAbsenceComments(Vacations);

                return;
            }

            WorkHours = Employment.HoursPerDay - vacationHours;
            AbsenceHours = vacationHours;
            AbsenceReason = AbsenceReason.Vacation;
            AbsenceComments = CalculateAbsenceComments(Vacations);

            return;
        }

        WorkHours = Employment.HoursPerDay;
    }

    private bool IsEmployed()
    {
        return Employment != null && Employment.ContainsDate(Date);
    }

    private bool IsWeekEnd()
    {
        return Date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    private void ComputeOfficialHolidays()
    {
        validOfficialHolidays = OfficialHolidays
            .Where(x => string.Equals(x.Country, Employment.Country, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private bool HasOfficialHolidays()
    {
        return validOfficialHolidays.Any();
    }

    private bool IsWorkDay()
    {
        bool isWorkDay = Employment.IsWorkDay(Date.DayOfWeek);
        return isWorkDay;
    }

    private  string CalculateAbsenceComments()
    {
        IEnumerable<string> officialHolidayNames = validOfficialHolidays.Select(x => x.Name);
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
}