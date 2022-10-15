using System;
using System.Collections.Generic;
using DustInTheWind.VeloCity.Domain;
using DustInTheWind.VeloCity.Ports.DataAccess;

namespace DustInTheWind.VeloCity.JsonFiles
{
    public class JsonDatabase : IDataStorage
    {
        private JsonDatabaseFile jsonDatabaseFile;

        public Guid InstanceId { get; private set; }

        public DatabaseState State { get; private set; }

        public string PersistenceLocation { get; set; }

        public DataAccessException LastError { get; private set; }

        public WarningException LastWarning { get; private set; }

        public List<JTeamMember> TeamMembers { get; set; } = new();

        public List<JOfficialHoliday> OfficialHolidays { get; set; } = new();

        public List<JSprint> Sprints { get; set; } = new();

        public event EventHandler Opened;

        /// <exception cref="DataAccessException">In case the database could not be opened.</exception>
        public void Open()
        {
            if (State == DatabaseState.Opened)
                return;

            if (State != DatabaseState.Closed)
                CloseInternal();

            try
            {
                jsonDatabaseFile = new JsonDatabaseFile(PersistenceLocation);
                jsonDatabaseFile.Open();

                LoadAllData();

                State = DatabaseState.Opened;
                InstanceId = Guid.NewGuid();

                OnOpened();
            }
            catch (DataAccessException ex)
            {
                LastError = ex;
                State = DatabaseState.Error;
                throw;
            }
            catch (Exception ex)
            {
                DataAccessException dataAccessException = new(ex);
                LastError = dataAccessException;
                State = DatabaseState.Error;

                throw dataAccessException;
            }
            finally
            {
                LastWarning = jsonDatabaseFile.LastWarning;
            }
        }

        private void LoadAllData()
        {
            Sprints = jsonDatabaseFile.Document.Sprints;
            TeamMembers = jsonDatabaseFile.Document.TeamMembers;
            OfficialHolidays = jsonDatabaseFile.Document.OfficialHolidays;
        }

        public void Close()
        {
            CloseInternal();
        }

        private void CloseInternal()
        {
            TeamMembers.Clear();
            OfficialHolidays.Clear();
            Sprints.Clear();

            LastError = null;
            LastWarning = null;

            InstanceId = Guid.Empty;

            State = DatabaseState.Closed;
        }

        public void Persist()
        {
            if (State != DatabaseState.Opened)
                throw new DataAccessException("The database is not opened.");

            jsonDatabaseFile.Document.Sprints = Sprints;
            jsonDatabaseFile.Document.TeamMembers = TeamMembers;
            jsonDatabaseFile.Document.OfficialHolidays = OfficialHolidays;

            jsonDatabaseFile.Save();
        }

        protected virtual void OnOpened()
        {
            Opened?.Invoke(this, EventArgs.Empty);
        }

        public void Reopen()
        {
            Close();
            Open();
        }
    }
}