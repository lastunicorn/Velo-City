// VeloCity
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

namespace DustInTheWind.VeloCity.Wpf.Presentation.CustomControls;

internal static class WaitHandleExtensions
{
    public static Task<bool> ToTask(this WaitHandle waitHandle, int timeoutMilliseconds = -1)
    {
        if (waitHandle == null) throw new ArgumentNullException(nameof(waitHandle));

        TaskCompletionSource<bool> taskCompletionSource = new();

        RegisteredWaitHandle registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
                waitObject: waitHandle,
                callBack: (state, timedOut) =>
            {
                taskCompletionSource.TrySetResult(!timedOut);
            },
                state: null,
                timeout: TimeSpan.FromMilliseconds(timeoutMilliseconds),
                executeOnlyOnce: true);

        Task<bool> task = taskCompletionSource.Task;

        return task.ContinueWith(t =>
        {
            bool success = registeredWaitHandle.Unregister(null);

            try
            {
                return success && t.Result;
            }
            catch
            {
                return false;
            }
        });
    }
}