using System.Globalization;
using System.Text;

namespace SharpLogger.LogConstruction
{
    class IDAppender : ILogConstructor
    {
        string _idSplit;
        string _bracketLeft;
        string _bracketRight;

        public IDAppender(string idSplit, string bracketLeft, string bracketRight)
        {
            _bracketLeft = bracketLeft;
            _bracketRight = bracketRight;
            _idSplit = string.IsNullOrEmpty(idSplit)
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
                    sb.Append(first ? _bracketLeft : _idSplit);
                    first = false;
                    sb.Append(id.ToString(CultureInfo.InvariantCulture));
                }
            }
            if (!first)
            {
                sb.Append(_bracketRight);
            }
        }
    }
}
