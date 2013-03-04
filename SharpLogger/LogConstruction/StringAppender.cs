using System;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class StringAppender : ILogConstructor
    {
        string _staticString;

        public StringAppender(String staticString)
        {
            _staticString = staticString;
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(_staticString);
        }
    }
}
