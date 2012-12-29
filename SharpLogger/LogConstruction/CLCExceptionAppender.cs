using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLogger
{
    class CLCExceptionAppender : ConfigurableLogConstructor
    {

        private void AppendException(StringBuilder sb, Exception ex)
        {
            if (ex != null)
            {
                sb.Append('\n');
                sb.Append(ex.Message);
                AppendException(sb, ex.InnerException);
            }
        }

        public override void ConstructLine(StringBuilder sb, LogItem item)
        {
            AppendException(sb, item.ex);
            next.ConstructLine(sb, item);
        }
    }
}
