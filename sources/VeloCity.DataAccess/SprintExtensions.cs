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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.JsonFiles.JsonFileModel;

namespace DustInTheWind.VeloCity.DataAccess;

internal static class SprintExtensions
{
    public static IEnumerable<JSprint> ToJEntities(this IEnumerable<Sprint> sprints)
    {
        return sprints
            .Select(x => x.ToJEntity())
            .ToList();
    }

    public static JSprint ToJEntity(this Sprint sprint)
    {
        return new JSprint
        {
            Id = sprint.Id,
            Number = sprint.Number,
            Name = sprint.Title,
            StartDate = sprint.StartDate,
            EndDate = sprint.EndDate,
            Goal = sprint.Goal,
            CommitmentStoryPoints = sprint.CommitmentStoryPoints,
            ActualStoryPoints = sprint.ActualStoryPoints,
            State = sprint.State.ToJEntity(),
            Comments = sprint.Comments
        };
    }

    public static JSprintState ToJEntity(this SprintState sprintState)
    {
        return sprintState switch
        {
            SprintState.Unknown => JSprintState.Invlid,
            SprintState.New => JSprintState.New,
            SprintState.InProgress => JSprintState.InProgress,
            SprintState.Closed => JSprintState.Closed,
            _ => throw new ArgumentOutOfRangeException(nameof(sprintState), sprintState, null)
        };
    }

    public static IEnumerable<Sprint> ToEntities(this IEnumerable<JSprint> sprints)
    {
        if (sprints == null)
            return Enumerable.Empty<Sprint>();

        return sprints
            .Select(x => x.ToEntity());
    }

    public static Sprint ToEntity(this JSprint sprint)
    {
        return new Sprint
        {
            Id = sprint.Id,
            Number = sprint.Number,
            Title = sprint.Name,
            DateInterval = new DateInterval(sprint.StartDate, sprint.EndDate),
            Goal = sprint.Goal,
            CommitmentStoryPoints = sprint.CommitmentStoryPoints,
            ActualStoryPoints = sprint.ActualStoryPoints,
            State = sprint.State.ToEntity(),
            Comments = sprint.Comments
        };
    }

    public static SprintState ToEntity(this JSprintState sprintState)
    {
        return sprintState switch
        {
            JSprintState.Invlid => SprintState.Unknown,
            JSprintState.New => SprintState.New,
            JSprintState.InProgress => SprintState.InProgress,
            JSprintState.Closed => SprintState.Closed,
            _ => throw new ArgumentOutOfRangeException(nameof(sprintState), sprintState, null)
        };
    }

    public static SprintCollection ToSprintCollection(this IEnumerable<Sprint> sprints)
    {
        return new SprintCollection(sprints);
    }
}