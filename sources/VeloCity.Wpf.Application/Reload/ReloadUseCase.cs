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

using DustInTheWind.VeloCity.Infrastructure;
using DustInTheWind.VeloCity.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.Reload;

public class ReloadUseCase : IRequestHandler<ReloadRequest, Unit>
{
    private readonly EventBus eventBus;
    private readonly IDataStorage dataStorage;

    public ReloadUseCase(EventBus eventBus, IDataStorage dataStorage)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.dataStorage = dataStorage ?? throw new ArgumentNullException(nameof(dataStorage));
    }

    public async Task<Unit> Handle(ReloadRequest request, CancellationToken cancellationToken)
    {
        bool success = ReloadData();

        if (success)
            await RaiseEvent(cancellationToken);

        return Unit.Value;
    }

    private bool ReloadData()
    {
        try
        {
            dataStorage.Reopen();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task RaiseEvent(CancellationToken cancellationToken)
    {
        ReloadEvent reloadEvent = new();
        await eventBus.Publish(reloadEvent, cancellationToken);
    }
}