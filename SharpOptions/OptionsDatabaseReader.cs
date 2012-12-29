using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SharpOptions
{
    class OptionsDatabaseReader : OptionsReader
    {
        const string createTableQuery =
@"
if '{0}' not in 
  (SELECT name = TABLE_CATALOG+'.' + TABLE_SCHEMA + '.' + TABLE_NAME FROM INFORMATION_SCHEMA.TABLES)
begin 
  CREATE TABLE {0}(
 [Name] [nvarchar](50) NOT NULL,
 [Value] [nvarchar](max) NOT NULL,
 ) ON [PRIMARY] 
end";
        const string insertReplaceQuery =
@"
MERGE INTO {0} AS Target
USING (VALUES {1})
       AS Source (NewName, NewValue)
ON Target.Name = Source.NewName
WHEN MATCHED THEN
	UPDATE SET Value = Source.NewValue
WHEN NOT MATCHED BY TARGET THEN
	INSERT (Name, Value) VALUES (NewName, NewValue);";

        string connectionString;
        string tableName;

        public OptionsDatabaseReader(string connectionString, string tableName)
        {
            this.connectionString = connectionString;
            this.tableName = tableName;
        }

        public bool CanRead
        {
            get
            {
                //will avoid double read.
                return true;
            }
        }

        void CreateTable(SqlConnection connection)
        {
            string query = string.Format(createTableQuery, tableName);
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        void ReadList(SqlConnection connection, List<KeyValuePair<string, string>> list)
        {
            using (var command = new SqlCommand("select Name, Value from " + tableName, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        string value = reader.GetString(1);
                        if (!string.IsNullOrEmpty(name))
                        {
                            list.Add(new KeyValuePair<string, string>(name, value));
                        }
                    }
                }
            }
        }

        public IEnumerable<KeyValuePair<string, string>> ReadOptions()
        {
            var list = new List<KeyValuePair<string, string>>();            
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    CreateTable(connection);
                    ReadList(connection, list);
                }
                catch (Exception)
                {
                    //fail silently
                }
            }
            return list;
        }

        public void WriteOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            StringBuilder values = new StringBuilder();
            bool first = true;
            foreach (var pair in options)
            {
                if (!first)
                {
                    values.Append(", ");
                }
                first = false;
                values.Append("(N'");
                values.Append(pair.Key);
                values.Append("', N'");
                values.Append(pair.Value);
                values.Append("')");
            }

            if (!first)
            {
                string query = string.Format(insertReplaceQuery, tableName, values.ToString());
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception)
                    {
                        //fail silently as always
                    }
                }
            }
        }
    }
}
