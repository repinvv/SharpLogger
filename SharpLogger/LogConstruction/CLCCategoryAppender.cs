using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class CLCCategoryAppender : ConfigurableLogConstructor
    {
        public override void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(item.category);
            next.ConstructLine(sb, item);
        }
    }
}
