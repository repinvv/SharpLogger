using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOptions;

namespace SharpLogger
{
    class CLCLevelAppender : ConfigurableLogConstructor
    {
        public override void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(LogLevel.GetLevel(item.level));
            next.ConstructLine(sb, item);
        }
    }
}
