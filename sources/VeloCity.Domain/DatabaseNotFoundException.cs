using System;

namespace DustInTheWind.VeloCity.Domain
{
    public class DatabaseNotFoundException : Exception
    {
        private const string DefaultMessage = "Could not open the database file.";

        public DatabaseNotFoundException()
            : base(DefaultMessage)
        {
        }

        public DatabaseNotFoundException(Exception innerException)
            : base(DefaultMessage, innerException)
        {
        }
    }
}