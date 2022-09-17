using System.Collections.Generic;

namespace DustInTheWind.VeloCity.Wpf.Presentation.SprintsArea.SprintOverview
{
    internal class EstimatedVelocityInfo : InfoBase
    {
        public List<int> PreviousSprintNumbers { get; set; }

        protected override IEnumerable<string> BuildMessage()
        {
            string previousSprints = string.Join(", ", PreviousSprintNumbers);
            yield return $"The average velocity calculated using the last {PreviousSprintNumbers.Count} closed sprints: {previousSprints}";
        }
    }
}