using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.IO;

namespace DatabaseTest
{
    [TestClass]
    public class UnitTest1
    {
        const string UNIT_TEST_DB_NAME = "openalpr_milestone_unittest";

        [ClassInitialize]
        public static void RunOnceSetup(TestContext context)
        {
            delete_db();
        }

        private static void delete_db()
        {
            string db_path = DB.GetDbPath(UNIT_TEST_DB_NAME);
            if (File.Exists(db_path))
            {
                File.Delete(db_path);
            }
        }

        [TestMethod]
        public void TestDatabaseCreate()
        {
            using (DB db = new DB(UNIT_TEST_DB_NAME))
            {
                Settings settings = db.GetSettings();

                // Check default setting is retrieved
                Assert.IsTrue(settings.ServicePort == 22019);

                settings.ServicePort = 1234;
                bool success = db.SaveSettings(settings);
                Assert.IsTrue(success = true);

                Assert.IsTrue(db.GetSettings().ServicePort == 1234);

                settings.ServicePort = 5555;
                Assert.IsTrue(db.SaveSettings(settings));
                Assert.IsTrue(db.GetSettings().ServicePort == 5555);
            }
        }

        [TestMethod]
        public void TestReadOnlyBehavior()
        {
            delete_db();

            // Setup a readonly database, then do a write inside of it.  This should work.

            using (DB db_readonly = new DB(UNIT_TEST_DB_NAME, true))
            {

                Assert.IsTrue(db_readonly.GetSettings().ServicePort == 22019);
                Assert.IsFalse(db_readonly.SaveSettings(db_readonly.GetSettings()));

                using (DB db = new DB(UNIT_TEST_DB_NAME))
                {
                    Settings settings = db.GetSettings();

                    // Check default setting is retrieved
                    Assert.IsTrue(settings.ServicePort == 22019);
                    Assert.IsTrue(db_readonly.GetSettings().ServicePort == 22019);
                    
                    settings.ServicePort = 1234;
                    bool success = db.SaveSettings(settings);
                    Assert.IsTrue(success = true);

                    Settings settings_after = db.GetSettings();
                    Assert.IsTrue(settings.ServicePort == 1234);
                    Assert.IsTrue(db_readonly.GetSettings().ServicePort == 1234);

                }
            }
        }

        [ClassCleanup]
        public static void RunOnceTearDown()
        {
            string db_path = DB.GetDbPath(UNIT_TEST_DB_NAME);
            if (File.Exists(db_path))
            {
                File.Delete(db_path);
            }
        }
    }
}
