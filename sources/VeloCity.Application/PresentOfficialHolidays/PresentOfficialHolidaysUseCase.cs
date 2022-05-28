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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Application.PresentOfficialHolidays
{
    internal class PresentOfficialHolidaysUseCase : IRequestHandler<PresentOfficialHolidaysRequest, PresentOfficialHolidaysResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ISystemClock systemClock;

        public PresentOfficialHolidaysUseCase(IUnitOfWork unitOfWork, ISystemClock systemClock)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.systemClock = systemClock ?? throw new ArgumentNullException(nameof(systemClock));
        }

        public Task<PresentOfficialHolidaysResponse> Handle(PresentOfficialHolidaysRequest request, CancellationToken cancellationToken)
        {
            PresentOfficialHolidaysResponse response = GetOfficialHolidays(request);
            return Task.FromResult(response);
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidays(PresentOfficialHolidaysRequest request)
        {
            if (request.SprintNumber != null)
                return GetOfficialHolidaysBySprint(request.SprintNumber.Value, request.Country);

            if (request.Year != null)
                return GetOfficialHolidaysByYear(request.Year.Value, request.Country);

            return GetOfficialHolidaysForCurrentYear(request.Country);
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysBySprint(int sprintNumber, string country)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            IEnumerable<OfficialHoliday> officialHolidays = country == null
                ? unitOfWork.OfficialHolidayRepository.Get(sprint.StartDate, sprint.EndDate)
                : unitOfWork.OfficialHolidayRepository.Get(sprint.StartDate, sprint.EndDate, country);

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = officialHolidays
                    .SelectMany(x => x.GetInstancesFor(sprint.StartDate, sprint.EndDate))
                    .OrderBy(x => x.Date)
                    .ToList(),
                RequestType = RequestType.BySprint,
                SprintNumber = sprint.Number,
                SprintTimeInterval = sprint.DateInterval
            };
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysByYear(int year, string country)
        {
            IEnumerable<OfficialHoliday> officialHolidays = country == null
                ? unitOfWork.OfficialHolidayRepository.GetByYear(year)
                : unitOfWork.OfficialHolidayRepository.GetByYear(year, country);

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = officialHolidays
                    .Select(x => x.GetInstanceFor(year))
                    .OrderBy(x => x.Date)
                    .ToList(),
                RequestType = RequestType.ByYear,
                Year = year
            };
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysForCurrentYear(string country)
        {
            int currentYear = systemClock.Today.Year;

            IEnumerable<OfficialHoliday> officialHolidays = country == null
                ? unitOfWork.OfficialHolidayRepository.GetByYear(currentYear)
                : unitOfWork.OfficialHolidayRepository.GetByYear(currentYear, country);

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = officialHolidays
                    .Select(x => x.GetInstanceFor(currentYear))
                    .OrderBy(x => x.Date)
                    .ToList(),
                RequestType = RequestType.ByCurrentYear,
                Year = currentYear
            };
        }
    }
}