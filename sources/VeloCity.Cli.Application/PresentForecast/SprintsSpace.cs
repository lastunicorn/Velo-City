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

using System.Collections;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Domain.SprintModel;

namespace DustInTheWind.VeloCity.Cli.Application.PresentForecast;

public class SprintsSpace : IEnumerable<Sprint>
{
    private readonly SprintFactory sprintFactory;

    public DateInterval DateInterval { get; set; }

    public List<Sprint> ExistingSprints { get; set; }

    public int DefaultSprintSize { get; set; } = 14;

    public SprintsSpace(SprintFactory sprintFactory)
    {
        this.sprintFactory = sprintFactory ?? throw new ArgumentNullException(nameof(sprintFactory));
    }

    public IEnumerator<Sprint> GetEnumerator()
    {
        return new SprintEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class SprintEnumerator : IEnumerator<Sprint>
    {
        private readonly SprintsSpace sprintsSpace;

        private bool isFirstCall = true;
        private int lastSprintNumber;
        private DateTime nextStartDate;
        private DateTime endDate;
        private IEnumerator<Sprint> existingSprintsEnumerator;
        private bool moreSprintsExist;
        private Sprint existingSprint;

        public Sprint Current { get; private set; }

        object IEnumerator.Current => Current;

        public SprintEnumerator(SprintsSpace sprintsSpace)
        {
            this.sprintsSpace = sprintsSpace ?? throw new ArgumentNullException(nameof(sprintsSpace));
        }

        public bool MoveNext()
        {
            if (isFirstCall)
            {
                Initialize();
                isFirstCall = false;
            }

            if (nextStartDate > endDate)
                return false;

            if (moreSprintsExist)
            {
                if (existingSprint.StartDate == nextStartDate)
                {
                    PromoteCurrentExistingSprint();
                    MoveToNextExistingSprint();
                    return true;
                }

                DateTime maxEndDate = existingSprint.StartDate.AddDays(-1);
                PromoteNewImaginarySprint(maxEndDate).Wait();

                return true;
            }

            PromoteNewImaginarySprint(endDate).Wait();
            return true;
        }

        private void Initialize()
        {
            lastSprintNumber = 0;

            nextStartDate = sprintsSpace.DateInterval.StartDate ?? DateTime.MinValue;
            endDate = sprintsSpace.DateInterval.EndDate ?? DateTime.MaxValue;

            existingSprintsEnumerator = sprintsSpace.ExistingSprints
                .OrderBy(x => x.StartDate)
                .GetEnumerator();

            moreSprintsExist = true;
            existingSprint = null;

            MoveToNextExistingSprint();
        }

        private void MoveToNextExistingSprint()
        {
            moreSprintsExist = existingSprintsEnumerator.MoveNext();
            existingSprint = existingSprintsEnumerator.Current;

            while (moreSprintsExist && (existingSprint == null || existingSprint.StartDate < nextStartDate))
            {
                moreSprintsExist = existingSprintsEnumerator.MoveNext();
                existingSprint = existingSprintsEnumerator.Current;
            }
        }

        private void PromoteCurrentExistingSprint()
        {
            lastSprintNumber = existingSprint.Number;
            Current = existingSprint;

            nextStartDate = existingSprint.EndDate.AddDays(1);
        }

        private async Task PromoteNewImaginarySprint(DateTime maxEndDate)
        {
            int daysUntilNextSprint = (int)(maxEndDate - nextStartDate).TotalDays;
            int currentSprintSize = Math.Min(sprintsSpace.DefaultSprintSize, daysUntilNextSprint + 1);
            DateTime sprintEndDate = nextStartDate.AddDays(currentSprintSize - 1);

            Sprint nextSprint = await sprintsSpace.sprintFactory.GenerateImaginarySprint(nextStartDate, sprintEndDate);
            lastSprintNumber++;
            nextSprint.Number = lastSprintNumber;
            nextSprint.Title = $"Sprint {lastSprintNumber} (Presumed)";
            Current = nextSprint;

            nextStartDate = sprintEndDate.AddDays(1);
        }

        public void Reset()
        {
            Initialize();
        }

        public void Dispose()
        {
            existingSprintsEnumerator?.Dispose();
        }
    }
}