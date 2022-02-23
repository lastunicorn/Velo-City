using System;

namespace DustInTheWind.VeloCity.Application.PresentSprints
{
    public class SprintOverview
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TotalWorkHours { get; set; }

        public float EstimatedStoryPoints { get; set; }

        public float EstimatedVelocity { get; set; }

        public int CommitmentStoryPoints { get; set; }

        public int ActualStoryPoints { get; set; }

        public float ActualVelocity { get; set; }
    }
}