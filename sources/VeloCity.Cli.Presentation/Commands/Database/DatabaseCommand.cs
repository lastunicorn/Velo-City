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
using DustInTheWind.VeloCity.Cli.Application.OpenDatabase;
using DustInTheWind.VeloCity.Domain.DatabaseEditing;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Presentation.Commands.Database;

[Command("db", ShortDescription = "Opens the database for editing.", Order = 101)]
public class DatabaseCommand : ICommand
{
    private readonly IMediator mediator;

    public string DatabaseFilePath { get; private set; }

    public DatabaseEditorType DatabaseEditorType { get; private set; }

    public DatabaseCommand(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task Execute()
    {
        OpenDatabaseRequest request = new();
        OpenDatabaseResponse response = await mediator.Send(request);

        DatabaseFilePath = response.DatabaseFilePath;
        DatabaseEditorType = response.DatabaseEditorType;
    }
}