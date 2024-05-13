using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintMemberCalendar;

public class VacationCalendarDayViewModel : ViewModelBase
{
    private HoursValue? workHours;

    public DateTime Date { get; init; }

    public bool IsCurrentDay { get; init; }

    public bool IsWorkDay { get; init; }

    public HoursValue? WorkHours
    {
        get => workHours;
        set
        {
            if (Nullable.Equals(value, workHours)) return;
            workHours = value;
            OnPropertyChanged();
        }
    }
}