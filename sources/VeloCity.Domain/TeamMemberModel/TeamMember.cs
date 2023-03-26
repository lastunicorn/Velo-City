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
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class TeamMember
{
    private List<VelocityPenalty> velocityPenalties;
    private VacationCollection vacations;
    private int id;

    public int Id
    {
        get => id;
        set
        {
            if (id != 0)
                throw new InvalidOperationException("Once assigned, the id of a team member cannot be changed.");

            id = value;
        }
    }

    public PersonName Name { get; init; }

    public EmploymentCollection Employments { get; init; }

    public bool HasActiveEmployment
    {
        get
        {
            Employment employment = Employments?.GetLastEmployment();
            return employment is { EndDate: null };
        }
    }

    public string Comments { get; set; }

    public VacationCollection Vacations
    {
        get => vacations;
        set
        {
            if (vacations != null)
                vacations.Changed -= HandleVacationsChanged;

            vacations = value;

            if (vacations != null)
                vacations.Changed += HandleVacationsChanged;
        }
    }

    public List<VelocityPenalty> VelocityPenalties
    {
        get => velocityPenalties;
        set
        {
            if (velocityPenalties != null)
            {
                foreach (VelocityPenalty velocityPenalty in velocityPenalties)
                    velocityPenalty.TeamMember = null;
            }

            velocityPenalties = value;

            if (velocityPenalties != null)
            {
                foreach (VelocityPenalty velocityPenalty in velocityPenalties)
                    velocityPenalty.TeamMember = this;
            }
        }
    }

    public event EventHandler VacationsChanged;

    private void HandleVacationsChanged(object sender, EventArgs e)
    {
        OnVacationsChanged();
    }

    public SprintMember ToSprintMember(Sprint sprint)
    {
        return new SprintMember(this, sprint);
    }

    public void RemoveVacation(DateTime date)
    {
        Vacations?.Remove(date);
    }

    public void AddVacation(DateTime date, HoursValue hours)
    {
        date = date.Date;

        Employment employment = Employments?.GetEmploymentFor(date);
        Vacation existingVacation = Vacations?.FirstOrDefault(x => x.Match(date));

        if (employment == null)
        {
            // vacation is required
            if (hours.Value > 0)
            {
                throw new NotEmployedException(id, date);
            }
        }
        else
        {
            int maxHourPerDay = employment.HoursPerDay;

            // no vacation exists
            if (existingVacation == null)
            {
                Vacations ??= new VacationCollection();

                if (hours < 0)
                    hours = 0;

                if (hours > maxHourPerDay)
                    hours = maxHourPerDay;

                Vacations.AddForDate(date, hours.Value);



                //// partial day vacation is required
                //if (hours.Value > 0 && hours.Value < employment.HoursPerDay)
                //{
                //    Vacations ??= new VacationCollection();
                //    Vacations.AddPartial(date, hours.Value);
                //}

                //// full day vacation is required
                //else if (hours.Value >= maxHourPerDay)
                //{
                //    Vacations ??= new VacationCollection();
                //    Vacations.AddFull(date);
                //}
            }

            // vacation exists
            else
            {
                // current day == once vacation
                if (existingVacation is VacationOnce existingVacationOnce)
                {
                    // partial day vacation is required
                    if (hours.Value > 0 && hours.Value < maxHourPerDay)
                    {
                        // change the hours count on existing.

                        existingVacationOnce.HourCount = hours;

                        // todo: check if can merge with previous
                        // todo: check if can merge with next
                    }

                    // full day vacation is required
                    else if (hours.Value >= maxHourPerDay)
                    {
                        // change the hours count on existing to full.

                        existingVacationOnce.HourCount = null;
                    }
                }

                // current day == daily vacation
                else if (existingVacation is VacationDaily existingVacationDaily)
                {
                    // partial day vacation is required
                    if (hours.Value > 0 && hours.Value < maxHourPerDay)
                    {
                        // change the hours count on existing.

                        existingVacationDaily.HourCount = hours;
                    }

                    // full day vacation is required
                    else if (hours.Value > maxHourPerDay)
                    {
                        // change the hours count on existing to full.

                        existingVacationDaily.HourCount = null;
                    }
                }

                // current day == something else
                else
                {
                    // cannot change it.

                    throw new Exception("Cannot change the existing vacation.");
                }
            }
        }
    }

    public IEnumerable<Vacation> GetVacationsFor(DateTime date)
    {
        return Vacations?.GetVacationsFor(date) ?? Enumerable.Empty<Vacation>();
    }

    public override string ToString()
    {
        return Name.FullName;
    }

    protected virtual void OnVacationsChanged()
    {
        VacationsChanged?.Invoke(this, EventArgs.Empty);
    }
}