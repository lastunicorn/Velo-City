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
using System.Collections.ObjectModel;
using System.Linq;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class VacationCollection : Collection<Vacation>
{
    public event EventHandler Changed;

    public VacationCollection()
    {
    }

    public VacationCollection(IEnumerable<Vacation> vacations)
    {
        if (vacations == null) throw new ArgumentNullException(nameof(vacations));

        IEnumerable<Vacation> nonNullVacations = vacations.Where(x => x != null);

        foreach (Vacation vacation in nonNullVacations)
        {
            Items.Add(vacation);
            vacation.Changed += HandleVacationChanged;
        }
    }

    protected override void InsertItem(int index, Vacation item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        base.InsertItem(index, item);

        item.Changed += HandleVacationChanged;

        OnChanged();
    }

    protected override void RemoveItem(int index)
    {
        Vacation vacation = Items[index];
        vacation.Changed -= HandleVacationChanged;

        base.RemoveItem(index);

        OnChanged();
    }

    protected override void SetItem(int index, Vacation item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        Vacation vacation = Items[index];
        vacation.Changed -= HandleVacationChanged;

        base.SetItem(index, item);

        item.Changed += HandleVacationChanged;

        OnChanged();
    }

    protected override void ClearItems()
    {
        foreach (Vacation vacation in Items)
            vacation.Changed -= HandleVacationChanged;

        base.ClearItems();

        OnChanged();
    }

    private void HandleVacationChanged(object sender, EventArgs e)
    {
        OnChanged();
    }

    public void Remove(DateTime date)
    {
        date = date.Date;

        Vacation existingVacation = Items.FirstOrDefault(x => x.Match(date));

        // current day == once vacation
        if (existingVacation is VacationOnce)
        {
            // delete the existing vacation

            Items.Remove(existingVacation);
        }

        // current day == daily vacation
        else if (existingVacation is VacationDaily existingVacationDaily)
        {
            DateTime previousDate = date.AddDays(-1);
            DateTime nextDate = date.AddDays(1);

            // remove existing vacation

            Items.Remove(existingVacation);

            // create vacation1 from existing start until previous

            int previousDaysCount = (int)(previousDate - (existingVacationDaily.DateInterval.StartDate ?? DateTime.MinValue)).TotalDays + 1;

            if (previousDaysCount == 1)
            {
                // create new once vacation

                Vacation newVacation = new VacationOnce
                {
                    Date = previousDate,
                    HourCount = existingVacation.HourCount,
                    Comments = existingVacation.Comments
                };

                Items.Add(newVacation);
            }
            else if (previousDaysCount > 1)
            {
                // create new daily vacation

                Vacation newVacation = new VacationDaily
                {
                    DateInterval = new DateInterval(existingVacationDaily.DateInterval.StartDate, previousDate),
                    HourCount = existingVacation.HourCount,
                    Comments = existingVacation.Comments
                };

                Items.Add(newVacation);
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
                    Comments = existingVacation.Comments
                };

                Items.Add(newVacation);
            }
            else if (nextDaysCount > 1)
            {
                // create new daily vacation

                Vacation newVacation = new VacationDaily
                {
                    DateInterval = new DateInterval(nextDate, existingVacationDaily.DateInterval.EndDate),
                    HourCount = existingVacation.HourCount,
                    Comments = existingVacation.Comments
                };

                Items.Add(newVacation);
            }
        }

        // current day == something else
        else
        {
            // cannot change it.

            throw new Exception("Cannot change the existing vacation.");
        }
    }

    public void AddForDate(DateTime date, HoursValue? hours = null)
    {
        DateTime previousDate = date.AddDays(-1);
        DateTime nextDate = date.AddDays(1);

        Vacation previousVacation = Items.FirstOrDefault(x => x.Match(previousDate));
        Vacation nextVacation = Items.FirstOrDefault(x => x.Match(nextDate));

        // previous day == once vacation
        if (previousVacation is VacationOnce previousVacationOnce && previousVacationOnce.HourCount == hours)
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
            {
                //  delete both and create a daily for previous, current and next.

                previousVacation.Changed -= HandleVacationChanged;
                Items.Remove(previousVacation);

                nextVacation.Changed -= HandleVacationChanged;
                Items.Remove(nextVacation);

                Vacation newVacation = new VacationDaily
                {
                    DateInterval = new DateInterval(previousDate, nextDate),
                    HourCount = hours
                };
                Items.Add(newVacation);
                newVacation.Changed += HandleVacationChanged;
            }

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextVacationDaily && nextVacationDaily.HourCount == hours)
            {
                //  delete previous and extend next to start from previous.

                previousVacation.Changed -= HandleVacationChanged;
                Items.Remove(previousVacation);

                nextVacationDaily.DateInterval = nextVacationDaily.DateInterval.InflateLeft(2);
            }

            // next day == other vacation (cannot merge)
            else
            {
                //  delete previous and create a daily for previous and current.

                previousVacation.Changed -= HandleVacationChanged;
                Items.Remove(previousVacation);

                Vacation newVacation = new VacationDaily
                {
                    DateInterval = new DateInterval(previousDate, date),
                    HourCount = hours
                };
                Items.Add(newVacation);
                newVacation.Changed += HandleVacationChanged;
            }
        }

        // previous day == daily vacation
        else if (previousVacation is VacationDaily previousDayVacationDaily && previousDayVacationDaily.HourCount == hours)
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
            {
                //  delete next and extend previous to end at next.

                nextVacation.Changed -= HandleVacationChanged;
                Items.Remove(nextVacation);

                previousDayVacationDaily.DateInterval = previousDayVacationDaily.DateInterval.InflateRight(2);
            }

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextDayVacationDaily && nextDayVacationDaily.HourCount == hours)
            {
                //  delete next and extend previous to end at next end.

                nextVacation.Changed -= HandleVacationChanged;
                Items.Remove(nextVacation);

                DateTime? nextVacationEndDate = nextDayVacationDaily.DateInterval.EndDate;
                previousDayVacationDaily.DateInterval = previousDayVacationDaily.DateInterval.ChangeEndDate(nextVacationEndDate);
            }

            // next day == other vacation (cannot merge)
            else
            {
                // extend previous to end at current.

                previousDayVacationDaily.DateInterval = previousDayVacationDaily.DateInterval.ChangeEndDate(date);
            }
        }

        // previous day == something else (cannot merge)
        else
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
            {
                //  delete next and create daily for current and next.

                nextVacation.Changed -= HandleVacationChanged;
                Items.Remove(nextVacation);

                Vacation newVacation = new VacationDaily
                {
                    DateInterval = new DateInterval(date, nextDate),
                    HourCount = hours
                };
                Items.Add(newVacation);
                newVacation.Changed += HandleVacationChanged;
            }

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextDayVacationDaily && nextDayVacationDaily.HourCount == hours)
            {
                //  extend next to start from current.

                nextDayVacationDaily.DateInterval = nextDayVacationDaily.DateInterval.ChangeStartDate(date);
            }

            // next day == other vacation (cannot merge)
            else
            {
                // create new once vacation

                Vacation newVacation = new VacationOnce
                {
                    Date = date,
                    HourCount = hours
                };

                Items.Add(newVacation);
                newVacation.Changed += HandleVacationChanged;
            }
        }
    }

    public IEnumerable<Vacation> GetVacationsFor(DateTime date)
    {
        return Items.Where(x => x.Match(date));
    }

    protected virtual void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}