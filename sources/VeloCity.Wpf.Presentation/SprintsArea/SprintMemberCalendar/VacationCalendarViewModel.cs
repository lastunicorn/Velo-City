using System.Collections.ObjectModel;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar;

public class VacationCalendarViewModel
{
    public ObservableCollection<VacationCalendarMonthViewModel> Months { get; } = new();

    public void Add(VacationCalendarDayViewModel day)
    {
        VacationCalendarMonthViewModel month = GetOrCreateMonth(day.Date);
        month.AddDay(day);
    }

    private VacationCalendarMonthViewModel GetOrCreateMonth(DateTime date)
    {
        for (int i = 0; i < Months.Count; i++)
        {
            VacationCalendarMonthViewModel month = Months[i];

            int result = month.Contains(date);

            switch (result)
            {
                case < 0:
                    return CreateMonth(date, i);

                case 0:
                    return month;
            }
        }

        return CreateMonth(date);
    }

    private VacationCalendarMonthViewModel CreateMonth(DateTime date, int i)
    {
        VacationCalendarMonthViewModel newMonth = new(date.Year, date.Month);
        Months.Insert(i, newMonth);
        return newMonth;
    }

    private VacationCalendarMonthViewModel CreateMonth(DateTime date)
    {
        VacationCalendarMonthViewModel newMonth = new(date.Year, date.Month);
        Months.Add(newMonth);
        return newMonth;
    }
}