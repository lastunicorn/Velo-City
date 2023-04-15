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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.Cli.Application.PresentForecast;

public class Forecast
{
    private readonly IUnitOfWork unitOfWork;

    public DateTime? EndDate { get; set; }

    public uint AnalysisLookBack { get; set; }

    public List<int> ExcludedSprints { get; set; }

    public List<string> ExcludedTeamMembers { get; set; }

    public SprintList HistorySprints { get; private set; }

    public SprintList FutureSprints { get; private set; }

    public DateTime ActualStartDate => FutureSprints.FirstOrDefault()?.StartDate ?? DateTime.MinValue;

    public DateTime ActualEndDate => FutureSprints.LastOrDefault()?.EndDate ?? DateTime.MinValue;

    public HoursValue TotalWorkHours { get; private set; }

    public Velocity EstimatedVelocity { get; private set; }

    public StoryPoints EstimatedStoryPoints { get; private set; }

    public StoryPoints EstimatedStoryPointsWithVelocityPenalties { get; private set; }

    public List<SprintForecast> ForecastSprints { get; private set; }

    public Forecast(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Calculate()
    {
        Sprint referenceSprint = await GetReferenceSprint();

        HistorySprints = await RetrievePreviousSprints(referenceSprint);
        EstimatedVelocity = EstimateVelocity(HistorySprints);

        FutureSprints = await GenerateFutureSprints(referenceSprint);

        foreach (Sprint futureSprint in FutureSprints)
            futureSprint.ExcludedTeamMembers = ExcludedTeamMembers;

        ForecastSprints = FutureSprints
            .Select(x => new SprintForecast(x, EstimatedVelocity))
            .ToList();

        TotalWorkHours = FutureSprints.Sum(x => x.TotalWorkHours);

        EstimatedStoryPoints = TotalWorkHours * EstimatedVelocity;

        bool velocityPenaltiesExists = FutureSprints
            .SelectMany(x => x.GetVelocityPenalties())
            .Any();

        int? totalWorkHoursWithVelocityPenalties = FutureSprints
            .Sum(x => x.TotalWorkHoursWithVelocityPenalties);

        EstimatedStoryPointsWithVelocityPenalties = velocityPenaltiesExists
            ? totalWorkHoursWithVelocityPenalties * EstimatedVelocity
            : StoryPoints.Empty;
    }

    public async Task<Sprint> GetReferenceSprint()
    {
        Sprint currentSprint = await unitOfWork.SprintRepository.GetLastInProgress()
                               ?? await unitOfWork.SprintRepository.GetLastClosed();

        if (currentSprint == null)
            throw new NoSprintException();

        currentSprint.ExcludedTeamMembers = ExcludedTeamMembers;

        return currentSprint;
    }

    private async Task<SprintList> RetrievePreviousSprints(Sprint referenceSprint)
    {
        bool excludedSprintsExists = ExcludedSprints is { Count: > 0 };

        IEnumerable<Sprint> sprintEnumeration = excludedSprintsExists
            ? await unitOfWork.SprintRepository.GetClosedSprintsBefore(referenceSprint.Number, AnalysisLookBack, ExcludedSprints)
            : await unitOfWork.SprintRepository.GetClosedSprintsBefore(referenceSprint.Number, AnalysisLookBack);

        SprintList sprints = sprintEnumeration.ToSprintList();

        if (sprints.Count == 0)
            throw new Exception("There are no history sprints. The forecast calculation cannot proceed. History sprints are needed to calculate the estimated velocity.");

        foreach (Sprint sprint in sprints)
            sprint.ExcludedTeamMembers = ExcludedTeamMembers;

        return sprints;
    }

    private static Velocity EstimateVelocity(SprintList historySprints)
    {
        Velocity estimatedVelocity = historySprints.CalculateAverageVelocity();

        if (estimatedVelocity.IsEmpty)
            throw new Exception("Error calculating the estimated velocity.");

        return estimatedVelocity;
    }

    private async Task<SprintList> GenerateFutureSprints(Sprint referenceSprint)
    {
        const int defaultSprintSize = 14;

        DateTime calculatedStartDate = referenceSprint.EndDate.AddDays(1);
        DateTime calculatedEndDate = EndDate ?? referenceSprint.EndDate.AddDays(defaultSprintSize * 3);

        SprintFactory sprintFactory = new(unitOfWork);
        SprintsSpace sprintsSpace = new(sprintFactory)
        {
            DefaultSprintSize = defaultSprintSize,
            DateInterval = new DateInterval(calculatedStartDate, calculatedEndDate),
            ExistingSprints = (await unitOfWork.SprintRepository.Get(calculatedStartDate, calculatedEndDate))
                .ToList()
        };

        return sprintsSpace.ToSprintList();
    }
}