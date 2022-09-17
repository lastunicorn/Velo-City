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
using System.Collections.Generic;
using System.Threading;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors
{
    internal class ConcurrentDictionary<TKey, TValue> : IDisposable
    {
        private readonly ReaderWriterLockSlim readerWriterLockSlim = new(LockRecursionPolicy.SupportsRecursion);
        private readonly Dictionary<TKey, TValue> dictionary = new();

        public TValue Get(TKey key)
        {
            readerWriterLockSlim.EnterReadLock();

            try
            {
                return dictionary.ContainsKey(key)
                    ? dictionary[key]
                    : default;
            }
            finally
            {
                if (readerWriterLockSlim.IsReadLockHeld)
                    readerWriterLockSlim.ExitReadLock();
            }
        }

        public void Set(TKey key, TValue value)
        {
            readerWriterLockSlim.EnterWriteLock();

            try
            {
                if (dictionary.ContainsKey(key))
                    dictionary[key] = value;
                else
                    dictionary.Add(key, value);
            }
            finally
            {
                if (readerWriterLockSlim.IsWriteLockHeld)
                    readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Remove(TKey key)
        {
            readerWriterLockSlim.EnterWriteLock();

            try
            {
                if (dictionary.ContainsKey(key))
                    dictionary.Remove(key);
            }
            finally
            {
                if (readerWriterLockSlim.IsWriteLockHeld)
                    readerWriterLockSlim.ExitWriteLock();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            readerWriterLockSlim?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}