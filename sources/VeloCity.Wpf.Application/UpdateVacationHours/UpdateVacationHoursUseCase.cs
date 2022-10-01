using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours
{
    internal class UpdateVacationHoursUseCase : IRequestHandler<UpdateVacationHoursRequest, Unit>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly EventBus eventBus;

        public UpdateVacationHoursUseCase(IUnitOfWork unitOfWork, EventBus eventBus)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public async Task<Unit> Handle(UpdateVacationHoursRequest request, CancellationToken cancellationToken)
        {
            TeamMember teamMember = unitOfWork.TeamMemberRepository.Get(request.TeamMemberId);

            if (teamMember == null)
                throw new Exception($"Team member with id {request.TeamMemberId} was not found.");

            DateTime date = request.Date.Date;
            DateTime previousDate = date.AddDays(-1);
            DateTime nextDate = date.AddDays(1);

            Vacation existingVacation = teamMember.Vacations
                .FirstOrDefault(x => x.Match(date));

            // no vacation exists
            if (existingVacation == null)
            {
                // no vacation is required
                if (request.Hours == null || request.Hours.Value <= 0)
                {
                    // do nothing
                }

                // partial day vacation is required
                else if (request.Hours.Value > 0 && request.Hours.Value < 8)
                {
                    // create new once vacation

                    Vacation newVacation = new VacationOnce
                    {
                        Date = date,
                        HourCount = request.Hours.Value.Value,
                        TeamMember = teamMember
                    };

                    teamMember.Vacations.Add(newVacation);

                    // todo: check if can merge with previous and/or next.
                }

                // full day vacation is required
                else if (request.Hours.Value >= 8)
                {
                    Vacation previousDayVacation = teamMember.Vacations
                        .FirstOrDefault(x => x.Match(previousDate));

                    Vacation nextDayVacation = teamMember.Vacations
                        .FirstOrDefault(x => x.Match(nextDate));

                    // previous day == once vacation
                    if (previousDayVacation is VacationOnce previousDayVacationOnce)
                    {
                        // next day == once vacation
                        if (nextDayVacation is VacationOnce nextDayVacationOnce)
                        {
                            //  delete both and create a daily for previous, current and next.

                            teamMember.Vacations.Remove(previousDayVacation);
                            teamMember.Vacations.Remove(nextDayVacation);

                            Vacation newVacation = new VacationDaily
                            {
                                DateInterval = new DateInterval(previousDate, nextDate)
                            };
                            teamMember.Vacations.Add(newVacation);
                        }

                        // next day == daily vacation
                        else if (nextDayVacation is VacationDaily nextDayVacationDaily)
                        {
                            //  delete previous and extend next to start from previous.

                            teamMember.Vacations.Remove(previousDayVacation);
                            nextDayVacationDaily.DateInterval = nextDayVacationDaily.DateInterval.InflateLeft(2);
                        }

                        else
                        {
                            //  delete previous and create a daily for previous and current.
                            teamMember.Vacations.Remove(previousDayVacation);

                            Vacation newVacation = new VacationDaily
                            {
                                DateInterval = new DateInterval(previousDate, date)
                            };
                            teamMember.Vacations.Add(newVacation);
                        }
                    }

                    // previous day == daily vacation
                    else if (previousDayVacation is VacationDaily previousDayVacationDaily)
                    {
                        // next day == once vacation
                        if (nextDayVacation is VacationOnce nextDayVacationOnce)
                        {
                            //  delete next and extend previous to end at next.

                            teamMember.Vacations.Remove(nextDayVacation);
                            previousDayVacationDaily.DateInterval = previousDayVacationDaily.DateInterval.InflateRight(2);
                        }

                        // next day == daily vacation
                        else if (nextDayVacation is VacationDaily nextDayVacationDaily)
                        {
                            //  delete next and extend previous to end at next end.

                            teamMember.Vacations.Remove(nextDayVacation);

                            DateTime? nextVacationEndDate = nextDayVacationDaily.DateInterval.EndDate;
                            previousDayVacationDaily.DateInterval = previousDayVacationDaily.DateInterval.ChangeEndDate(nextVacationEndDate);
                        }

                        // next day == other vacation
                        else
                        {
                            if (previousDayVacationDaily.HourCount == request.Hours)
                            {
                                // extend previous to end at current.

                                previousDayVacationDaily.DateInterval = previousDayVacationDaily.DateInterval.ChangeEndDate(date);
                            }
                            else
                            {
                                // create new once vacation

                                Vacation newVacation = new VacationOnce
                                {
                                    Date = date,
                                    TeamMember = teamMember
                                };

                                teamMember.Vacations.Add(newVacation);
                            }
                        }
                    }

                    // previous day == something else
                    else
                    {
                        // next day == once vacation
                        if (nextDayVacation is VacationOnce nextDayVacationOnce)
                        {
                            //  delete next and create daily for current and next.

                            teamMember.Vacations.Remove(nextDayVacation);

                            Vacation newVacation = new VacationDaily
                            {
                                DateInterval = new DateInterval(date, nextDate)
                            };
                            teamMember.Vacations.Add(newVacation);
                        }

                        // next day == daily vacation
                        else if (nextDayVacation is VacationDaily nextDayVacationDaily)
                        {
                            //  extend next to start from current.

                            nextDayVacationDaily.DateInterval = nextDayVacationDaily.DateInterval.ChangeStartDate(date);
                        }

                        else
                        {
                            // create new once vacation

                            Vacation newVacation = new VacationOnce
                            {
                                Date = date,
                                TeamMember = teamMember
                            };

                            teamMember.Vacations.Add(newVacation);
                        }
                    }
                }
            }

            // vacation exists
            else
            {
                // current day == once vacation
                if (existingVacation is VacationOnce existingVacationOnce)
                {
                    // no vacation is required
                    if (request.Hours == null || request.Hours.Value <= 0)
                    {
                        // delete the existing vacation

                        teamMember.Vacations.Remove(existingVacation);
                    }

                    // partial day vacation is required
                    else if (request.Hours.Value > 0 && request.Hours.Value < 8)
                    {
                        // change the hours count on existing.

                        existingVacationOnce.HourCount = request.Hours;

                        // todo: check if can merge with previous
                        // todo: check if can merge with next
                    }

                    // full day vacation is required
                    else if (request.Hours.Value >= 8)
                    {
                        // change the hours count on existing to 8.

                        existingVacationOnce.HourCount = null;
                    }
                }

                // current day == daily vacation
                else if (existingVacation is VacationDaily existingVacationDaily)
                {
                    // no vacation is required
                    if (request.Hours == null || request.Hours.Value <= 0)
                    {
                        // remove existing vacation

                        teamMember.Vacations.Remove(existingVacation);

                        // create vacation1 from existing start until previous

                        int previousDaysCount = (int)(previousDate - (existingVacationDaily.DateInterval.StartDate ?? DateTime.MinValue)).TotalDays + 1;
                        
                        if (previousDaysCount == 1)
                        {
                            // create new once vacation

                            Vacation newVacation = new VacationOnce
                            {
                                Date = previousDate,
                                HourCount = existingVacation.HourCount,
                                Comments = existingVacation.Comments,
                                TeamMember = teamMember
                            };

                            teamMember.Vacations.Add(newVacation);
                        }
                        else if (previousDaysCount > 1)
                        {
                            // create new daily vacation

                            Vacation newVacation = new VacationDaily
                            {
                                DateInterval = new DateInterval(existingVacationDaily.DateInterval.StartDate, previousDate),
                                HourCount = existingVacation.HourCount,
                                Comments = existingVacation.Comments,
                                TeamMember = teamMember
                            };

                            teamMember.Vacations.Add(newVacation);
                        }

                        // create vacation2 from next until existing end

                        int nextDaysCount = (int)((existingVacationDaily.DateInterval.EndDate ?? DateTime.MaxValue) - nextDate).TotalDays + 1;
                        
                        if (nextDaysCount == 1)
                        {
                            // create new once vacation

                            Vacation newVacation = new VacationOnce
                            {
                                Date = nextDate,
                                HourCount = existingVacation.HourCount,
                                Comments = existingVacation.Comments,
                                TeamMember = teamMember
                            };

                            teamMember.Vacations.Add(newVacation);
                        }
                        else if (nextDaysCount > 1)
                        {
                            // create new daily vacation

                            Vacation newVacation = new VacationDaily
                            {
                                DateInterval = new DateInterval(nextDate, existingVacationDaily.DateInterval.EndDate),
                                HourCount = existingVacation.HourCount,
                                Comments = existingVacation.Comments,
                                TeamMember = teamMember
                            };

                            teamMember.Vacations.Add(newVacation);
                        }
                    }

                    // partial day vacation is required
                    else if (request.Hours.Value > 0 && request.Hours.Value < 8)
                    {
                        // change the hours count on existing.

                        existingVacationDaily.HourCount = request.Hours;
                    }

                    // full day vacation is required
                    else if (request.Hours.Value > 8)
                    {
                        // change the hours count on existing to 8.

                        existingVacationDaily.HourCount = 8;
                    }
                }

                // current day == something else
                else
                {
                    // cannot change it.

                    throw new Exception("Cannot change the existing vacation.");
                }
            }

            unitOfWork.SaveChanges();

            TeamMemberVacationChangedEvent teamMemberVacationChangedEvent = new();
            await eventBus.Publish(teamMemberVacationChangedEvent, cancellationToken);

            return Unit.Value;
        }
    }
}