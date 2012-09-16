using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOptions
{
   interface OptionsReader
   {
      bool CanRead { get; }
      IEnumerable<KeyValuePair<string, string>> ReadOptions();
      void WriteOptions(IEnumerable<KeyValuePair<string, string>> options);
   }
}
