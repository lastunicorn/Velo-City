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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.JsonFiles;

namespace DustInTheWind.VeloCity.DataAccess;

internal static class VelocityPenaltyExtensions
{
    public static IEnumerable<JVelocityPenalty> ToJEntities(this IEnumerable<VelocityPenalty> velocityPenalties)
    {
        return velocityPenalties?
            .Select(x => x.ToJEntity());
    }

    public static JVelocityPenalty ToJEntity(this VelocityPenalty velocityPenalty)
    {
        return new JVelocityPenalty
        {
            SprintId = velocityPenalty.Sprint.Id,
            Value = velocityPenalty.Value,
            Duration = velocityPenalty.Duration <= 1
                ? null
                : velocityPenalty.Duration,
            Comments = velocityPenalty.Comments
        };
    }

    public static IEnumerable<VelocityPenalty> ToEntities(this IEnumerable<JVelocityPenalty> velocityPenalties, VeloCityDbContext dbContext)
    {
        return velocityPenalties?
            .Select(x => x.ToEntity(dbContext));
    }

    public static VelocityPenalty ToEntity(this JVelocityPenalty velocityPenalty, VeloCityDbContext dbContext)
    {
        return new VelocityPenalty
        {
            Sprint = dbContext.Sprints.FirstOrDefault(x => x.Id == velocityPenalty.SprintId),
            Value = velocityPenalty.Value,
            Duration = velocityPenalty.Duration ?? 1,
            Comments = velocityPenalty.Comments
        };
    }
}