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
using System.Collections.Generic;
using System.Linq;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;

namespace DustInTheWind.VeloCity.DataAccess
{
    internal class OfficialHolidayRepository : IOfficialHolidayRepository
    {
        private readonly Database database;

        public OfficialHolidayRepository(Database database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public IEnumerable<OfficialHoliday> GetAll()
        {
            return database.OfficialHolidays;
        }

        public IEnumerable<OfficialHoliday> GetAll(DateTime startDate, DateTime endDate)
        {
            return database.OfficialHolidays
                .Where(x => x.Date >= startDate && x.Date <= endDate);
        }
    }
}