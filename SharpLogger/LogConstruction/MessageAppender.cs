using System.Text;

namespace SharpLogger.LogConstruction
{
    class MessageAppender : ILogConstructor
    {
        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(item.Message);
        }
    }
}
