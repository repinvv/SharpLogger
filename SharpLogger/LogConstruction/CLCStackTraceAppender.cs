using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
   class CLCStackTraceAppender : ConfigurableLogConstructor
   {

      public override void ConstructLine(StringBuilder sb, LogItem item)
      {
         if (item.ex != null)
            sb.Append(item.ex.StackTrace);
         next.ConstructLine(sb, item);
      }
   }
}
