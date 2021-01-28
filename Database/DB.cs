using log4net;
using System;
using System.Globalization;
using System.Data.SQLite;
using System.IO;

namespace Database
{
    public class DB : IDisposable
    {
        internal static ILog Log { get; private set; }
        private const string SETTINGS_ID = "1";
        private const string SETTINGS_TABLE = "Settings";
        bool disposed = false;
        private SQLiteConnection _db;
        private bool _readOnly;
        private string _dbName;
        private const int _version = 3;
        int _nConnections;

        public DB(string dbName, bool readOnly = false)
        {

            _dbName = GetDbPath(dbName);
            _nConnections = 0;
            _readOnly = readOnly;
        }

        public static string GetDbPath(string dbName)
        {
            const string PlugName = "OpenALPR";
            string mappingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), PlugName, "Database");

            if (!Directory.Exists(mappingPath))
            {
                Directory.CreateDirectory(mappingPath);
                Helper.SetDirectoryNetworkServiceAccessControl(mappingPath);
            }

            return $"{mappingPath}\\{dbName}.db";
        }

        public void Connect(string file, bool readOnly = false)
        {
            if (_nConnections > 0)
            {
                _nConnections -= 1;
            }

            string connectionString = $"Data Source={file};Version={_version};";
            if (readOnly)
                connectionString += "mode=ReadOnly";
            _db = new SQLiteConnection(connectionString, true);
            ++_nConnections;
            _db.Open();

            if (!readOnly)
            {
                CreateSettingsTable();
            }
        }




        public Settings GetSettings()
        {

            Settings settings = null;

            try
            {
                Connect(_dbName, _readOnly);

                string sql = $"SELECT * FROM [{SETTINGS_TABLE}] WHERE Id=1";

                SQLiteCommand command = new SQLiteCommand(sql, _db);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime d = DateTime.Now;
                    String s = Convert.ToString(reader["Created"]);
                    DateTime.TryParseExact(s, "yyyy-MM-dd HH.mm.ss", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out d);

                    settings = new Settings()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Created = d, //Convert.ToDateTime(reader["Created"]),
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
                    };
                }
            }
            catch (Exception ex)
            {
                /*
                Console.WriteLine("\nMessage ---\n{0}", ex.Message);
                Console.WriteLine(
                    "\nHelpLink ---\n{0}", ex.HelpLink);
                Console.WriteLine("\nSource ---\n{0}", ex.Source);
                Console.WriteLine(
                    "\nStackTrace ---\n{0}", ex.StackTrace);
                Console.WriteLine(
                    "\nTargetSite ---\n{0}", ex.TargetSite);

                throw new Exception(ex.Message);
                */
            }
            finally
            {
                Disconnect();
            }


            if (settings == null)
                settings = Defaults();

            return settings;
        }

        private void CreateSettingsTable()
        {
            string sql = @"'Created'	TEXT,
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
                                        'UseUTC'	INTEGER";

            string query = $"CREATE TABLE  IF NOT EXISTS '{SETTINGS_TABLE}' (" + $"'Id'	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, {sql});";
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

        public bool SaveSettings(Settings settings)
        {
            if (_readOnly)
            {
                Console.WriteLine("Attempting to write to a read only database");
                return false;
            }

            Connect(_dbName, _readOnly);

            // Use UPSERT to ensure that only one item is saved at any time:
            // https://www.sqlite.org/draft/lang_UPSERT.html

            string insert_query = $"INSERT INTO {SETTINGS_TABLE} (Id, Created, OpenALPRServerUrl, MilestoneServerName, MilestoneUserName, MilestonePassword, EventExpireAfterDays, EpochStartSecondsBefore, EpochEndSecondsAfter, AddBookmarks, AutoMapping, ServicePort, ClientSettingsProviderServiceUri, UseUTC) VALUES ('{SETTINGS_ID}', '{settings.Created}', '{settings.OpenALPRServerUrl}', '{settings.MilestoneServerName}', '{settings.MilestoneUserName}', '{settings.MilestonePassword}', {settings.EventExpireAfterDays}, {settings.EpochStartSecondsBefore}, {settings.EpochEndSecondsAfter}, {Convert.ToInt32(settings.AddBookmarks)}, {Convert.ToInt32(settings.AutoMapping)}, {settings.ServicePort}, '{settings.ClientSettingsProviderServiceUri}', {Convert.ToInt32(settings.UseUTC)})";
            string update_query = $"UPDATE SET OpenALPRServerUrl = '{ settings.OpenALPRServerUrl }', MilestoneServerName = '{ settings.MilestoneServerName }', MilestoneUserName = '{ settings.MilestoneUserName }', MilestonePassword = '{ settings.MilestonePassword }', EventExpireAfterDays = { settings.EventExpireAfterDays }, EpochStartSecondsBefore = { settings.EpochStartSecondsBefore }, EpochEndSecondsAfter = { settings.EpochEndSecondsAfter }, AddBookmarks = { Convert.ToInt32(settings.AddBookmarks) }, AutoMapping = { Convert.ToInt32(settings.AutoMapping) }, ServicePort = { settings.ServicePort }, ClientSettingsProviderServiceUri = '{ settings.ClientSettingsProviderServiceUri }', UseUTC = { Convert.ToInt32(settings.UseUTC) } WHERE Id = { SETTINGS_ID } ";

            string upsert_query = $"{insert_query} on conflict(Id) do {update_query}";
            SQLiteCommand insertSQL = new SQLiteCommand(upsert_query, _db);
            try
            {
                insertSQL.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Disconnect();
            }

            return true;
        }

        private Settings Defaults()
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
                UseUTC = false
            };
            return settings;
        }

        private void Disconnect()
        {
            _db.Close();
            --_nConnections;
            
        }

        public void Dispose()
        {
            while (_nConnections > 0)
                Disconnect();

            // Fix for hacky Sqlite behavior described here:
            // https://stackoverflow.com/questions/8511901/system-data-sqlite-close-not-releasing-database-file
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
            Dispose();
        }
    }
}
