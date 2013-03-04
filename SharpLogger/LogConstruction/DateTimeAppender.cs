using System;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class DateTimeAppender : ILogConstructor
    {
        string _dateTimeFormat;
        string _milisecondsFormat;

        public DateTimeAppender(string dateTimeFormat, string milisecondsFormat)
        {
            _dateTimeFormat = dateTimeFormat;
            _milisecondsFormat = milisecondsFormat;
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(item.Time.ToString(_dateTimeFormat));
            sb.Append(String.Format(_milisecondsFormat, item.Time.Millisecond));
        }
    }
}
