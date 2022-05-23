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
using DustInTheWind.ConsoleTools.Controls.Tables;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.Configuring;

namespace DustInTheWind.VeloCity.Presentation
{
    public class DataGridFactory
    {
        private readonly IConfig config;

        public DataGridFactory(IConfig config)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public DataGrid Create()
        {
            return new DataGrid
            {
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.DarkGray
                },
                Margin = "0 1 0 0",
                Border =
                {
                    Template = DecideBorderTemplate()
                }
            };
        }

        private BorderTemplate DecideBorderTemplate()
        {
            switch (config.DataGridStyle)
            {
                case DataGridStyle.PlusMinus:
                    return BorderTemplate.PlusMinusBorderTemplate;

                case DataGridStyle.SingleLine:
                    return BorderTemplate.SingleLineBorderTemplate;

                case DataGridStyle.DoubleLine:
                    return BorderTemplate.DoubleLineBorderTemplate;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}