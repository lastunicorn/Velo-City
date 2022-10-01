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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DustInTheWind.VeloCity.ChartTools
{
    public class Chart : IEnumerable<ChartBarValue>
    {
        private readonly List<ChartBarValue> chartBars = new();
        private int? actualSize;

        public int ActualSize
        {
            get => actualSize ?? MaxValue;
            set => actualSize = value;
        }

        public int MaxValue { get; private set; }

        public ChartBarValue this[int index] => chartBars[index];

        public void Add(ChartBarValue chartBarValue)
        {
            if (chartBarValue.Container != null)
                throw new ArgumentException("The chart bar is already part of another chart.", nameof(chartBarValue));

            chartBarValue.Container = this;
            chartBars.Add(chartBarValue);
        }

        public void AddRange(IEnumerable<ChartBarValue> chartBars)
        {
            foreach (ChartBarValue chartBar in chartBars)
                Add(chartBar);
        }

        public void Calculate()
        {
            MaxValue = chartBars
                .Select(x => x.MaxValue)
                .Max();

            foreach (ChartBarValue chartBar in chartBars)
                chartBar.Calculate();
        }

        public IEnumerator<ChartBarValue> GetEnumerator()
        {
            return chartBars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}