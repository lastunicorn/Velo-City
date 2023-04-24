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

using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.UserAccess.NewTeamMemberConfirmation;

internal class NewTeamMemberConfirmationViewModel : ViewModelBase
{
    private int employmentHours;
    private string employmentCountry;
    private DateTime startDate;
    private string firstName;
    private string middleName;
    private string lastName;
    private string nickname;

    public string Title { get; }

    public int EmploymentHours
    {
        get => employmentHours;
        set
        {
            if (value == employmentHours)
                return;

            employmentHours = value;
            OnPropertyChanged();
        }
    }

    public string EmploymentCountry
    {
        get => employmentCountry;
        set
        {
            if (value == employmentCountry)
                return;

            employmentCountry = value;
            OnPropertyChanged();
        }
    }

    public DateTime StartDate
    {
        get => startDate;
        set
        {
            if (value.Equals(startDate))
                return;

            startDate = value;
            OnPropertyChanged();
        }
    }

    public string FirstName
    {
        get => firstName;
        set
        {
            if (value == firstName) return;
            firstName = value;
            OnPropertyChanged();
        }
    }

    public string MiddleName
    {
        get => middleName;
        set
        {
            if (value == middleName) return;
            middleName = value;
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => lastName;
        set
        {
            if (value == lastName) return;
            lastName = value;
            OnPropertyChanged();
        }
    }

    public string Nickname
    {
        get => nickname;
        set
        {
            if (value == nickname) return;
            nickname = value;
            OnPropertyChanged();
        }
    }

    public NewTeamMemberConfirmationViewModel()
    {
        Title = "Create Team Member";
    }
}