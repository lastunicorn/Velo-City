﻿// Velo City
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
using DustInTheWind.VeloCity.Application.PresentOfficialHolidays;

namespace DustInTheWind.VeloCity.Presentation.Commands.Holidays
{
    public class RequestTypeViewModel
    {
        private readonly PresentOfficialHolidaysResponse response;

        public RequestTypeViewModel(PresentOfficialHolidaysResponse response)
        {
            this.response = response ?? throw new ArgumentNullException(nameof(response));
        }

        public override string ToString()
        {
            return response.RequestType switch
            {
                RequestType.BySprint => $"The official holidays for the sprint {response.SprintNumber}:",
                RequestType.ByYear => $"The official holidays for the year {response.Year}:",
                RequestType.ByCurrentYear => $"The official holidays for the current year {response.Year}:",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}