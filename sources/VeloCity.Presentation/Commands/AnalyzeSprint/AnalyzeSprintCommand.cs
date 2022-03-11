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
    public class AnalyzeSprintCommand : ICommand
    {
        private readonly IMediator mediator;

        public string SprintName { get; private set; }

        public SprintState SprintState { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public List<DateTime> WorkDays { get; private set; }

        public List<SprintMember> SprintMembers { get; private set; }

        public int TotalWorkHours { get; private set; }

        public float? EstimatedStoryPoints { get; private set; }

        public float? EstimatedVelocity { get; private set; }

        public int CommitmentStoryPoints { get; private set; }

        public int ActualStoryPoints { get; private set; }

        public float ActualVelocity { get; private set; }

        public int LookBackSprintCount { get; private set; }

        public List<int> PreviousSprints { get; private set; }

        public List<int> ExcludesSprints { get; private set; }

        public AnalyzeSprintCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(Arguments arguments)
        {
            AnalyzeSprintRequest request = new()
            {
                SprintNumber = GetSprintNumber(arguments),
                ExcludedSprints = GetExcludedSprintsList(arguments)
            };

            AnalyzeSprintResponse response = await mediator.Send(request);

            SprintName = response.SprintName;
            SprintState = response.SprintState;
            StartDate = response.StartDate;
            EndDate = response.EndDate;
            WorkDays = response.WorkDays;
            SprintMembers = response.SprintMembers;
            TotalWorkHours = response.TotalWorkHours;
            EstimatedStoryPoints = response.EstimatedStoryPoints;
            EstimatedVelocity = response.EstimatedVelocity;
            CommitmentStoryPoints = response.CommitmentStoryPoints;
            ActualStoryPoints = response.ActualStoryPoints;
            ActualVelocity = response.ActualVelocity;
            LookBackSprintCount = response.LookBackSprintCount;
            PreviousSprints = response.PreviousSprints;
            ExcludesSprints = response.ExcludesSprints;
        }

        private static int? GetSprintNumber(Arguments arguments)
        {
            Argument argument = arguments.GetOrdinal(1);
            string rawValue = argument?.Value;

            return rawValue == null
                ? null
                : int.Parse(rawValue);
        }

        private static List<int> GetExcludedSprintsList(Arguments arguments)
        {
            Argument argument = arguments["exclude"];

            return argument?.Value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
        }
    }
}