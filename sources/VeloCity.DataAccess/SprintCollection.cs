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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using DustInTheWind.VeloCity.Domain;

namespace DustInTheWind.VeloCity.DataAccess;

public class SprintCollection : EntityCollection<Sprint, int>
{
    public SprintCollection()
    {
    }

    public SprintCollection(IEnumerable<Sprint> sprints)
        : base(sprints.ToList())
    {
    }

    protected override int GetId(Sprint item)
    {
        return item.Id;
    }

    protected override bool HasId(Sprint item)
    {
        return item.Id != 0;
    }

    protected override void SetId(Sprint item, int id)
    {
        item.Id = id;
    }

    protected override int GenerateNewId()
    {
        IEnumerable<Sprint> sprintsOrderedById = Items.OrderBy(x => x.Id);
        IEnumerable<int> possibleIds = Enumerable.Range(1, int.MaxValue);

        using IEnumerator<Sprint> sprintEnumerator = sprintsOrderedById.GetEnumerator();
        using IEnumerator<int> possibleIdEnumerator = possibleIds.GetEnumerator();

        while (possibleIdEnumerator.MoveNext())
        {
            bool sprintExists = sprintEnumerator.MoveNext();

            if (!sprintExists)
                return possibleIdEnumerator.Current;

            if (possibleIdEnumerator.Current != sprintEnumerator.Current.Id)
                return possibleIdEnumerator.Current;
        }

        throw new DataException("Cannot generate new id. Collection is full.");
    }
}

// public class SprintCollection : Collection<Sprint>
// {
//     public SprintCollection()
//     {
//     }
//
//     public SprintCollection(IEnumerable<Sprint> sprints)
//         : base(sprints.ToList())
//     {
//     }
//
//     protected override void InsertItem(int index, Sprint sprint)
//     {
//         if (sprint == null) throw new ArgumentNullException(nameof(sprint));
//
//         bool instanceAlreadyExist = Items.Any(x => ReferenceEquals(x, sprint));
//
//         if (instanceAlreadyExist)
//             return;
//
//         if (sprint.Id == 0)
//             sprint.Id = GenerateNewId();
//
//         base.InsertItem(index, sprint);
//     }
//
//     protected override void SetItem(int index, Sprint sprint)
//     {
//         if (sprint == null) throw new ArgumentNullException(nameof(sprint));
//
//         bool instanceAlreadyExist = Items.Any(x => ReferenceEquals(x, sprint));
//
//         if (instanceAlreadyExist)
//             throw new DataException("The item already exists in the collection.");
//
//         if (sprint.Id == 0)
//             sprint.Id = GenerateNewId();
//
//         base.SetItem(index, sprint);
//     }
//
//     private int GenerateNewId()
//     {
//         IEnumerable<Sprint> sprintsOrderedById = Items.OrderBy(x => x.Id);
//         IEnumerable<int> possibleIds = Enumerable.Range(1, int.MaxValue);
//
//         using IEnumerator<Sprint> sprintEnumerator = sprintsOrderedById.GetEnumerator();
//         using IEnumerator<int> possibleIdEnumerator = possibleIds.GetEnumerator();
//
//         while (possibleIdEnumerator.MoveNext())
//         {
//             bool sprintExists = sprintEnumerator.MoveNext();
//
//             if (!sprintExists)
//                 return possibleIdEnumerator.Current;
//
//             if (possibleIdEnumerator.Current != sprintEnumerator.Current.Id)
//                 return possibleIdEnumerator.Current;
//         }
//
//         throw new DataException("Cannot generate new id. Collection is full.");
//     }
//
//     public IEnumerable<int> GetDuplicateIds()
//     {
//         return Items
//             .GroupBy(x => x.Id)
//             .Where(x => x.Count() > 1)
//             .Select(x => x.Key);
//     }
// }