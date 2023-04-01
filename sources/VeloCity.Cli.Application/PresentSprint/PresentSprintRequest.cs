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

using System.Collections.Generic;
using MediatR;

namespace DustInTheWind.VeloCity.Cli.Application.PresentSprint;

public class PresentSprintRequest : IRequest<PresentSprintResponse>
{
    public int? SprintNumber { get; set; }

    public List<int> ExcludedSprints { get; set; }

    public bool IncludeTeamDetails { get; set; }

    public List<string> ExcludedTeamMembers { get; set; }

    public uint? AnalysisLookBack { get; set; }
}