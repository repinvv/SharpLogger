using System;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class ExceptionAppender : ILogConstructor
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

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            AppendException(sb, item.Ex);
        }
    }
}
