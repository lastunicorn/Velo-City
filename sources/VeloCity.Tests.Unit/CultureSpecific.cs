﻿// VeloCity
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

using System.Globalization;

namespace DustInTheWind.VeloCity.Tests.Unit;

internal static class CultureSpecific
{
    public static T RunUsingCulture<T>(CultureInfo cultureInfo, Func<T> action)
    {
        CultureInfo oldCultureInfo = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = cultureInfo;

        try
        {
            return action();
        }
        finally
        {
            CultureInfo.CurrentCulture = oldCultureInfo;
        }
    }

    public static void RunUsingCulture(CultureInfo cultureInfo, Action action)
    {
        CultureInfo oldCultureInfo = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = cultureInfo;

        try
        {
            action();
        }
        finally
        {
            CultureInfo.CurrentCulture = oldCultureInfo;
        }
    }
}