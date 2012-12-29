using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class CLCStringAppender : ConfigurableLogConstructor
    {
        string staticString;

        public CLCStringAppender(String staticString)
        {
            this.staticString = staticString;
        }

        public override void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(staticString);
            next.ConstructLine(sb, item);
        }

        public override string ToString()
        {
            return base.ToString() + ".Value=\"" + staticString + "\"";
        }
    }
}
