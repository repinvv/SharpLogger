using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpLogger;
using SharpOptions;
using System.Threading;

namespace DatabaseTest
{
    class Program
    {
        static void CreateSqlDatabase(string path, string dbName)
        {
            string filename = string.Format("{0}\\{1}.mdf", path, dbName);
            string connectionString = @"Data Source=(LocalDb)\v11.0;Initial Catalog=master";
            if (!File.Exists(filename))
            {

                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText =
                            String.Format("CREATE DATABASE {0} ON PRIMARY (NAME={0}, FILENAME='{1}')", dbName, filename);
                        command.ExecuteNonQuery();

                        command.CommandText =
                            String.Format("EXEC sp_detach_db '{0}', 'true'", dbName);
                        command.ExecuteNonQuery();
                    }
                }
            }
            //return string.Format("{0};AttachDBFilename={1}", connectionString, filename);
        }
        static int subsubmain(int b)
        {
            int a = 7;
            return a / b;
        }

        static int submain(int b)
        {
            int a = 5;
            return a / subsubmain(b);
        }

        static void Main(string[] args)
        {
            string pwd = Directory.GetCurrentDirectory();
            string dbName = "LoggerTest";
            CreateSqlDatabase(pwd, dbName);
            string connectionString =
                string.Format(@"Data Source=(LocalDb)\v11.0;Initial Catalog={0};AttachDBFilename={1}\{2}.mdf"
                , dbName, pwd, dbName);

            IOptions options = new Options(connectionString, dbName + ".dbo.LogOptions", OptionsReaderType.Database);
            options["LogWriteTarget"] = "Database";
            options["LogConnectionString"] = connectionString;
            options["LogWriteTable"] = dbName + ".dbo.LogOutput";
            options["LogDaysToKeep"] = "1";
            LogAccess.Init(options);
            options.Save();

            var logger = LogAccess.GetLogger("alogger");
            for (int n = 0; n < 30; n++)
            {
                try
                {
                    Console.Write(submain(0));
                }
                catch (Exception ex)
                {
                    logger.Error("happy error", ex);
                }
            }
            for (int n = 0; n < 10000; n++)
            {
                logger.Info("info" + n, new int[] { n, 2, 3 });
            }
            Console.ReadKey();
        }
    }
}
