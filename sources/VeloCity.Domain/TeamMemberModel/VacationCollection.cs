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

using System.Collections.ObjectModel;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class VacationCollection : Collection<Vacation>
{
    public event EventHandler Changed;

    public VacationCollection()
    {
    }

    public VacationCollection(IEnumerable<Vacation> vacations)
    {
        if (vacations == null)
            return;

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

    public void SetVacation(DateTime date, HoursValue? hours, VacationSetOption option = VacationSetOption.WholeSeries)
    {
        date = date.Date;

        Vacation existingVacation = Items.FirstOrDefault(x => x.Match(date));

        switch (existingVacation)
        {
            case null:
                SetVacation_WhenCurrentIsNone(date, hours);
                break;

            case VacationOnce existingVacationOnce:
                SetVacation_WhenCurrentIsOnce(existingVacationOnce, hours);
                break;

            case VacationDaily existingVacationDaily:
                if (existingVacationDaily.DateInterval.StartDate == date && existingVacationDaily.DateInterval.EndDate == date)
                    SetVacation_WhenCurrentIsDaily_Current(existingVacationDaily, date, hours);
                else
                    SetVacation_WhenCurrentIsDaily(existingVacationDaily, date, hours, option);
                break;

            case VacationWeekly:
            case VacationMonthly:
            case VacationYearly:
                SetVacation_WhenCurrentIsSeries(existingVacation, hours, option);

                break;

            default:
                throw new Exception("Vacation type not supported.");
        }
    }

    private void SetVacation_WhenCurrentIsNone(DateTime date, HoursValue? hours)
    {
        if (hours <= 0)
            return;

        DateTime previousDate = date.AddDays(-1);
        DateTime nextDate = date.AddDays(1);

        Vacation previousVacation = Items.FirstOrDefault(x => x.Match(previousDate));
        Vacation nextVacation = Items.FirstOrDefault(x => x.Match(nextDate));

        // previous day == once vacation (can merge left)
        if (previousVacation is VacationOnce previousVacationOnce && previousVacationOnce.HourCount == hours)
        {
            // next day == once vacation (can merge right)
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                Merge(previousVacationOnce, nextVacationOnce);

            // next day == daily vacation (can merge right)
            else if (nextVacation is VacationDaily nextVacationDaily && nextVacationDaily.HourCount == hours)
                Merge(previousVacationOnce, nextVacationDaily);

            // next day == other vacation (cannot merge right)
            else
                ExtendRight(previousVacationOnce);
        }

        // previous day == daily vacation (can merge left)
        else if (previousVacation is VacationDaily previousDayVacationDaily && previousDayVacationDaily.HourCount == hours)
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                Merge(previousDayVacationDaily, nextVacationOnce);

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextDayVacationDaily && nextDayVacationDaily.HourCount == hours)
                Merge(previousDayVacationDaily, nextDayVacationDaily);

            // next day == other vacation (cannot merge right)
            else
                ExtendRight(previousDayVacationDaily);
        }

        // previous day == something else (cannot merge left)
        else
        {
            // next day == once vacation (can merge right)
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                ExtendLeft(nextVacationOnce);

            // next day == daily vacation (can merge right)
            else if (nextVacation is VacationDaily nextDayVacationDaily && nextDayVacationDaily.HourCount == hours)
                ExtendLeft(nextDayVacationDaily);

            // next day == other vacation (cannot merge right)
            else
            {
                // create new once vacation

                AddInternal(new VacationOnce
                {
                    Date = date,
                    HourCount = hours
                });
            }
        }
    }

    private void SetVacation_WhenCurrentIsOnce(VacationOnce existingVacation, HoursValue? hours)
    {
        if (hours == existingVacation.HourCount)
            return;

        if (hours <= 0)
        {
            RemoveInternal(existingVacation);
            return;
        }

        DateTime previousDate = existingVacation.Date.AddDays(-1);
        DateTime nextDate = existingVacation.Date.AddDays(1);

        Vacation previousVacation = Items.FirstOrDefault(x => x.Match(previousDate));
        Vacation nextVacation = Items.FirstOrDefault(x => x.Match(nextDate));

        // previous day == once vacation
        if (previousVacation is VacationOnce previousVacationOnce && previousVacationOnce.HourCount == hours)
        {
            RemoveInternal(existingVacation);

            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                Merge(previousVacationOnce, nextVacationOnce);

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextVacationDaily && nextVacationDaily.HourCount == hours)
                Merge(previousVacationOnce, nextVacationDaily);

            // next day == other vacation (cannot merge right)
            else
                ExtendRight(previousVacationOnce);
        }

        // previous day == daily vacation
        else if (previousVacation is VacationDaily previousDayVacationDaily && previousDayVacationDaily.HourCount == hours)
        {
            RemoveInternal(existingVacation);

            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                Merge(previousDayVacationDaily, nextVacationOnce);

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextDayVacationDaily && nextDayVacationDaily.HourCount == hours)
                Merge(previousDayVacationDaily, nextDayVacationDaily);

            // next day == other vacation (cannot merge right)
            else
                ExtendRight(previousDayVacationDaily);
        }

        // previous day == something else (cannot merge right)
        else
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
            {
                RemoveInternal(existingVacation);
                ExtendLeft(nextVacationOnce);
            }

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextDayVacationDaily && nextDayVacationDaily.HourCount == hours)
            {
                RemoveInternal(existingVacation);
                ExtendLeft(nextDayVacationDaily);
            }

            // next day == other vacation (cannot merge)
            else
            {
                existingVacation.HourCount = hours;
            }
        }
    }

    private void SetVacation_WhenCurrentIsDaily_Current(VacationDaily existingVacation, DateTime date, HoursValue? hours)
    {
        if (hours == existingVacation.HourCount)
            return;

        RemoveInternal(existingVacation);

        if (hours <= 0)
            return;

        DateTime previousDate = date.AddDays(-1);
        DateTime nextDate = date.AddDays(1);

        Vacation previousVacation = Items.FirstOrDefault(x => x.Match(previousDate));
        Vacation nextVacation = Items.FirstOrDefault(x => x.Match(nextDate));

        // previous day == once vacation
        if (previousVacation is VacationOnce previousVacationOnce && previousVacationOnce.HourCount == hours)
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                Merge(previousVacationOnce, nextVacationOnce);

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextVacationDaily && nextVacationDaily.HourCount == hours)
                Merge(previousVacationOnce, nextVacationDaily);

            // next day == other vacation (cannot merge)
            else
                ExtendRight(previousVacationOnce);
        }

        // previous day == daily vacation
        else if (previousVacation is VacationDaily previousVacationDaily && previousVacationDaily.HourCount == hours)
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                Merge(previousVacationDaily, nextVacationOnce);

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextVacationDaily && nextVacationDaily.HourCount == hours)
                Merge(previousVacationDaily, nextVacationDaily);

            // next day == other vacation (cannot merge)
            else
                ExtendRight(previousVacationDaily);
        }

        // previous day == something else (cannot merge left)
        else
        {
            // next day == once vacation
            if (nextVacation is VacationOnce nextVacationOnce && nextVacationOnce.HourCount == hours)
                ExtendLeft(nextVacationOnce);

            // next day == daily vacation
            else if (nextVacation is VacationDaily nextVacationDaily && nextVacationDaily.HourCount == hours)
                ExtendLeft(nextVacationDaily);

            // next day == other vacation (cannot merge)
            else
            {
                AddInternal(new VacationOnce
                {
                    Date = date,
                    HourCount = hours
                });
            }
        }
    }

    private void SetVacation_WhenCurrentIsDaily(VacationDaily existingVacation, DateTime date, HoursValue? hours, VacationSetOption option)
    {
        if (hours == existingVacation.HourCount)
            return;

        switch (option)
        {
            case VacationSetOption.SingleDay:
                RemoveSingleDay(existingVacation, date);

                if (hours > 0)
                {
                    AddInternal(new VacationOnce
                    {
                        Date = date,
                        HourCount = hours
                    });
                }

                break;

            case VacationSetOption.WholeSeries:
                if (hours <= 0)
                    RemoveInternal(existingVacation);
                else
                    existingVacation.HourCount = hours;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(option));
        }
    }

    private void RemoveSingleDay(VacationDaily vacation, DateTime date)
    {
        bool dateIsDuringVacation = vacation.DateInterval.ContainsDate(date);

        if (!dateIsDuringVacation)
            return;

        uint leftDays = vacation.CountDaysBefore(date);
        uint rightDays = vacation.CountDaysAfter(date);

        if (leftDays == 0)
        {
            switch (rightDays)
            {
                case 0:
                    RemoveInternal(vacation);
                    break;

                case 1:
                    RemoveInternal(vacation);
                    AddInternal(new VacationOnce
                    {
                        Date = vacation.EndDate ?? DateTime.MaxValue,
                        HourCount = vacation.HourCount
                    });
                    break;

                default:
                    DateTime startDate = date.AddDays(1);
                    vacation.ChangeStartDate(startDate);
                    break;
            }
        }
        else if (leftDays == 1)
        {
            switch (rightDays)
            {
                case 0:
                    RemoveInternal(vacation);
                    break;

                case 1:
                    RemoveInternal(vacation);
                    AddInternal(new VacationOnce
                    {
                        Date = vacation.EndDate ?? DateTime.MaxValue,
                        HourCount = vacation.HourCount
                    });
                    break;

                default:
                    DateTime startDate = date.AddDays(1);
                    vacation.ChangeStartDate(startDate);
                    break;
            }

            AddInternal(new VacationOnce
            {
                Date = vacation.StartDate ?? DateTime.MinValue,
                HourCount = vacation.HourCount
            });
        }
        else
        {
            DateTime? lastDate = vacation.EndDate;

            DateTime endDate = date.AddDays(-1);
            vacation.ChangeEndDate(endDate);

            switch (rightDays)
            {
                case 0:
                    break;

                case 1:
                    AddInternal(new VacationOnce
                    {
                        Date = vacation.EndDate ?? DateTime.MaxValue,
                        HourCount = vacation.HourCount
                    });
                    break;

                default:
                    DateTime startDate = date.AddDays(1);

                    AddInternal(new VacationDaily
                    {
                        DateInterval = new DateInterval(startDate, lastDate),
                        HourCount = vacation.HourCount
                    });

                    break;
            }
        }
    }

    private void SetVacation_WhenCurrentIsSeries(Vacation existingVacation, HoursValue? hours, VacationSetOption option)
    {
        switch (option)
        {
            case VacationSetOption.SingleDay:
                throw new Exception("Updating a single day from a series is not supported.");

            case VacationSetOption.WholeSeries when hours <= 0:
                RemoveInternal(existingVacation);
                break;

            case VacationSetOption.WholeSeries:
                existingVacation.HourCount = hours;
                break;
        }
    }

    private void Merge(VacationOnce vacationLeft, VacationOnce vacationRight)
    {
        RemoveInternal(vacationLeft);
        RemoveInternal(vacationRight);

        DateTime startDate = vacationLeft.Date;
        DateTime endDate = vacationRight.Date;

        AddInternal(new VacationDaily
        {
            DateInterval = new DateInterval(startDate, endDate),
            HourCount = vacationLeft.HourCount
        });
    }

    private void Merge(VacationOnce vacationLeft, VacationDaily vacationRight)
    {
        RemoveInternal(vacationLeft);
        vacationRight.ChangeStartDate(vacationLeft.Date);
    }

    private void Merge(VacationDaily vacationLeft, VacationOnce vacationRight)
    {
        RemoveInternal(vacationRight);
        vacationLeft.ChangeEndDate(vacationRight.Date);
    }

    private void Merge(VacationDaily vacationLeft, VacationDaily vacationRight)
    {
        RemoveInternal(vacationRight);
        vacationLeft.ChangeEndDate(vacationRight.EndDate);
    }

    private void ExtendLeft(VacationOnce vacation)
    {
        //  delete vacation and create daily for new-left and vacation.

        RemoveInternal(vacation);

        DateTime startDate = vacation.Date.AddDays(-1);
        DateTime endDate = vacation.Date;

        Vacation newVacation = new VacationDaily
        {
            DateInterval = new DateInterval(startDate, endDate),
            HourCount = vacation.HourCount
        };
        AddInternal(newVacation);
    }

    private static void ExtendLeft(VacationDaily vacation)
    {
        vacation.ExtendLeft(1);
    }

    private void ExtendRight(VacationOnce vacation)
    {
        //  delete vacation and create a daily for vacation and new-right.

        RemoveInternal(vacation);

        DateTime startDate = vacation.Date;
        DateTime endDate = vacation.Date.AddDays(1);

        AddInternal(new VacationDaily
        {
            DateInterval = new DateInterval(startDate, endDate),
            HourCount = vacation.HourCount
        });
    }

    private static void ExtendRight(VacationDaily vacation)
    {
        vacation.ExtendRight(1);
    }

    private void AddInternal(Vacation vacation)
    {
        Items.Add(vacation);
        vacation.Changed += HandleVacationChanged;
    }

    private void RemoveInternal(Vacation vacation)
    {
        vacation.Changed -= HandleVacationChanged;
        Items.Remove(vacation);
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