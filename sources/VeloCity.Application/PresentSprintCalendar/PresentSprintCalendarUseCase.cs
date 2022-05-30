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

namespace DustInTheWind.VeloCity.Application.PresentSprintCalendar
{
    internal class PresentSprintCalendarUseCase : IRequestHandler<PresentSprintCalendarRequest, PresentSprintCalendarResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentSprintCalendarUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentSprintCalendarResponse> Handle(PresentSprintCalendarRequest request, CancellationToken cancellationToken)
        {
            PresentSprintCalendarResponse response = CreateResponse(request);
            return Task.FromResult(response);
        }

        private PresentSprintCalendarResponse CreateResponse(PresentSprintCalendarRequest request)
        {
            if (request.SprintNumber != null)
                return CreateSprintCalendarResponse(request.SprintNumber.Value);

            if (request.StartDate != null)
                return CreateMonthCalendarResponse(request.StartDate.Value, request.EndDate);

            return CreateSprintCalendarResponse();
        }

        private PresentSprintCalendarResponse CreateSprintCalendarResponse(int sprintNumber)
        {
            Sprint sprint = unitOfWork.SprintRepository.GetByNumber(sprintNumber);

            if (sprint == null)
                throw new SprintDoesNotExistException(sprintNumber);

            return new PresentSprintCalendarResponse
            {
                SprintCalendar = new SprintCalendar
                {
                    SprintName = sprint.Name,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    Days = sprint.EnumerateAllDays().ToList(),
                    SprintMembers = sprint.SprintMembersOrderedByEmployment.ToList()
                }
            };
        }

        private PresentSprintCalendarResponse CreateMonthCalendarResponse(DateTime startDate, DateTime? endDate)
        {
            return new PresentSprintCalendarResponse
            {
                MonthCalendars = SplitInMonthIntervals(startDate, endDate)
                    .Select(x => new MonthCalendar
                    {
                        Year = x.StartDate.Value.Year,
                        Month = x.StartDate.Value.Month,
                        Days = EnumerateAllDays(x.StartDate.Value, x.EndDate.Value).ToList()
                    })
                    .ToList()
            };
        }

        private static IEnumerable<DateInterval> SplitInMonthIntervals(DateTime startDate, DateTime? endDate)
        {
            if (endDate == null)
            {
                yield return CreateNextMonthInterval(startDate, endDate);
            }
            else
            {
                DateTime date = startDate;

                while (date <= endDate)
                {
                    DateInterval monthInterval = CreateNextMonthInterval(date, endDate);
                    yield return monthInterval;

                    date = monthInterval.EndDate.Value.AddDays(1);
                }
            }
        }

        private static DateInterval CreateNextMonthInterval(DateTime startDate, DateTime? endDate)
        {
            DateTime calculatedStartDate = startDate;

            int daysInMonth = DateTime.DaysInMonth(calculatedStartDate.Year, calculatedStartDate.Month);
            DateTime calculatedEndDate = new(calculatedStartDate.Year, calculatedStartDate.Month, daysInMonth);

            if (calculatedEndDate > endDate)
                calculatedEndDate = endDate.Value;

            return new DateInterval(calculatedStartDate, calculatedEndDate);
        }

        private IEnumerable<SprintDay> EnumerateAllDays(DateTime startDate, DateTime endDate)
        {
            int totalDaysCount = (int)(endDate.Date - startDate.Date).TotalDays + 1;

            return Enumerable.Range(0, totalDaysCount)
                .Select(x =>
                {
                    DateTime date = startDate.AddDays(x);
                    return ToSprintDay(date);
                });
        }

        private SprintDay ToSprintDay(DateTime date)
        {
            List<OfficialHoliday> officialHolidays = unitOfWork.OfficialHolidayRepository.GetAll()
                .ToList();

            return new SprintDay
            {
                Date = date,
                OfficialHolidays = officialHolidays
                    .Where(x => x.Match(date))
                    .Select(x => x.GetInstanceFor(date.Year))
                    .ToList()
            };
        }

        private PresentSprintCalendarResponse CreateSprintCalendarResponse()
        {
            Sprint sprint = unitOfWork.SprintRepository.GetLastInProgress();

            if (sprint == null)
                throw new NoSprintException();

            return new PresentSprintCalendarResponse
            {
                SprintCalendar = new SprintCalendar
                {
                    SprintName = sprint.Name,
                    StartDate = sprint.StartDate,
                    EndDate = sprint.EndDate,
                    Days = sprint.EnumerateAllDays().ToList(),
                    SprintMembers = sprint.SprintMembersOrderedByEmployment.ToList()
                }
            };
        }
    }
}