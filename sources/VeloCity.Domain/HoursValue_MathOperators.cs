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

namespace DustInTheWind.VeloCity.Domain;

public readonly partial struct HoursValue
{
    #region HoursValue +- HoursValue

    public static HoursValue operator +(HoursValue hoursValue1, HoursValue hoursValue2)
    {
        return new HoursValue
        {
            Value = hoursValue1.Value + hoursValue2.Value
        };
    }

    public static HoursValue operator -(HoursValue hoursValue1, HoursValue hoursValue2)
    {
        return new HoursValue
        {
            Value = hoursValue1.Value - hoursValue2.Value
        };
    }

    #endregion

    #region HoursValue? +- HoursValue

    public static HoursValue operator +(HoursValue? hoursValue1, HoursValue hoursValue2)
    {
        int value1 = hoursValue1?.Value ?? 0;

        return new HoursValue
        {
            Value = value1 + hoursValue2.Value
        };
    }

    public static HoursValue operator -(HoursValue? hoursValue1, HoursValue hoursValue2)
    {
        int value1 = hoursValue1?.Value ?? 0;

        return new HoursValue
        {
            Value = value1 - hoursValue2.Value
        };
    }

    #endregion

    #region HoursValue +- HoursValue?

    public static HoursValue operator +(HoursValue hoursValue1, HoursValue? hoursValue2)
    {
        int value2 = hoursValue2?.Value ?? 0;

        return new HoursValue
        {
            Value = hoursValue1.Value + value2
        };
    }

    public static HoursValue operator -(HoursValue hoursValue1, HoursValue? hoursValue2)
    {
        int value2 = hoursValue2?.Value ?? 0;

        return new HoursValue
        {
            Value = hoursValue1.Value - value2
        };
    }

    #endregion

    #region HoursValue? +- HoursValue?

    public static HoursValue operator +(HoursValue? hoursValue1, HoursValue? hoursValue2)
    {
        if (hoursValue1 == null && hoursValue2 == null)
            return null;

        int value1 = hoursValue1?.Value ?? 0;
        int value2 = hoursValue2?.Value ?? 0;

        return new HoursValue
        {
            Value = value1 + value2
        };
    }

    public static HoursValue operator -(HoursValue? hoursValue1, HoursValue? hoursValue2)
    {
        if (hoursValue1 == null && hoursValue2 == null)
            return null;

        int value1 = hoursValue1?.Value ?? 0;
        int value2 = hoursValue2?.Value ?? 0;

        return new HoursValue
        {
            Value = value1 - value2
        };
    }

    #endregion

    #region HoursValue +- int

    public static HoursValue operator +(HoursValue hoursValue1, int hoursValue2)
    {
        return new HoursValue
        {
            Value = hoursValue1.Value + hoursValue2
        };
    }

    public static HoursValue operator -(HoursValue hoursValue1, int hoursValue2)
    {
        return new HoursValue
        {
            Value = hoursValue1.Value - hoursValue2
        };
    }

    #endregion

    #region HoursValue? +- int

    public static HoursValue operator +(HoursValue? hoursValue1, int hoursValue2)
    {
        int value1 = hoursValue1?.Value ?? 0;

        return new HoursValue
        {
            Value = value1 + hoursValue2
        };
    }

    public static HoursValue operator -(HoursValue? hoursValue1, int hoursValue2)
    {
        int value1 = hoursValue1?.Value ?? 0;

        return new HoursValue
        {
            Value = value1 - hoursValue2
        };
    }

    #endregion

    #region int +- HoursValue

    public static HoursValue operator +(int hoursValue1, HoursValue hoursValue2)
    {
        return new HoursValue
        {
            Value = hoursValue1 + hoursValue2.Value
        };
    }

    public static HoursValue operator -(int hoursValue1, HoursValue hoursValue2)
    {
        return new HoursValue
        {
            Value = hoursValue1 - hoursValue2.Value
        };
    }

    #endregion

    #region int +- HoursValue?

    public static int operator +(int hoursValue1, HoursValue? hoursValue2)
    {
        int value2 = hoursValue2?.Value ?? 0;

        return new HoursValue
        {
            Value = hoursValue1 + value2
        };
    }

    public static int operator -(int hoursValue1, HoursValue? hoursValue2)
    {
        int value2 = hoursValue2 ?? 0;

        return new HoursValue
        {
            Value = hoursValue1 - value2
        };
    }

    #endregion
}