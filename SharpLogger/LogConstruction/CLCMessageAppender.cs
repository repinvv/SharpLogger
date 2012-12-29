using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class CLCMessageAppender : ConfigurableLogConstructor
    {

        public override void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(item.message);
            next.ConstructLine(sb, item);
        }
    }
}
