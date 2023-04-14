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

namespace DustInTheWind.VeloCity.Domain;

public readonly struct DateInterval
{
    public static DateInterval FullInfinite { get; } = new();

    public DateTime? StartDate { get; }

    public DateTime? EndDate { get; }

    public bool IsFullInfinite => StartDate == null && EndDate == null;

    public bool IsHalfInfinite => StartDate == null || EndDate == null;

    public bool IsZero => StartDate != null && EndDate != null && StartDate.Value == EndDate.Value;

    public uint TotalDays => (uint)((EndDate ?? DateTime.MaxValue) - (StartDate ?? DateTime.MinValue)).TotalDays + 1;

    public DateInterval(DateTime? startDate = null, DateTime? endDate = null)
    {
        StartDate = startDate?.Date;
        EndDate = endDate?.Date;

        if (StartDate > EndDate)
            throw new ArgumentException($"End date ({endDate}) must be after start date ({startDate}).", nameof(endDate));
    }

    public bool IsIntersecting(DateInterval dateInterval)
    {
        if (dateInterval.IsFullInfinite)
            return true;

        if (dateInterval.StartDate == null)
            return StartDate == null || dateInterval.EndDate!.Value >= StartDate.Value;

        if (dateInterval.EndDate == null)
            return EndDate == null || dateInterval.StartDate!.Value <= EndDate.Value;

        return ContainsDate(dateInterval.EndDate.Value) ||
               ((EndDate == null || dateInterval.EndDate.Value > EndDate.Value) && dateInterval.StartDate.Value <= EndDate);
    }

    public bool IsIntersecting(DateTime? startDate, DateTime? endDate)
    {
        DateInterval dateInterval = new(startDate, endDate);
        return IsIntersecting(dateInterval);
    }

    public bool ContainsDate(DateTime dateTime)
    {
        DateTime date = dateTime.Date;

        return (StartDate == null || date >= StartDate.Value) &&
               (EndDate == null || date <= EndDate.Value);
    }

    public bool DoesContinueWith(DateInterval dateInterval)
    {
        if (EndDate == null || EndDate.Value == DateTime.MaxValue.Date || dateInterval.StartDate == null)
            return false;

        return EndDate.Value.AddDays(1) == dateInterval.StartDate.Value;
    }

    public override string ToString()
    {
        string startDateString = StartDate?.ToString("d") ?? "<<<";
        string endDateString = EndDate?.ToString("d") ?? ">>>";
        return $"{startDateString} - {endDateString}";
    }

    public DateInterval InflateLeft(uint dayCount)
    {
        DateTime? newStartDate = StartDate == null
            ? null
            : StartDate - DateTime.MinValue > TimeSpan.FromDays(dayCount)
                ? StartDate.Value.AddDays(-dayCount)
                : DateTime.MinValue;

        return new DateInterval(newStartDate, EndDate);
    }

    public DateInterval InflateRight(uint dayCount)
    {
        DateTime? newEndDate = EndDate == null
            ? null
            : DateTime.MaxValue - EndDate > TimeSpan.FromDays(dayCount)
                ? EndDate.Value.AddDays(dayCount)
                : DateTime.MaxValue;

        return new DateInterval(StartDate, newEndDate);
    }

    public DateInterval ChangeStartDate(DateTime? date)
    {
        DateTime? newStartDate = date?.Date;
        return new DateInterval(newStartDate, EndDate);
    }

    public DateInterval ChangeEndDate(DateTime? date)
    {
        DateTime? newEndDate = date?.Date;
        return new DateInterval(StartDate, newEndDate);
    }

    public static DateInterval? Intersect(DateInterval dateInterval1, DateInterval dateInterval2)
    {
        return new DateIntervalIntersection(dateInterval1, dateInterval2);
    }
}