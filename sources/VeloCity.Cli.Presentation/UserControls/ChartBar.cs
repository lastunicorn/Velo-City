// Velo City
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

namespace DustInTheWind.VeloCity.Cli.Presentation.UserControls
{
    internal class ChartBar
    {
        public int DefaultMaxDisplayLength { get; set; } = 24;

        public Chart Parent { get; set; }

        public int MaxValue { get; set; }

        public int Value { get; set; }

        public override string ToString()
        {
            if (MaxValue == 0)
                return string.Empty;

            int chartBarMaxDisplayLength = Parent == null
                ? DefaultMaxDisplayLength
                : (int)Math.Round((double)Parent.MaxDisplayLength * MaxValue / Parent.MaxValue);

            int chartBarValue = (int)Math.Round((float)Value * chartBarMaxDisplayLength / MaxValue);
            return new string('═', chartBarValue) + new string('-', chartBarMaxDisplayLength - chartBarValue);
        }
    }
}