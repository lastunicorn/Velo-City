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

namespace DustInTheWind.VeloCity.Infrastructure;

public class EventBus
{
    private readonly Dictionary<Type, List<object>> subscribersByEvent = new();

    public void Subscribe<TEvent>(Func<TEvent, CancellationToken, Task> action)
    {
        List<object> actions = GetBucket<TEvent>() ?? CreateBucket<TEvent>();
        actions.Add(action);
    }

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
        List<object> bucket = GetBucket<TEvent>();

        if (bucket == null)
            return;

        IEnumerable<Func<TEvent, CancellationToken, Task>> actions = bucket.Cast<Func<TEvent, CancellationToken, Task>>();

        foreach (Func<TEvent, CancellationToken, Task> action in actions)
            await action(@event, cancellationToken);
    }

    private List<object> GetBucket<TEvent>()
    {
        return subscribersByEvent.ContainsKey(typeof(TEvent))
            ? subscribersByEvent[typeof(TEvent)]
            : null;
    }

    private List<object> CreateBucket<TEvent>()
    {
        List<object> actions = new();
        subscribersByEvent.Add(typeof(TEvent), actions);
        return actions;
    }
}