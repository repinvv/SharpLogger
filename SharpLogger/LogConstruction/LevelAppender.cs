using System.Text;

namespace SharpLogger.LogConstruction
{
    class LevelAppender : ILogConstructor
    {
        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(LogLevel.GetLevel(item.Level));
        }
    }
}
