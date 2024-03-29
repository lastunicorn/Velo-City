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

using System.ComponentModel;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprint.SprintOverview;

public class SprintStateViewModel
{
    private readonly SprintState sprintState;

    public SprintStateViewModel(SprintState sprintState)
    {
        if (!Enum.IsDefined(typeof(SprintState), sprintState)) throw new InvalidEnumArgumentException(nameof(sprintState), (int)sprintState, typeof(SprintState));
        this.sprintState = sprintState;
    }

    public override string ToString()
    {
        return sprintState switch
        {
            SprintState.Unknown => "unknown",
            SprintState.New => "new",
            SprintState.InProgress => "in progress",
            SprintState.Closed => "closed",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}