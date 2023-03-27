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

namespace DustInTheWind.VeloCity.Domain.TeamMemberModel;

public abstract class Vacation
{
    private int? hourCount;
    private string comments;

    public int? HourCount
    {
        get => hourCount;
        set
        {
            hourCount = value;
            OnChanged();
        }
    }

    public string Comments
    {
        get => comments;
        set
        {
            comments = value;
            OnChanged();
        }
    }

    public event EventHandler Changed;

    public abstract bool Match(DateTime date);

    protected virtual void OnChanged()
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}