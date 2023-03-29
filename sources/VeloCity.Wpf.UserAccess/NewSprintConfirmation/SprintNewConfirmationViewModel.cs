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
using DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

namespace DustInTheWind.VeloCity.Wpf.UserAccess.NewSprintConfirmation;

public class SprintNewConfirmationViewModel : ViewModelBase
{
    private string sprintTitle;
    private DateTime endDate = DateTime.Today.AddDays(1);
    private uint sprintLength;

    public string Title { get; }

    public string SprintTitle
    {
        get => sprintTitle;
        set
        {
            sprintTitle = value;
            OnPropertyChanged();
        }
    }

    public DateTime StartDate { get; }

    public DateTime EndDate
    {
        get => endDate;
        private set
        {
            endDate = value;
            OnPropertyChanged();
        }
    }

    public uint SprintLength
    {
        get => sprintLength;
        set
        {
            //if (value <= 0)
            //    throw new ArgumentException("Sprint length must be a positive integer.");

            sprintLength = value;
            OnPropertyChanged();

            UpdateEndDate();
        }
    }

    public SprintNewConfirmationViewModel(int sprintNumber, DateTime startDate)
    {
        Title = $"Create Sprint {sprintNumber}";
        StartDate = startDate;
        UpdateEndDate();
    }

    private void UpdateEndDate()
    {
        EndDate = sprintLength > 0
            ? StartDate.AddDays(sprintLength - 1)
            : StartDate;
    }
}