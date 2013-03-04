using System.Text;

namespace SharpLogger.LogConstruction
{
    class CategoryAppender : ILogConstructor
    {
        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            sb.Append(item.Category);
        }
    }
}
