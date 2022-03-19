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
using System.Linq;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Application.AnalyzeSprint;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Presentation.Infrastructure;
using MediatR;

namespace DustInTheWind.VeloCity.Presentation.Commands.AnalyzeSprint
{
    [Command("sprint", ShortDescription = "Makes an analysis of the sprint and displays the result.", Order = 1)]
    [CommandUsage("sprint")]
    [CommandUsage("sprint [sprint-number]")]
    [CommandUsage("sprint [sprint-number] -exclude [sprint-number[,sprint-number[...]]]")]
    public class AnalyzeSprintCommand : ICommand
    {
        private readonly IMediator mediator;

        [CommandParameter(DisplayName = "Sprint Number", Order = 1, IsOptional = true)]
        public int? SprintNumber { get; set; }

        [CommandParameter(DisplayName = "Excluded Sprints", Name = "exclude", IsOptional = true)]
        public List<int> ExcludedSprints { get; set; }

        public List<SprintMember> SprintMembers { get; private set; }

        public SprintOverviewViewModel SprintOverviewViewModel { get; set; }

        public SprintCalendarViewModel SprintCalendarViewModel { get; set; }

        public AnalyzeSprintCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            AnalyzeSprintRequest request = new()
            {
                SprintNumber = SprintNumber,
                ExcludedSprints = ExcludedSprints
            };

            AnalyzeSprintResponse response = await mediator.Send(request);

            SprintOverviewViewModel = new SprintOverviewViewModel(response);
            SprintCalendarViewModel = new SprintCalendarViewModel(response);

            SprintMembers = response.SprintMembers;
        }
    }
}