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

namespace DustInTheWind.VeloCity.Domain;

public readonly partial struct HoursValue
{
    #region HoursValue vs HoursValue

    public static bool operator >(HoursValue hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1.Value > hoursValue2.Value;
    }

    public static bool operator <(HoursValue hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1.Value < hoursValue2.Value;
    }

    public static bool operator >=(HoursValue hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1.Value >= hoursValue2.Value;
    }

    public static bool operator <=(HoursValue hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1.Value <= hoursValue2.Value;
    }

    #endregion

    #region int vs HoursValue

    public static bool operator >(int hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1 > hoursValue2.Value;
    }

    public static bool operator <(int hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1 < hoursValue2.Value;
    }

    public static bool operator >=(int hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1 >= hoursValue2.Value;
    }

    public static bool operator <=(int hoursValue1, HoursValue hoursValue2)
    {
        return hoursValue1 <= hoursValue2.Value;
    }

    #endregion

    #region HoursValue vs int

    public static bool operator >(HoursValue hoursValue1, int hoursValue2)
    {
        return hoursValue1.Value > hoursValue2;
    }

    public static bool operator <(HoursValue hoursValue1, int hoursValue2)
    {
        return hoursValue1.Value < hoursValue2;
    }

    public static bool operator >=(HoursValue hoursValue1, int hoursValue2)
    {
        return hoursValue1.Value >= hoursValue2;
    }

    public static bool operator <=(HoursValue hoursValue1, int hoursValue2)
    {
        return hoursValue1.Value <= hoursValue2;
    }

    #endregion

    #region int vs HoursValue?

    public static bool operator >(int hoursValue1, HoursValue? hoursValue2)
    {
        return hoursValue1 > hoursValue2?.Value;
    }

    public static bool operator <(int hoursValue1, HoursValue? hoursValue2)
    {
        return hoursValue1 < hoursValue2?.Value;
    }

    public static bool operator >=(int hoursValue1, HoursValue? hoursValue2)
    {
        return hoursValue1 >= hoursValue2?.Value;
    }

    public static bool operator <=(int hoursValue1, HoursValue? hoursValue2)
    {
        return hoursValue1 <= hoursValue2?.Value;
    }

    #endregion

    #region HoursValue? vs int

    public static bool operator >(HoursValue? hoursValue1, int hoursValue2)
    {
        return hoursValue1?.Value > hoursValue2;
    }

    public static bool operator <(HoursValue? hoursValue1, int hoursValue2)
    {
        return hoursValue1?.Value < hoursValue2;
    }

    public static bool operator >=(HoursValue? hoursValue1, int hoursValue2)
    {
        return hoursValue1?.Value >= hoursValue2;
    }

    public static bool operator <=(HoursValue? hoursValue1, int hoursValue2)
    {
        return hoursValue1?.Value <= hoursValue2;
    }

    #endregion
}