using System.Globalization;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class IDAppender : ILogConstructor
    {
        string idSplit;
        string bracketLeft;
        string bracketRight;

        public IDAppender(string idSplit, string bracketLeft, string bracketRight)
        {
            this.bracketLeft = bracketLeft;
            this.bracketRight = bracketRight;
            this.idSplit = string.IsNullOrEmpty(idSplit)
                ? " "
                : idSplit.Substring(0, 1);
        }

        public void ConstructLine(StringBuilder sb, LogItem item)
        {
            bool first = true;
            foreach (int id in item.Ids)
            {
                if (id != default(int))
                {
                    sb.Append(first ? bracketLeft : idSplit);
                    first = false;
                    sb.Append(id.ToString(CultureInfo.InvariantCulture));
                }
            }
            if (!first)
            {
                sb.Append(bracketRight);
            }
        }
    }
}
