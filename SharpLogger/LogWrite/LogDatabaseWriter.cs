using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SharpOptions;
using System.Data.SqlClient;
using System.Data;

namespace SharpLogger.LogWrite
{
    class LogDatabaseWriter : ILogWriter
    {
        const string CreateTableQuery =
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
        const string InsertQuery =
@"
insert into {0} values {1};";
        const string ValueFormat =
@"(@date{0}, @cat{0}, @level{0}, @id{0}, @ids{0}, @message{0}, @ex{0}, @stack{0})";
        const string CleanupQuery =
@"
delete from {0} where DateTime < dateadd(day, -{1}, getdate())";

        string _tableName;
        string _connectionString;
        char _idSplitChar;
        int _timeout;
        int _daysToKeep;
        int _messageBatch;
        TimeSpan _forceFlush;
        DateTime _lastFlush;
        DateTime _lastCleanup;
        List<LogItem> _messageList = new List<LogItem>();

        private void Query(string query, Action<SqlCommand> cAction)
        {
            using (var connection = new SqlConnection(_connectionString))
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
            Query(string.Format(CreateTableQuery, _tableName),
                command => command.ExecuteNonQuery());
        }

        public LogDatabaseWriter(IOptions options)
        {
            _connectionString = options["LogConnectionString"];
            _tableName = options["LogWriteTable"];
            _timeout = options.GetInt("LogDataFlush", 500);
            _messageBatch = options.GetInt("LogMessageBatch");
            if (_messageBatch > 50 || _messageBatch < 1)
            {
                _messageBatch = 2;
            }
            string idSplit = options["LogIDSplitChar"];
            _daysToKeep = options.GetInt("LogDaysToKeep");
            if (_daysToKeep < 1 || _daysToKeep > 365)
            {
                _daysToKeep = 14;
            }
            _idSplitChar = idSplit == string.Empty ? ' ' : idSplit[0];
            _forceFlush = TimeSpan.FromSeconds((double)_timeout / 500);
            _lastFlush = DateTime.Now;
            _lastCleanup = DateTime.Now - TimeSpan.FromHours(1);
            CreateTable();
        }


        public void Write(LogItem message)
        {
            _messageList.Add(message);
            if (DateTime.Now - _lastFlush > _forceFlush || _messageList.Count >= _messageBatch)
            {
                Flush();
            }
        }

        private void Cleanup()
        {
            if (DateTime.Now - _lastCleanup > TimeSpan.FromHours(1))
            {
                Query(string.Format(CleanupQuery, _tableName, _daysToKeep),
                command => command.ExecuteNonQuery());
            }
        }

        public void Flush()
        {
            Cleanup();
            if (_messageList.Count == 0)
            {
                return;
            }
            var values = new StringBuilder();

            for (int n = 0; n < _messageList.Count; n++)
            {
                if (n != 0)
                {
                    values.Append(", ");
                }
                values.Append(string.Format(ValueFormat, n));
            }

            Query(string.Format(InsertQuery, _tableName, values),
            command =>
            {
                for (int n = 0; n < _messageList.Count; n++)
                {
                    command.Parameters.Add("@date" + n, SqlDbType.DateTime).Value = _messageList[n].Time;
                    command.Parameters.Add("@cat" + n, SqlDbType.NVarChar, 100).Value = _messageList[n].Category;
                    command.Parameters.Add("@level" + n, SqlDbType.Int).Value = _messageList[n].Level;
                    command.Parameters.Add("@id" + n, SqlDbType.Int).Value = _messageList[n].Ids[0];
                    command.Parameters.Add("@message" + n, SqlDbType.NVarChar).Value = _messageList[n].Message;
                    string exMessage = _messageList[n].Ex == null ? string.Empty : _messageList[n].Ex.Message;
                    string stackTrace = _messageList[n].Ex == null ? string.Empty : _messageList[n].Ex.StackTrace;
                    command.Parameters.Add("@ex" + n, SqlDbType.NVarChar).Value = exMessage;
                    command.Parameters.Add("@stack" + n, SqlDbType.NVarChar).Value = stackTrace;
                    values.Clear();
                    bool first = true;
                    foreach (int id in _messageList[n].Ids)
                    {
                        if (id != default(int))
                        {
                            if (!first)
                            {
                                values.Append(_idSplitChar);
                            }
                            first = false;
                            values.Append(id.ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    command.Parameters.Add("@ids" + n, SqlDbType.NVarChar, 100).Value = values.ToString();
                }
                command.ExecuteNonQuery();
            });
            _messageList.Clear();
        }

        public int GetTimeout()
        {
            return _timeout;
        }
    }
}
