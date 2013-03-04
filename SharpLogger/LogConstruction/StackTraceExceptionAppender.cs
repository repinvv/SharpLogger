using System;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class StackTraceExceptionAppender : ILogConstructor
    {
        private void AppendException(StringBuilder sb, Exception ex)
        {
            if (ex != null)
            {
                sb.Append('\n');
                sb.Append(ex.Message);
                sb.Append('\n');
                sb.Append(ex.StackTrace);
                AppendException(sb, ex.InnerException);
            }
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            AppendException(sb, item.Ex);
        }
    }
}
