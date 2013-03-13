using System;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class StringAppender : ILogConstructor
    {
        string staticString;

        public StringAppender(String staticString)
        {
            this.staticString = staticString;
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(staticString);
        }
    }
}
