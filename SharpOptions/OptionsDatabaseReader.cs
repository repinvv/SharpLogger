using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOptions
{
    class OptionsDatabaseReader : OptionsReader
    {

        public OptionsDatabaseReader(string connectionString, string tableName)
        {
        }

        public bool CanRead
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<KeyValuePair<string, string>> ReadOptions()
        {
            throw new NotImplementedException();
        }

        public void WriteOptions(IEnumerable<KeyValuePair<string, string>> options)
        {
            throw new NotImplementedException();
        }
    }
}
