﻿// VeloCity
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
using System.Runtime.Serialization;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Wpf.Application.CloseSprint;

[Serializable]
internal class InvalidSprintStateException : Exception
{
    public InvalidSprintStateException(int sprintNumber, SprintState sprintState)
        : base(BuildMessage(sprintNumber, sprintState))
    {
    }

    private static string BuildMessage(int sprintNumber, SprintState sprintState)
    {
        const string messageTemplate = "The sprint '{0}' is in an invalid state. Sprint state: '{1}'.";
        return string.Format(messageTemplate, sprintNumber, sprintState);
    }

    protected InvalidSprintStateException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}