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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace DustInTheWind.VeloCity.DataAccess;

public abstract class EntityCollection<TItem, TId> : Collection<TItem>
    where TItem : class
{
    protected EntityCollection()
    {
    }

    protected EntityCollection(IEnumerable<TItem> items)
        : base(items.ToList())
    {
    }

    protected abstract TId GetId(TItem item);

    protected abstract bool HasId(TItem item);

    protected abstract void SetId(TItem item, TId id);

    protected abstract TId GenerateNewId();

    protected override void InsertItem(int index, TItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        bool instanceAlreadyExist = Items.Any(x => ReferenceEquals(x, item));

        if (instanceAlreadyExist)
            return;

        bool itemHasId = HasId(item);
        if (!itemHasId)
        {
            TId newId = GenerateNewId();
            SetId(item, newId);
        }

        base.InsertItem(index, item);
    }

    protected override void SetItem(int index, TItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        bool instanceAlreadyExist = Items.Any(x => ReferenceEquals(x, item));

        if (instanceAlreadyExist)
            throw new DataException("The item already exists in the collection.");

        bool itemHasId = HasId(item);
        if (!itemHasId)
        {
            TId newId = GenerateNewId();
            SetId(item, newId);
        }

        base.SetItem(index, item);
    }

    public IEnumerable<TId> GetDuplicateIds()
    {
        return Items
            .GroupBy(GetId)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key);
    }
}