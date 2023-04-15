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

using Autofac;
using DustInTheWind.VeloCity.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Bootstrapper;

internal class MediatrRequestBus : IRequestBus
{
    private readonly ILifetimeScope lifetimeScope;

    public MediatrRequestBus(ILifetimeScope lifetimeScope)
    {
        this.lifetimeScope = lifetimeScope ?? throw new ArgumentNullException(nameof(lifetimeScope));
    }

    public async Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default)
    {
        await using ILifetimeScope localLifetimeScope = lifetimeScope.BeginLifetimeScope();

        IMediator mediator = localLifetimeScope.Resolve<IMediator>();

        if (request is IRequest<TResponse> mediatorRequest)
            return await mediator.Send(mediatorRequest, cancellationToken);

        object rawResponse = await mediator.Send(request, cancellationToken);

        if (rawResponse is TResponse response)
            return response;

        Type responseType = typeof(TResponse);
        throw new Exception($"Response is not of type {responseType.FullName}");
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        await using ILifetimeScope localLifetimeScope = lifetimeScope.BeginLifetimeScope();

        IMediator mediator = localLifetimeScope.Resolve<IMediator>();

        if (request is IRequest mediatorRequest)
            await mediator.Send(mediatorRequest, cancellationToken);
        else
            await mediator.Send(request, cancellationToken);
    }
}