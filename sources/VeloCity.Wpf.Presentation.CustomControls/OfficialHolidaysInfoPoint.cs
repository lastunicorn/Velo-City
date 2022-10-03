using System.Collections.Generic;
using System.Windows;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls
{
    public class OfficialHolidaysInfoPoint : InfoPoint
    {
        public static readonly DependencyProperty OfficialHolidaysProperty = DependencyProperty.Register(
            nameof(OfficialHolidays),
            typeof(List<OfficialHoliday>),
            typeof(OfficialHolidaysInfoPoint)
        );

        public List<OfficialHoliday> OfficialHolidays
        {
            get => (List<OfficialHoliday>)GetValue(OfficialHolidaysProperty);
            set => SetValue(OfficialHolidaysProperty, value);
        }

        static OfficialHolidaysInfoPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OfficialHolidaysInfoPoint), new FrameworkPropertyMetadata(typeof(OfficialHolidaysInfoPoint)));
        }
    }

    public class OfficialHoliday
    {
        public string HolidayName { get; set; }

        public string HolidayCountry { get; set; }
    }
}