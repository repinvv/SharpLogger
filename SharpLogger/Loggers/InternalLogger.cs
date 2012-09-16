using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
   interface InternalLogger : Logger
   {
      void SetLevel(int level);

      bool this[int level] {set;}
   }
}
