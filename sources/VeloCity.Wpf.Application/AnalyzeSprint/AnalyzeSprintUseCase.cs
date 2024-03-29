﻿// VeloCity
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

using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;
using DustInTheWind.VeloCity.Ports.SettingsAccess;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.AnalyzeSprint;

internal class AnalyzeSprintUseCase : IRequestHandler<AnalyzeSprintRequest, AnalyzeSprintResponse>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IConfig config;

    public AnalyzeSprintUseCase(IUnitOfWork unitOfWork, IConfig config)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public async Task<AnalyzeSprintResponse> Handle(AnalyzeSprintRequest request, CancellationToken cancellationToken)
    {
        request.Sprint.ExcludedTeamMembers = request.ExcludedTeamMembers;

        SprintList historySprints = await RetrievePreviousSprints(request);
        Velocity estimatedVelocity = historySprints.CalculateAverageVelocity();

        List<VelocityPenaltyInstance> velocityPenalties = request.Sprint.GetVelocityPenalties();
        HoursValue totalWorkHoursWithVelocityPenalties = request.Sprint.TotalWorkHoursWithVelocityPenalties;

        return new AnalyzeSprintResponse
        {
            HistorySprints = historySprints,
            EstimatedVelocity = estimatedVelocity,
            EstimatedStoryPoints = estimatedVelocity.IsEmpty
                ? StoryPoints.Empty
                : request.Sprint.TotalWorkHours * estimatedVelocity,
            VelocityPenalties = velocityPenalties,
            TotalWorkHoursWithVelocityPenalties = totalWorkHoursWithVelocityPenalties,
            EstimatedStoryPointsWithVelocityPenalties = estimatedVelocity.IsEmpty || !velocityPenalties.Any()
                ? StoryPoints.Empty
                : totalWorkHoursWithVelocityPenalties * estimatedVelocity
        };
    }

    private async Task<SprintList> RetrievePreviousSprints(AnalyzeSprintRequest request)
    {
        bool excludedSprintsExists = request.ExcludedSprints is { Count: > 0 };
        uint analysisLookBack = request.AnalysisLookBack ?? config.AnalysisLookBack;

        IEnumerable<Sprint> sprintsEnumeration = excludedSprintsExists
            ? await unitOfWork.SprintRepository.GetClosedSprintsBefore(request.Sprint.Number, analysisLookBack, request.ExcludedSprints)
            : await unitOfWork.SprintRepository.GetClosedSprintsBefore(request.Sprint.Number, analysisLookBack);

        List<Sprint> sprints = sprintsEnumeration.ToList();

        foreach (Sprint sprint in sprints)
            sprint.ExcludedTeamMembers = request.ExcludedTeamMembers;

        return sprints.ToSprintList();
    }
}