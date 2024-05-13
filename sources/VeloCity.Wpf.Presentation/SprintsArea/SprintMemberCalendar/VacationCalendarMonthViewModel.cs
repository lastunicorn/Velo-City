using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar;

public class VacationCalendarMonthViewModel : ViewModelBase
{
    private readonly int year;
    private readonly int month;
    private string title;

    public string Title
    {
        get => title;
        set
        {
            if (value == title) return;
            title = value;
            OnPropertyChanged();
        }
    }

    public List<VacationCalendarDayViewModel> Days { get; } = new();

    public VacationCalendarMonthViewModel(int year, int month)
    {
        this.year = year;
        this.month = month;
    }

    public int Contains(DateTime date)
    {
        if (date.Year < year) return -1;
        if (date.Year > year) return 1;

        if (date.Month < month) return -1;
        if (date.Month > month) return 1;

        return 0;
    }

    public void AddDay(VacationCalendarDayViewModel day)
    {
        Days.Add(day);
    }
}