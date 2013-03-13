using System;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class DateTimeAppender : ILogConstructor
    {
        string dateTimeFormat;
        string milisecondsFormat;

        public DateTimeAppender(string dateTimeFormat, string milisecondsFormat)
        {
            this.dateTimeFormat = dateTimeFormat;
            this.milisecondsFormat = milisecondsFormat;
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(item.Time.ToString(dateTimeFormat));
            sb.Append(String.Format(milisecondsFormat, item.Time.Millisecond));
        }
    }
}
