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

using DustInTheWind.ConsoleTools.Commando;
using DustInTheWind.VeloCity.Cli.Application.PresentSprints;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Sprints;

[Command("sprints", ShortDescription = "An overview of the last n sprints.", Order = 2)]
public class PresentSprintsCommand : ICommand
{
    private readonly IMediator mediator;

    [CommandParameter(DisplayName = "sprint count", Name = "count", ShortName = 'c', Order = 1, IsOptional = true)]
    public int? SprintCount { get; set; }

    public List<SprintOverview> SprintOverviews { get; private set; }

    public PresentSprintsCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        PresentSprintsRequest request = new()
        {
            Count = SprintCount
        };

        PresentSprintsResponse response = await mediator.Send(request);

        SprintOverviews = response.SprintOverviews;
    }
}