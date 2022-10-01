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

namespace DustInTheWind.VeloCity.ChartTools
{
    public class ChartBarValue<T> : IChartBarValue
    {
        private int? actualSpace;
        private int? actualMaxValue;
        private int? actualFillValue;
        private int? actualEmptyValue;
        private int? actualEmptySpace;

        public Chart<T> Container { get; set; }

        public T Item { get; set; }

        public int MaxValue { get; set; }

        public int FillValue { get; set; }

        public int ActualSpace => actualSpace ?? MaxValue;

        public int ActualMaxValue => actualMaxValue ?? MaxValue;

        public int ActualFillValue => actualFillValue ?? FillValue;

        public int ActualEmptyValue => actualEmptyValue ?? (ActualMaxValue - ActualFillValue);

        public int ActualEmptySpace => actualEmptySpace ?? (ActualSpace - ActualMaxValue);

        public void Calculate()
        {
            if (Container == null)
                return;

            actualSpace = Container.ActualSize;
            actualMaxValue = (int)Math.Round((double)actualSpace * MaxValue / Container.MaxValue);
            actualFillValue = (int)Math.Round((double)actualSpace * FillValue / Container.MaxValue);
            actualEmptyValue = actualMaxValue - actualFillValue;
            actualEmptySpace = actualSpace - actualMaxValue;
        }

        public override string ToString()
        {
            if (MaxValue == 0)
                return string.Empty;

            return new string('═', ActualFillValue) + new string('-', ActualEmptyValue);
        }
    }
}