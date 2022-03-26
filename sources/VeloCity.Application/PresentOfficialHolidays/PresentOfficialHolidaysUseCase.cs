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

        public PresentOfficialHolidaysUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentOfficialHolidaysResponse> Handle(PresentOfficialHolidaysRequest request, CancellationToken cancellationToken)
        {
            PresentOfficialHolidaysResponse response = GetOfficialHolidays(request);
            return Task.FromResult(response);
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidays(PresentOfficialHolidaysRequest request)
        {
            if (request.SprintNumber != null)
                return GetOfficialHolidaysBySprint(request.SprintNumber.Value);

            if (request.Year != null)
                return GetOfficialHolidaysByYear(request.Year.Value);

            return GetOfficialHolidaysForCurrentYear();
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysBySprint(int sprintNumber)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = unitOfWork.OfficialHolidayRepository.Get(sprint.StartDate, sprint.EndDate).ToList(),
                RequestType = RequestType.BySprint,
                SprintNumber = sprint.Number
            };
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysByYear(int year)
        {
            return new PresentOfficialHolidaysResponse()
            {
                OfficialHolidays = unitOfWork.OfficialHolidayRepository.GetByYear(year).ToList(),
                RequestType = RequestType.ByYear,
                Year = year
            };
        }

        private PresentOfficialHolidaysResponse GetOfficialHolidaysForCurrentYear()
        {
            int currentYear = DateTime.Now.Year;

            return new PresentOfficialHolidaysResponse
            {
                OfficialHolidays = unitOfWork.OfficialHolidayRepository.GetByYear(currentYear).ToList(),
                RequestType = RequestType.ByCurrentYear,
                Year = currentYear
            };
        }
    }
}