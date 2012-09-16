using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
  class CLCDateTimeAppender :ConfigurableLogConstructor
  {
    string dateTimeFormat;
    string milisecondsFormat;

    public CLCDateTimeAppender(string dateTimeFormat, string milisecondsFormat)
    {
      this.dateTimeFormat = dateTimeFormat;
      this.milisecondsFormat = milisecondsFormat;
    }

    public override void ConstructLine(StringBuilder sb, LogItem item)
    {
      sb.Append(item.time.ToString(dateTimeFormat));
      sb.Append(String.Format(milisecondsFormat,item.time.Millisecond));
      next.ConstructLine(sb, item);
    }
  }
}
