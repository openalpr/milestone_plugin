using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DB : IDisposable
    {
        internal static ILog Log { get; private set; }
        bool disposed = false;
        private SQLiteConnection _db;
        private string _dbName;
        private int _version;

        public DB(string dbName, int version)
        {
            _dbName = $"{DatabaseDefinition.applicationPath}\\{dbName}.db";
            _version = version;
            Connect(_dbName);
        }

        public void Connect(string file)
        {
            string connnectionString = $"Data Source={file};Version={_version};";
            _db = new SQLiteConnection(connnectionString, true);
            _db.Open();
        }

        public void CreateTable(string table)
        {
            if (Check(table) == 0)
                CreateSettingsTable(table, @"'Created'	TEXT,
                                            'OpenALPRServerUrl'	TEXT,
	                                        'MilestoneServerName'	TEXT,
	                                        'MilestoneUserName'	TEXT,
	                                        'MilestonePassword'	TEXT,
	                                        'EventExpireAfterDays'	INTEGER,
	                                        'EpochStartSecondsBefore'	INTEGER,
	                                        'EpochEndSecondsAfter'	INTEGER,
	                                        'AddBookmarks'	INTEGER,
	                                        'AutoMapping'	INTEGER,
	                                        'ServicePort'	INTEGER,
	                                        'ClientSettingsProviderServiceUri'	TEXT,
                                            'UseUTC'	INTEGER");
        }

        private int Check(string table)
        {
            string sql = $"SELECT count(name) as Count FROM sqlite_master WHERE type='table' AND name='{table}'";
            int count = 0;
            SQLiteCommand command = new SQLiteCommand(sql, _db);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                count = Convert.ToInt32(reader["Count"]);
                return count;
            }

            return count;
        }

        public List<Settings> GetSettings(string table)
        {
            string sql = $"SELECT * FROM [{table}]";

            SQLiteCommand command = new SQLiteCommand(sql, _db);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Settings> settings = new List<Settings>();
            while (reader.Read())
            {
                try
                {
                    settings.Add(new Settings()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Created = Convert.ToDateTime(reader["Created"]),
                        AddBookmarks = Convert.ToBoolean(reader["AddBookmarks"]),
                        AutoMapping = Convert.ToBoolean(reader["AddBookmarks"]),
                        EpochEndSecondsAfter = Convert.ToInt32(reader["EpochEndSecondsAfter"]),
                        EpochStartSecondsBefore = Convert.ToInt32(reader["EpochStartSecondsBefore"]),
                        EventExpireAfterDays = Convert.ToInt32(reader["EventExpireAfterDays"]),
                        ServicePort = Convert.ToInt32(reader["ServicePort"]),
                        ClientSettingsProviderServiceUri = Convert.ToString(reader["ClientSettingsProviderServiceUri"]),
                        MilestonePassword = Convert.ToString(reader["MilestonePassword"]),
                        MilestoneServerName = Convert.ToString(reader["MilestoneServerName"]),
                        MilestoneUserName = Convert.ToString(reader["MilestoneUserName"]),
                        OpenALPRServerUrl = Convert.ToString(reader["OpenALPRServerUrl"]),
                        UseUTC = Convert.ToBoolean(reader["UseUTC"])
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            return settings;
        }

        private void CreateSettingsTable(string table, string sql)
        {
            string query = $"CREATE TABLE '{table}' (" + $"'Id'	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, {sql});";
            SQLiteCommand insertSQL = new SQLiteCommand(query, _db);
            try
            {
                insertSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveSettings(string table, Settings settings)
        {
            string query = $"INSERT INTO {table} (Created, OpenALPRServerUrl, MilestoneServerName, MilestoneUserName, MilestonePassword, EventExpireAfterDays, EpochStartSecondsBefore, EpochEndSecondsAfter, AddBookmarks, AutoMapping, ServicePort, ClientSettingsProviderServiceUri, UseUTC) VALUES ('{settings.Created}', '{settings.OpenALPRServerUrl}', '{settings.MilestoneServerName}', '{settings.MilestoneUserName}', '{settings.MilestonePassword}', {settings.EventExpireAfterDays}, {settings.EpochStartSecondsBefore}, {settings.EpochEndSecondsAfter}, {Convert.ToInt32(settings.AddBookmarks)}, {Convert.ToInt32(settings.AutoMapping)}, {settings.ServicePort}, '{settings.ClientSettingsProviderServiceUri}', {Convert.ToInt32(settings.UseUTC)});";
            SQLiteCommand insertSQL = new SQLiteCommand(query, _db);
            try
            {
                insertSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateSettings(string table, Settings settings)
        {
            string query = $"UPDATE {table} SET OpenALPRServerUrl = '{ settings.OpenALPRServerUrl }', MilestoneServerName = '{ settings.MilestoneServerName }', MilestoneUserName = '{ settings.MilestoneUserName }', MilestonePassword = '{ settings.MilestonePassword }', EventExpireAfterDays = { settings.EventExpireAfterDays }, EpochStartSecondsBefore = { settings.EpochStartSecondsBefore }, EpochEndSecondsAfter = { settings.EpochEndSecondsAfter }, AddBookmarks = { Convert.ToInt32(settings.AddBookmarks) }, AutoMapping = { Convert.ToInt32(settings.AutoMapping) }, ServicePort = { settings.ServicePort }, ClientSettingsProviderServiceUri = '{ settings.ClientSettingsProviderServiceUri }', UseUTC = { Convert.ToInt32(settings.UseUTC) } WHERE Id = { settings.Id } ";
            SQLiteCommand updateSQL = new SQLiteCommand(query, _db);
            try
            {
                updateSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Settings Defaults()
        {
            Settings settings = new Settings() {
                OpenALPRServerUrl = "http://localhost:48125/",
                MilestoneServerName = "http://localhost:80/",
                EventExpireAfterDays = 7,
                EpochStartSecondsBefore = 3,
                EpochEndSecondsAfter = 3,
                AddBookmarks = true,
                AutoMapping = true,
                ServicePort = 22019,
                UseUTC = true
            };
            return settings;
        }

        public void Dispose()
        {
            _db.Close();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
            }

            disposed = true;
        }

        ~DB()
        {
            Dispose(false);
        }
    }
}
