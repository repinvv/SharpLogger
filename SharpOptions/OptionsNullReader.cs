using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOptions
{
    class OptionsNullReader : OptionsReader
    {
        public bool CanRead
        {
            get { return true; }
        }

        public IEnumerable<KeyValuePair<string, string>> ReadOptions()
        {
            return new List<KeyValuePair<string, string>>();
        }

        public void WriteOptions(IEnumerable<KeyValuePair<string, string>> options)
        {

        }
    }
}
