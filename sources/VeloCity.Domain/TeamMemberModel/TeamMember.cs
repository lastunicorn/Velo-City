// VeloCity
// Copyright (C) 2022-2023 Dust in the Wind
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

using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public class TeamMember
{
    private List<VelocityPenalty> velocityPenalties;
    private VacationCollection vacations;
    private int id;
    private readonly EmploymentCollection employments = new();

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

    public EmploymentCollection Employments
    {
        get => employments;
        init
        {
            if (value != null)
                employments = value;
        }
    }

    public bool HasActiveEmployment
    {
        get
        {
            Employment employment = Employments.GetLastEmployment();

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

    public void SetVacation(DateTime date, HoursValue? hours)
    {
        date = date.Date;

        Employment employment = Employments.GetEmploymentFor(date);

        bool allowToSetVacation = employment != null || hours <= 0;

        if (!allowToSetVacation)
            throw new NotEmployedException(id, date);

        Vacations ??= new VacationCollection();
        Vacations.SetVacation(date, hours);
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