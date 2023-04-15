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

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private volatile bool isInitializeMode;

    protected bool IsInitializeMode => isInitializeMode;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void RunInInitializeMode(Action action)
    {
        isInitializeMode = true;

        try
        {
            action();
        }
        finally
        {
            isInitializeMode = false;
        }
    }

    protected async Task RunInInitializeMode(Func<Task> action)
    {
        isInitializeMode = true;

        try
        {
            await action();
        }
        finally
        {
            isInitializeMode = false;
        }
    }
}