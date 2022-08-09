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
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Pages.General
{
    internal class DateIntervalViewModel
    {
        private readonly DateTime? startDate;
        private readonly DateTime? endDate;

        public DateIntervalViewModel(DateInterval dateInterval)
        {
            startDate = dateInterval.StartDate;
            endDate = dateInterval.EndDate;
        }

        public DateIntervalViewModel(DateTime? startDate, DateTime? endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public override string ToString()
        {
            string startDateString = startDate?.ToString("d") ?? "<<<";
            string endDateString = endDate?.ToString("d") ?? ">>>";
            return $"{startDateString} - {endDateString}";
        }
    }
}