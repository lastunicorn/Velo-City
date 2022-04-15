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

namespace DustInTheWind.VeloCity.Domain
{
    public class SprintsSpace
    {
        private readonly SprintFactory sprintFactory;

        public DateInterval DateInterval { get; set; }

        public List<Sprint> ExistingSprints { get; set; }

        public int DefaultSprintSize { get; set; } = 14;

        public DateTime ActualStartDate { get; private set; }

        public DateTime ActualEndDate { get; private set; }

        public List<Sprint> AllSprints { get; set; }

        public SprintsSpace(SprintFactory sprintFactory)
        {
            this.sprintFactory = sprintFactory ?? throw new ArgumentNullException(nameof(sprintFactory));
        }

        public void GenerateMissingSprints()
        {
            if (DateInterval.IsZero)
                throw new Exception("The forecast interval cannot be zero.");

            DateTime startDate = DateInterval.StartDate ?? DateTime.MinValue;
            DateTime endDate = DateInterval.EndDate ?? DateTime.MaxValue;

            AllSprints = GenerateSprints(startDate, endDate).ToList();

            Sprint firstSprint = AllSprints.First();
            Sprint lastSprint = AllSprints.Last();

            ActualStartDate = firstSprint.StartDate;
            ActualEndDate = lastSprint.EndDate;
        }

        private IEnumerable<Sprint> GenerateSprints(DateTime startDate, DateTime endDate)
        {
            List<Sprint> orderedExistingSprints = ExistingSprints
                .OrderBy(x => x.StartDate)
                .ToList();

            DateTime currentDate = startDate;
            using IEnumerator<Sprint> existingSprintEnumerator = orderedExistingSprints.GetEnumerator();
            bool existsMoreSprints = existingSprintEnumerator.MoveNext();

            while (existsMoreSprints && (existingSprintEnumerator.Current == null || existingSprintEnumerator.Current.StartDate < currentDate))
            {
                existsMoreSprints = existingSprintEnumerator.MoveNext();
            }

            while (currentDate <= endDate)
            {
                if (existsMoreSprints)
                {
                    if (existingSprintEnumerator.Current.StartDate == currentDate)
                    {
                        yield return existingSprintEnumerator.Current;

                        if (existingSprintEnumerator.Current.EndDate == DateTime.MaxValue)
                            break;

                        currentDate = existingSprintEnumerator.Current.EndDate.AddDays(1);

                        while (existsMoreSprints && (existingSprintEnumerator.Current == null || existingSprintEnumerator.Current.StartDate < currentDate))
                        {
                            existsMoreSprints = existingSprintEnumerator.MoveNext();
                        }
                    }
                    else
                    {
                        DateTime sprintStartDate = currentDate;

                        int daysUntilNextSprint = (int)(existingSprintEnumerator.Current.StartDate - sprintStartDate).TotalDays;
                        int currentSprintSize = Math.Min(DefaultSprintSize, daysUntilNextSprint + 1);
                        DateTime sprintEndDate = sprintStartDate.AddDays(currentSprintSize - 1);

                        yield return sprintFactory.GenerateImaginarySprint(sprintStartDate, sprintEndDate);

                        if (sprintEndDate == DateTime.MaxValue)
                            break;

                        currentDate = sprintEndDate.AddDays(1);
                    }
                }
                else
                {
                    DateTime sprintStartDate = currentDate;

                    int daysUntilEndDate = (int)(endDate - sprintStartDate).TotalDays;
                    int currentSprintSize = Math.Min(DefaultSprintSize, daysUntilEndDate + 1);
                    DateTime sprintEndDate = sprintStartDate.AddDays(currentSprintSize - 1);

                    yield return sprintFactory.GenerateImaginarySprint(sprintStartDate, sprintEndDate);

                    if (sprintEndDate == DateTime.MaxValue)
                        break;

                    currentDate = sprintEndDate.AddDays(1);
                }
            }
        }
    }
}