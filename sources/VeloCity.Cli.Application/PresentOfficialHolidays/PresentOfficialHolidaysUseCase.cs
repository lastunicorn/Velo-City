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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.OfficialHolidayModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SystemAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentOfficialHolidays
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
            DateInterval sprintDateInterval = RetrieveSprintTimeLapse(sprintNumber);

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = GetOfficialHolidayInstancesForDateInterval(sprintDateInterval, country),
                RequestType = RequestType.BySprint,
                SprintNumber = sprintNumber,
                SprintDateInterval = sprintDateInterval
            };
        }

        private DateInterval RetrieveSprintTimeLapse(int sprintNumber)
        {
            DateInterval? sprintDateInterval = unitOfWork.SprintRepository.GetDateIntervalFor(sprintNumber);

            if (sprintDateInterval == null)
                throw new SprintDoesNotExistException(sprintNumber);

            if (sprintDateInterval.Value.IsHalfInfinite)
                throw new InvalidDateIntervalException(sprintNumber);

            return sprintDateInterval.Value;
        }

        private List<OfficialHolidayInstance> GetOfficialHolidayInstancesForDateInterval(DateInterval dateInterval, string country)
        {
            DateTime startDate = dateInterval.StartDate!.Value;
            DateTime endDate = dateInterval.EndDate!.Value;

            IEnumerable<OfficialHoliday> officialHolidays = country == null
                ? unitOfWork.OfficialHolidayRepository.Get(startDate, endDate)
                : unitOfWork.OfficialHolidayRepository.Get(startDate, endDate, country);

            return officialHolidays
                .SelectMany(x => x.GetInstancesFor(startDate, endDate))
                .OrderBy(x => x.Date)
                .ToList();
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysByYear(int year, string country)
        {
            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = GetOfficialHolidayInstancesForYear(year, country),
                RequestType = RequestType.ByYear,
                Year = year
            };
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysForCurrentYear(string country)
        {
            int currentYear = systemClock.Today.Year;

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = GetOfficialHolidayInstancesForYear(currentYear, country),
                RequestType = RequestType.ByCurrentYear,
                Year = currentYear
            };
        }

        private List<OfficialHolidayInstance> GetOfficialHolidayInstancesForYear(int year, string country)
        {
            IEnumerable<OfficialHoliday> officialHolidays = country == null
                ? unitOfWork.OfficialHolidayRepository.GetByYear(year)
                : unitOfWork.OfficialHolidayRepository.GetByYear(year, country);

            return officialHolidays
                .Select(x => x.GetInstanceFor(year))
                .OrderBy(x => x.Date)
                .ToList();
        }
    }
}