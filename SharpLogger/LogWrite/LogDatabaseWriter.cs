using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;
using System.Data.SqlClient;

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
insert into {0}
values {1};
";

        string tableName;
        string connectionString;
        string dateFormat;
        string miliSecondsFormat;
        char idSplitChar;
        int timeout;
        TimeSpan forceFlush;
        DateTime lastFlush;
        List<LogItem> messageList = new List<LogItem>();

        private void CreateTable()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (var command = new SqlCommand(string.Format(createTableQuery, tableName), connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception)
                {
                    //fail silently
                }
            }
        }

        public LogDatabaseWriter(IOptions options)
        {
            connectionString = options["LogConnectionString"];
            tableName = options["LogWriteTable"];
            timeout = options.GetInt("LogDataFlush", 500);
            dateFormat = options["LogDateTimeFormat"];
            miliSecondsFormat = options["LogMilisecondsFormat"];
            string idSplit = options["LogIDSplitChar"];
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
            CreateTable();
        }


        public void Write(LogItem message)
        {
            messageList.Add(message);
            if (DateTime.Now - lastFlush > forceFlush)
            {
                Flush();
            }
        }

        private string EscapeString(string input)
        {
            return input
                .Replace("\'", "\'\'");
                //.Replace("[", "[[");
        }

        public void Flush()
        {
            StringBuilder values = new StringBuilder();
            bool first = true;
            foreach (var message in messageList)
            {
                if (!first)
                {
                    values.Append(", ");
                }
                first = false;

                values.Append("('");    //datetime
                values.Append(message.time.ToString(dateFormat));
                values.Append(string.Format(miliSecondsFormat, message.time.Millisecond));
                values.Append("', N'"); //category
                values.Append(EscapeString(message.category));
                values.Append("', ");   //loglevel
                values.Append(message.level);
                values.Append(", ");    //id
                values.Append(message.ids[0]);
                values.Append(", N'");  //ids
                bool firstId = true;
                foreach (int id in message.ids)
                {
                    if (id != default(int))
                    {
                        if (!firstId)
                        {
                            values.Append(idSplitChar);
                        }
                        firstId = false;
                        values.Append(id.ToString());
                    }
                }
                values.Append("', N'"); //message
                values.Append(EscapeString(message.message));
                values.Append("', N'"); //Exception
                if (message.ex != null)
                {
                    values.Append(EscapeString(message.ex.Message));
                }
                values.Append("', N'"); //stacktrace
                if (message.ex != null)
                {
                    values.Append(EscapeString(message.ex.StackTrace));
                }
                values.Append("')");
            }

            if (!first)
            {
                string query = string.Format(insertQuery, tableName, values.ToString());
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                            messageList.Clear();
                        }
                    }
                    
                    catch (Exception)
                    {
                        //fail silently as always
                    }
                }
            }
        }

        public int GetTimeout()
        {
            return timeout;
        }
    }
}
