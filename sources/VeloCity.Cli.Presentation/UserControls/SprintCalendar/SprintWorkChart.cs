// VeloCity
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

using DustInTheWind.VeloCity.ChartTools;

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls.SprintCalendar;

internal class SprintWorkChart : Chart<CalendarItemViewModel>
{
    public SprintWorkChart(IEnumerable<CalendarItemViewModel> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        ActualSize = 24;

        AddRange(items);
        Calculate();
    }

    protected override ChartBarValue<CalendarItemViewModel> ToChartBarValue(CalendarItemViewModel item)
    {
        int workHours = item.WorkHours;
        int absenceHours = item.AbsenceHours;

        ChartBarValue<CalendarItemViewModel> chartBarValue = new()
        {
            MaxValue = workHours + absenceHours,
            FillValue = workHours,
            Item = item
        };

        return chartBarValue;
    }
}