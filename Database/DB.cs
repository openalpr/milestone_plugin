using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DB : IDisposable
    {
        bool disposed = false;
        private SQLiteConnection _db;
        private string _dbName;
        private int _version;

        public DB(string path, string dbName, int version)
        {
            _dbName = $"{path}\\{dbName}.db";
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
	                                        'ClientSettingsProviderServiceUri'	TEXT");
        }

        private int Check(string table)
        {
            string sql = $"SELECT count(name) as Count FROM sqlite_master WHERE type='table' AND name='{table}'";

            SQLiteCommand command = new SQLiteCommand(sql, _db);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                return Convert.ToInt32(reader["Count"]);
            }

            return 0;
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
                        OpenALPRServerUrl = Convert.ToString(reader["OpenALPRServerUrl"])
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
            string query = $"INSERT INTO {table} (Created, OpenALPRServerUrl, MilestoneServerName, MilestoneUserName, MilestonePassword, EventExpireAfterDays, EpochStartSecondsBefore, EpochEndSecondsAfter, AddBookmarks, AutoMapping, ServicePort, ClientSettingsProviderServiceUri) VALUES ('{settings.Created}', '{settings.OpenALPRServerUrl}', '{settings.MilestoneServerName}', '{settings.MilestoneUserName}', '{settings.MilestonePassword}', {settings.EventExpireAfterDays}, {settings.EpochStartSecondsBefore}, {settings.EpochEndSecondsAfter}, {Convert.ToInt32(settings.AddBookmarks)}, {Convert.ToInt32(settings.AutoMapping)}, {settings.ServicePort}, '{settings.ClientSettingsProviderServiceUri}');";
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

        public void Dispose()
        {
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
