﻿// Velo City
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

using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.Wpf.Application.PresentMainView
{
    public class SprintInfo
    {
        public int Id { get; }

        public string Name { get; }

        public int Number { get; }

        public DateInterval DateInterval { get; }
        
        public SprintState State { get; }

        public SprintInfo(Sprint sprint)
        {
            Id = sprint.Id;
            Name = sprint.Name;
            Number = sprint.Number;
            DateInterval = sprint.DateInterval;
            State = sprint.State;
        }
    }
}