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
using System.Linq;

namespace DustInTheWind.VeloCity.Domain
{
    public class VacationCollection : Collection<Vacation>
    {
        public event EventHandler Changed;

        public VacationCollection()
        {
        }

        public VacationCollection(IEnumerable<Vacation> vacations)
        {
            if (vacations == null) throw new ArgumentNullException(nameof(vacations));

            IEnumerable<Vacation> nonNullVacations = vacations.Where(x => x != null);

            foreach (Vacation vacation in nonNullVacations)
            {
                Items.Add(vacation);
                vacation.Changed += HandleVacationChanged;
            }
        }

        protected override void InsertItem(int index, Vacation item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            base.InsertItem(index, item);

            item.Changed += HandleVacationChanged;

            OnChanged();
        }

        protected override void RemoveItem(int index)
        {
            Vacation vacation = Items[index];
            vacation.Changed -= HandleVacationChanged;

            base.RemoveItem(index);

            OnChanged();
        }

        protected override void SetItem(int index, Vacation item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Vacation vacation = Items[index];
            vacation.Changed -= HandleVacationChanged;

            base.SetItem(index, item);

            item.Changed += HandleVacationChanged;

            OnChanged();
        }

        protected override void ClearItems()
        {
            foreach (Vacation vacation in Items)
                vacation.Changed -= HandleVacationChanged;

            base.ClearItems();

            OnChanged();
        }

        private void HandleVacationChanged(object sender, EventArgs e)
        {
            OnChanged();
        }

        public IEnumerable<Vacation> GetVacationsFor(DateTime date)
        {
            return Items.Where(x => x.Match(date));
        }

        protected virtual void OnChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}