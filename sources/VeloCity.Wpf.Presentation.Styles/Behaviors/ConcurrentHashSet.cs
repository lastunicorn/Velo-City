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
using System.Threading;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles.Behaviors
{
    public class ConcurrentHashSet<T> : IDisposable
    {
        private readonly ReaderWriterLockSlim readerWriterLockSlim = new(LockRecursionPolicy.SupportsRecursion);
        private readonly HashSet<T> hashSet = new();
        
        public bool Add(T item)
        {
            readerWriterLockSlim.EnterWriteLock();

            try
            {
                return hashSet.Add(item);
            }
            finally
            {
                if (readerWriterLockSlim.IsWriteLockHeld)
                    readerWriterLockSlim.ExitWriteLock();
            }
        }

        public void Clear()
        {
            readerWriterLockSlim.EnterWriteLock();

            try
            {
                hashSet.Clear();
            }
            finally
            {
                if (readerWriterLockSlim.IsWriteLockHeld)
                    readerWriterLockSlim.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            readerWriterLockSlim.EnterReadLock();

            try
            {
                return hashSet.Contains(item);
            }
            finally
            {
                if (readerWriterLockSlim.IsReadLockHeld)
                    readerWriterLockSlim.ExitReadLock();
            }
        }

        public bool Remove(T item)
        {
            readerWriterLockSlim.EnterWriteLock();

            try
            {
                return hashSet.Remove(item);
            }
            finally
            {
                if (readerWriterLockSlim.IsWriteLockHeld)
                    readerWriterLockSlim.ExitWriteLock();
            }
        }

        public int Count
        {
            get
            {
                readerWriterLockSlim.EnterReadLock();

                try
                {
                    return hashSet.Count;
                }
                finally
                {
                    if (readerWriterLockSlim.IsReadLockHeld)
                        readerWriterLockSlim.ExitReadLock();
                }
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