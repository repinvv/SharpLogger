using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;
using System.Data.SqlClient;
using System.Data;

namespace SharpLogger
{
    class LogDatabaseWriter : ILogWriter
    {
        const string createTableQuery =
@"
if '{0}' not in 
  (SELECT name = TABLE_CATALOG+'.' + TABLE_SCHEMA + '.' + TABLE_NAME FROM INFORMATION_SCHEMA.TABLES)
begin 
  CREATE TABLE {0}(
	[DateTime] [datetime] NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
	[LogLevel] [int] NOT NULL,
	[Id] [int] NOT NULL,
	[Ids] [nvarchar](100) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[ExceptionMessage] [nvarchar](max) NOT NULL,
	[StackTrace] [nvarchar](max) NOT NULL
  ) ON [PRIMARY] 
end";
        const string insertQuery =
@"
insert into {0} values {1};";
        const string valueFormat =
@"(@date{0}, @cat{0}, @level{0}, @id{0}, @ids{0}, @message{0}, @ex{0}, @stack{0})";
        const string cleanupQuery =
@"
delete from {0} where DateTime < dateadd(day, -{1}, getdate())";

        string tableName;
        string connectionString;
        string miliSecondsFormat;
        char idSplitChar;
        int timeout;
        int daysToKeep;
        int messageBatch;
        TimeSpan forceFlush;
        DateTime lastFlush;
        DateTime lastCleanup;
        List<LogItem> messageList = new List<LogItem>();

        private void Query(string query, Action<SqlCommand> cAction)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        cAction(command);
                    }
                }
                catch (Exception)
                {
                    //fail silently
                }
            }
        }

        private void CreateTable()
        {
            Query(string.Format(createTableQuery, tableName),
                (command) =>
                { command.ExecuteNonQuery(); });
        }

        public LogDatabaseWriter(IOptions options)
        {
            connectionString = options["LogConnectionString"];
            tableName = options["LogWriteTable"];
            timeout = options.GetInt("LogDataFlush", 500);
            messageBatch = options.GetInt("LogMessageBatch");
            if (messageBatch > 50 || messageBatch < 1)
            {
                messageBatch = 2;
            }
            miliSecondsFormat = options["LogMilisecondsFormat"];
            string idSplit = options["LogIDSplitChar"];
            daysToKeep = options.GetInt("LogDaysToKeep");
            if (daysToKeep < 1 || daysToKeep > 365)
            {
                daysToKeep = 14;
            }
            if (idSplit == string.Empty)
            {
                idSplitChar = ' ';
            }
            else
            {
                idSplitChar = idSplit[0];
            }
            forceFlush = TimeSpan.FromSeconds((double)timeout / 500);
            lastFlush = DateTime.Now;
            lastCleanup = DateTime.Now - TimeSpan.FromHours(1);
            CreateTable();
        }


        public void Write(LogItem message)
        {
            messageList.Add(message);
            if (DateTime.Now - lastFlush > forceFlush || messageList.Count >= messageBatch)
            {
                Flush();
            }
        }

        private void Cleanup()
        {
            if (DateTime.Now - lastCleanup > TimeSpan.FromHours(1))
            {
                Query(string.Format(cleanupQuery, tableName, daysToKeep),
                (command) =>
                { command.ExecuteNonQuery(); });
            }
        }

        public void Flush()
        {
            Cleanup();
            if (messageList.Count == 0)
            {
                return;
            }
            StringBuilder values = new StringBuilder();

            for (int n = 0; n < messageList.Count; n++)
            {
                if (n != 0)
                {
                    values.Append(", ");
                }
                values.Append(string.Format(valueFormat, n));
            }

            Query(string.Format(insertQuery, tableName, values.ToString()),
            (command) =>
            {
                for (int n = 0; n < messageList.Count; n++)
                {
                    command.Parameters.Add("@date" + n, SqlDbType.DateTime).Value = messageList[n].time;
                    command.Parameters.Add("@cat" + n, SqlDbType.NVarChar, 100).Value = messageList[n].category;
                    command.Parameters.Add("@level" + n, SqlDbType.Int).Value = messageList[n].level;
                    command.Parameters.Add("@id" + n, SqlDbType.Int).Value = messageList[n].ids[0];
                    command.Parameters.Add("@message" + n, SqlDbType.NVarChar).Value = messageList[n].message;
                    string exMessage = messageList[n].ex == null ? string.Empty : messageList[n].ex.Message;
                    string stackTrace = messageList[n].ex == null ? string.Empty : messageList[n].ex.StackTrace;
                    command.Parameters.Add("@ex" + n, SqlDbType.NVarChar).Value = exMessage;
                    command.Parameters.Add("@stack" + n, SqlDbType.NVarChar).Value = stackTrace;
                    values.Clear();
                    bool first = true;
                    foreach (int id in messageList[n].ids)
                    {
                        if (id != default(int))
                        {
                            if (!first)
                            {
                                values.Append(idSplitChar);
                            }
                            first = false;
                            values.Append(id.ToString());
                        }
                    }
                    command.Parameters.Add("@ids" + n, SqlDbType.NVarChar, 100).Value = values.ToString();
                }
                command.ExecuteNonQuery();
            });
            messageList.Clear();
        }

        public int GetTimeout()
        {
            return timeout;
        }
    }
}
